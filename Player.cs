using Godot;
using System;
using System.Collections.Generic;

public enum PlayerProperty {
	Health,
	MaxHealth,
	Mana,
	MaxMana
}

public class Property<T> where T : IComparable {
	public delegate void ChangedDel(); 

#nullable enable
	public Property<T>? Max { get; set; }
	public Property<T>? Min { get; set; }
#nullable disable

	private T _v;
	public event ChangedDel Changed;
	public T Value { get => _v; set {
		_v = value;
		if (Min is not null && _v.CompareTo(Min.Value) < 0)
			_v = Min.Value;
		if (Max is not null && _v.CompareTo(Max.Value) > 0)
			_v = Max.Value;
		Changed?.Invoke();
	}}
}

public partial class Player : CharacterBody3D
{
	[Signal]
	public delegate void UpdateLabelEventHandler(int id, string text);
	[Signal]
	public delegate void GoldAmountChangedEventHandler(int amount);
	[Signal]
	public delegate void ToggleOpenInventoryEventHandler(Player player, bool open);
	[Signal]
	public delegate void AddItemToInventoryEventHandler(Player player, InventoryItemBase item);
	[Signal]
	public delegate void RemoveItemFromInventoryEventHandler(ItemResource item, int inventoryIdx);
//	[Signal]
//	public delegate void HealthChangedEventHandler(int health);
//	[Signal]
//	public delegate void ManaChangedEventHandler(int mana);
//	[Signal]
//	public delegate void MaxHealthChangedEventHandler(int maxHealth);
//	[Signal]
//	public delegate void MaxManaChangedEventHandler(int maxMana);
	[Signal]
	public delegate void ClickedMoveToEventHandler(Vector3 position);
	[Signal]
	public delegate void PropertyChangedEventHandler(int property, int v);
	
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	
	private Node3D MeshNode;
	private Camera3D CameraNode;
	private AnimationTree AnimationsNode;
	private NavigationAgent3D NavigationAgentNode;
	
	private bool _fullscreen = false;
	private readonly float _zoomYMin = 2;
	private readonly float _zoomYMax = 12;
	
	public List<IItem> FocusedItems { get; } = new();
	
	// TODO? should always be int
	public Dictionary<PlayerProperty, Property<int>> Properties = new();
	
	private int _curItemI;
	public int CurItemI {
		get => _curItemI;
		set {
			// TODO set the current item to viewed false
			// TODO for now just set all to false, then set the current one to true
			foreach (var item in FocusedItems) item.SetViewed(false);
			_curItemI = value;
			if (_curItemI < 0) _curItemI = FocusedItems.Count - 1;
			if (_curItemI >= FocusedItems.Count) _curItemI = 0;
			if (FocusedItems.Count > 0) FocusedItems[_curItemI].SetViewed(true);
		}
	}
	
	private int _gold;
	public int Gold { get => _gold; set {
		_gold = value;
		EmitSignal(SignalName.GoldAmountChanged, _gold);
	} }
	
	public int Health { 
		get => Properties[PlayerProperty.Health].Value; 
		set => Properties[PlayerProperty.Health].Value = value;
	}
	
	public int MaxHealth { 
		get => Properties[PlayerProperty.MaxHealth].Value; 
		set => Properties[PlayerProperty.MaxHealth].Value = value;
	}
	
	public int Mana { 
		get => Properties[PlayerProperty.Mana].Value; 
		set => Properties[PlayerProperty.Mana].Value = value;
	}
	
	public int MaxMana { 
		get => Properties[PlayerProperty.MaxMana].Value; 
		set => Properties[PlayerProperty.MaxMana].Value = value;
	}
	

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	
	private bool _moveTo = false;
	private bool _inventoryOpen = false;
	
	
	public override void _Ready() {
		MeshNode = GetNode<Node3D>("%Mesh");
		CameraNode = GetNode<Camera3D>("%Camera");
		AnimationsNode = GetNode<AnimationTree>("%Animations");
		NavigationAgentNode = GetNode<NavigationAgent3D>("%NavigationAgent");

		// create properties
		var maxHealthProperty = new Property<int>();
		Properties.Add(PlayerProperty.MaxHealth, maxHealthProperty);
		maxHealthProperty.Value = 100;
		maxHealthProperty.Changed += () => EmitSignal(SignalName.PropertyChanged, (int)PlayerProperty.MaxHealth, maxHealthProperty.Value);
		
		var minHealthProperty = new Property<int>();
		minHealthProperty.Value = 0;
		
		var maxManaProperty = new Property<int>();
		maxManaProperty.Value = 100;
		Properties.Add(PlayerProperty.MaxMana, maxManaProperty);
		maxManaProperty.Changed += () => EmitSignal(SignalName.PropertyChanged, (int)PlayerProperty.MaxMana, maxManaProperty.Value);
		
		var minManaProperty = new Property<int>();
		minManaProperty.Value = 0;
		
		var healthProperty = new Property<int>();
		healthProperty.Max = maxHealthProperty;
		healthProperty.Min = minHealthProperty;
		healthProperty.Value = 50;
		healthProperty.Changed += () => EmitSignal(SignalName.PropertyChanged, (int)PlayerProperty.Health, healthProperty.Value);
		Properties.Add(PlayerProperty.Health, healthProperty);
		
		var manaProperty = new Property<int>();
		manaProperty.Max = maxManaProperty;
		manaProperty.Min = minManaProperty;
		manaProperty.Value = 50;
		manaProperty.Changed += () => EmitSignal(SignalName.PropertyChanged, (int)PlayerProperty.Mana, manaProperty.Value);
		Properties.Add(PlayerProperty.Mana, manaProperty);
		
		Gold = 0;
		Health = 50;
		Mana = 50;

	}
	
	public override void _Input(InputEvent e) {
		if (e.IsActionPressed("zoom_in")) {
			HandleZoom(4);
			
		}
		if (e.IsActionPressed("zoom_out")) {
			HandleZoom(-4);
		}
		if (e.IsActionPressed("toggle_fullscreen")) {
			var flag = DisplayServer.WindowMode.Fullscreen;
			if (_fullscreen) flag = DisplayServer.WindowMode.Windowed;
			DisplayServer.WindowSetMode(flag);
			_fullscreen = !_fullscreen;
		}
		if (!_inventoryOpen && e.IsActionPressed("move_to")) {
			var spaceState = GetWorld3D().DirectSpaceState;
			// TODO crashes app on exit ???
			var mousePos = GetViewport().GetMousePosition();
			var rayOrigin = CameraNode.ProjectRayOrigin(mousePos);
			var rayEnd = rayOrigin + CameraNode.ProjectRayNormal(mousePos) * 2000;
			var query = PhysicsRayQueryParameters3D.Create(rayOrigin, rayEnd);
			var rayArr = spaceState.IntersectRay(query);
			if (rayArr.ContainsKey("position")) {
				var collider = rayArr["collider"].As<Node>();
				if (!collider.IsInGroup("moveable_surface")) return;
				var pos = rayArr["position"];
				var v = pos.As<Vector3>();
				var target = new Vector3(v.X, GlobalPosition.Y, v.Z);
//				GlobalPosition = target;
				NavigationAgentNode.TargetPosition = target;
				_moveTo = true;
				EmitSignal(SignalName.ClickedMoveTo, target);
			}
		}
		if (e.IsActionPressed("scroll_up_focused_items")) {
			CurItemI += 1;
		}
		if (e.IsActionPressed("scroll_down_focused_items")) {
			CurItemI -= 1;
		}
		if (e.IsActionPressed("pickup_item")) {
			PickUpCurrentItem();
		}
		if (e.IsActionPressed("open_inventory")) {
			EmitSignal(SignalName.ToggleOpenInventory, this, _inventoryOpen);
			_inventoryOpen = !_inventoryOpen;
		}
	}
	
	private void HandleZoom(float v) {
		var d = CameraNode.Position.DirectionTo(MeshNode.Position);
		var target = CameraNode.Position + (-d * v);
		if (target.Y > _zoomYMax || target.Y < _zoomYMin) return;
		
		CreateTween()
			.TweenProperty(CameraNode, "position", target, .3f)
			.SetTrans(Tween.TransitionType.Quad);
	}

	public override void _PhysicsProcess(double delta)
	{
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("move_up", "move_down", "move_right", "move_left");
//		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");

//		Vector3 v = new Vector3((inputDir.Y + inputDir.X) / 2, 0, inputDir.X - inputDir.Y);
//		Vector3 v = new Vector3(inputDir.Y, 0, -inputDir.X);
		Vector3 v = new Vector3(inputDir.X, 0, inputDir.Y);

		Vector3 direction = (Transform.Basis * v).Normalized();

//		MoveTo(direction, delta);

		var velocity = Velocity;
		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
//		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
//			velocity.Y = JumpVelocity;
		
		if (direction != Vector3.Zero)
		{
			_moveTo = false;
			TurnToVelocity();
			
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		var idling = Velocity == Vector3.Zero;
		AnimationsNode.Set("parameters/conditions/idle", idling);
		AnimationsNode.Set("parameters/conditions/run", !idling);
		
		MoveAndSlide();
	}
	
	private void TurnToVelocity() {
		var target = MathF.Atan2(Velocity.X,Velocity.Z);
		var diff = (float)Mathf.Wrap(target - MeshNode.Rotation.Y, -Math.PI, Math.PI);
		CreateTween().TweenProperty(MeshNode, "rotation", new Vector3(MeshNode.Rotation.X, MeshNode.Rotation.Y + diff, MeshNode.Rotation.Z), .1f);			
	}
	
	public override void _Process(double delta) {
		if (!NavigationAgentNode.IsNavigationFinished() && _moveTo) {
			var pos = NavigationAgentNode.GetNextPathPosition();
			var dir = GlobalPosition.DirectionTo(pos);
			Velocity = dir * Speed;
			TurnToVelocity();
			
			
			var idling = Velocity == Vector3.Zero;
			AnimationsNode.Set("parameters/conditions/idle", idling);
			AnimationsNode.Set("parameters/conditions/run", !idling);
			MoveAndSlide();
//			MoveTo(pos, delta);
		}
	}
	
	private void OnItemPickupAreaBodyEntered(Node3D body)
	{
		// TODO works fine, just not when the navigation pointer is very close
		if (body.IsInGroup("dissapear_on_player_collide"))
			body.Visible = false;
		switch (body) {
			case IItem item:
				AddFocusItem(item);
				break;
		}
	}
	
	private void OnItemPickupAreaBodyExited(Node3D body)
	{
		switch (body) {
			case IItem item:
				RemoveFocusItem(item);
				break;
		}
	}
	
	private void AddFocusItem(IItem item) {
		item.OnEnterFocus(this);
		
		FocusedItems.Add(item);
		CurItemI = FocusedItems.Count - 1;
		
	}
	
	private void RemoveFocusItem(IItem item) {
		item.OnLeaveFocus(this);
		
		FocusedItems.Remove(item);
		if (FocusedItems.Count == 0) return;
		
		// TODO
		if (CurItemI >= FocusedItems.Count)
			CurItemI = FocusedItems.Count - 1;
			
	}
	
	private void PickUpCurrentItem() {
		if (FocusedItems.Count == 0) return;
		
		var item = FocusedItems[CurItemI];
		item.OnPickUp(this);
		
		CurItemI -= 1;
	}
	
//	private bool _inventoryOpen = false;
//	private void ToggleInventory() {
//		var target = 0;
//		if (_inventoryOpen) target = DisplayServer.ScreenGetSize().Y;
//
//		_inventoryOpen = !_inventoryOpen;
//	}

	public void AddToInventory(InventoryItemBase item) {
		EmitSignal(SignalName.AddItemToInventory, this, item);
	}
	
	public void RemoveFromInventory(ItemResource item, int inventoryIdx) {
		EmitSignal(SignalName.RemoveItemFromInventory, item, inventoryIdx);
	}
}





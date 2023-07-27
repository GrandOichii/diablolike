using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Signal]
	public delegate void UpdateLabelEventHandler(int id, string text);
	
//	[Signal]
//	public delegate void ClickedMoveToEventHandler(Vector3 position);
	
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	
	private Node3D MeshNode;
	private Camera3D CameraNode;
	private AnimationTree AnimationsNode;
	private NavigationAgent3D NavigationAgentNode;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	
	[Export]
	private Vector3 MoveTransform = new(1, 1, 1);
	
	public override void _Ready() {
		MeshNode = GetNode<Node3D>("%Mesh");
		CameraNode = GetNode<Camera3D>("%Camera");
		AnimationsNode = GetNode<AnimationTree>("%Animations");
		NavigationAgentNode = GetNode<NavigationAgent3D>("%NavigationAgent");
	}
	
	private bool _fullscreen = false;
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
		if (e.IsActionPressed("move_to")) {
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
			}
			// TODO just realised that don't need to rotate anything for isometric look, only the plane itself
		}
		
	}
	
	private readonly float _zoomYMin = 4;
	private readonly float _zoomYMax = 12;
	private void HandleZoom(float v) {
		var d = CameraNode.Position.DirectionTo(MeshNode.Position);
		var target = CameraNode.Position + (-d * v);
		if (target.Y > _zoomYMax || target.Y < _zoomYMin) return;
		
		GD.Print(target);
		CreateTween()
			.TweenProperty(CameraNode, "position", target, .3f)
			.SetTrans(Tween.TransitionType.Quad);
//		GD.Print();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
//		Vector2 inputDir = Input.GetVector("move_up", "move_down", "move_left", "move_right");
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");

//		Vector3 v = new Vector3((inputDir.Y + inputDir.X) / 2, 0, inputDir.X - inputDir.Y);
		Vector3 v = new Vector3(inputDir.Y, 0, -inputDir.X);

		Vector3 direction = (MoveTransform * v).Normalized();
		
//		MoveTo(direction);

		if (direction != Vector3.Zero)
		{
			var target = MathF.Atan2(Velocity.X,Velocity.Z);
			var diff = (float)Mathf.Wrap(target - MeshNode.Rotation.Y, -Math.PI, Math.PI);
			CreateTween().TweenProperty(MeshNode, "rotation", new Vector3(MeshNode.Rotation.X, MeshNode.Rotation.Y + diff, MeshNode.Rotation.Z), .1f);			
			
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		var idling = inputDir == Vector2.Zero;
		AnimationsNode.Set("parameters/conditions/idle", idling);
		AnimationsNode.Set("parameters/conditions/run", !idling);
		
		MoveAndSlide();
	}
	
	public override void _Process(double delta) {
		
	}
	
	private void MoveTo(Vector3 direction, float delta) {
		
	}
}

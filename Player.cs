using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Signal]
	public delegate void UpdateLabelEventHandler(int id, string text);
	
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	
	private Node3D MeshNode;
	private Camera3D CameraNode;
	private AnimationTree AnimationsNode;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	
	[Export]
	private Vector3 MoveTransform = new(1, 1, 1);
	
	public override void _Ready() {
		MeshNode = GetNode<Node3D>("%Mesh");
		CameraNode = GetNode<Camera3D>("%Camera");
		AnimationsNode = GetNode<AnimationTree>("%Animations");
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
		Vector2 inputDir = Input.GetVector("move_up", "move_down", "move_left", "move_right");
		Vector3 v = new Vector3((inputDir.Y + inputDir.X) / 2, 0, inputDir.X - inputDir.Y);
//		Vector3 v = new Vector3(inputDir.Y - inputDir.X, 0, (inputDir.Y + inputDir.X*2) / 2);

		Vector3 direction = (MoveTransform * v).Normalized();

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
}

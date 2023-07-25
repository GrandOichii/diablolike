using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	
	private Node3D Mesh;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	
	[Export]
	private Vector3 MoveTransform = new(1, 1, 1);
	
	public override void _Ready() {
		Mesh = GetNode<Node3D>("%Mesh");
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
		Vector3 v = new Vector3((inputDir.Y*2 + inputDir.X) / 2, 0, inputDir.X - inputDir.Y);
//		Vector3 v = new Vector3(inputDir.Y - inputDir.X, 0, (inputDir.Y + inputDir.X*2) / 2);

		var mt = MoveTransform * v;
		Vector3 direction = (MoveTransform * v).Normalized();
		Mesh.Rotation = new(Mesh.Rotation.X, mt.Z, Mesh.Rotation.Z);
		// Rotation = new(Input.GetActionStrength("move_left") - Input.GetActionStrength("move_right"), 0, Input.GetActionStrength("move_forward") - Input.GetActionStrength("move_backward"));
//		Vector3 direction = (MoveTransform * new Vector3(inputDir.X, 0, inputDir.Y))
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}

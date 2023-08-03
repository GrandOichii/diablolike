using Godot;
using System;

// [Tool]
public partial class Zombie : EnemyBase
{
	protected override void OnDetectionAreaBodyEntered(Node3D body)
	{
		if (body is not Player) return;
	}
}

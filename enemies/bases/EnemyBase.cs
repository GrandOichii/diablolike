using Godot;
using System;

//[Tool]
public partial class EnemyBase : CharacterBody3D
{
	[Export]
	public Node3D MeshNode { get; set; }
	[Export]
	public int Experience { get; set; }

	[Export]
	public EnemyBehaviourResource Behaviour { get; set; }
	
	// TODO throws exceptions on startup
	[Export]
	public float DetectionRadius {
		get => DetectionCollision is not null ? (DetectionCollision.Shape as CylinderShape3D).Radius : 0;
		set { 
			if (DetectionCollision is not null) 
				(DetectionCollision.Shape as CylinderShape3D).Radius = value;
		}
	}
	
	public CollisionShape3D Collision { get; set; }
	public CollisionShape3D DetectionCollision { get; set; }
	public NavigationAgent3D NavAgent { get; set; }
	
	public override void _Ready() {
		Collision = GetNode<CollisionShape3D>("%Collision");
		DetectionCollision = GetNode<CollisionShape3D>("%DetectionCollision");
		NavAgent = GetNode<NavigationAgent3D>("%NavigationAgent");
		
		Behaviour.Ready(this);
	}
	
	protected virtual void OnDetectionAreaBodyEntered(Node3D body)
	{
//		GD.Print("anogs");
		Behaviour.OnBodyDetected(body);
	}
	
	public override void _Process(double delta) {
		Behaviour.Process(this, delta);
	}
}



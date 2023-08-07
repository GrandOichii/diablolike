using Godot;
using System;


public partial class ChaseStateResource : StateResource {
	[Export]
	private float Speed { get; set; } = 3;
	
	public override void Process(EnemyBase controlled, bool isActive, double delta) {
		if (!isActive) {
			if (!controlled.Behaviour.HasValue("player"))
				return;
			controlled.Behaviour.CurrentState = (int)SearchAndChaseBehaviourResource.BStates.Chase;
		}
		var navAgent = controlled.NavAgent;
		var player = controlled.Behaviour.Blackboard["player"].As<Player>();
		navAgent.TargetPosition = player.Position;
		
		var pos = navAgent.GetNextPathPosition();
		var dir = controlled.GlobalPosition.DirectionTo(pos);
		controlled.Velocity = dir * Speed;
		var targetAngle = MathF.Atan2(controlled.Velocity.X,controlled.Velocity.Z);
		var diff = (float)Mathf.Wrap(targetAngle - controlled.MeshNode.Rotation.Y, -Math.PI, Math.PI);
		controlled.CreateTween().TweenProperty(controlled.MeshNode, "rotation", new Vector3(controlled.MeshNode.Rotation.X, controlled.MeshNode.Rotation.Y + diff, controlled.MeshNode.Rotation.Z), .1f);		
		controlled.MoveAndSlide();
	}
}

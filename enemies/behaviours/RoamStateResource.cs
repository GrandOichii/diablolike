using Godot;
using System;

public partial class RoamStateResource : StateResource {
	[Export]
	public float Speed { get; set; } = 2;
	[Export]
	public float RandomPointRadius { get; set; } = 15;
	
	private bool _setPath = false;
//	public Vector3 Target
	public override void Process(EnemyBase controlled, bool isActive, double delta) {
		if (!isActive) return;
		
		// TODO pretty bad, stop and start has to have a EnemyBase argument, can't implement now due to CurrentState being an auto field
		var navAgent = controlled.NavAgent;
		if (!_setPath) {
			var target = new Vector3((float)GD.RandRange(-RandomPointRadius, RandomPointRadius), 0, (float)GD.RandRange(-RandomPointRadius, RandomPointRadius));

			navAgent.TargetPosition = controlled.Position + target;
			_setPath = true;
		}
		if (navAgent.IsNavigationFinished()) {
			controlled.Behaviour.CurrentState = (int)SearchAndChaseBehaviourResource.BStates.Idle;
			return;
		}
		var pos = navAgent.GetNextPathPosition();
		var dir = controlled.GlobalPosition.DirectionTo(pos);
		controlled.Velocity = dir * Speed;
		// TODO turn
		var targetAngle = MathF.Atan2(controlled.Velocity.X,controlled.Velocity.Z);
		GD.Print(controlled.MeshNode);
		var diff = (float)Mathf.Wrap(targetAngle - controlled.MeshNode.Rotation.Y, -Math.PI, Math.PI);
		controlled.CreateTween().TweenProperty(controlled.MeshNode, "rotation", new Vector3(controlled.MeshNode.Rotation.X, controlled.MeshNode.Rotation.Y + diff, controlled.MeshNode.Rotation.Z), .1f);		
		controlled.MoveAndSlide();
	}
	
	public override void Stop() {
		base.Stop();
		
		_setPath = false;
	}
}

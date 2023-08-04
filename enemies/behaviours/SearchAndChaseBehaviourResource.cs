using Godot;
using System;

using Godot.Collections;

public partial class IdleState : State {
	[Export]
	private double WaitFor { get; set; } = 3;
	private double _waitLeft = 1;
	
	public override void Start() {
//		CreateTimer();
		_waitLeft = WaitFor;
	}
	
	public override void Process(EnemyBase controlled, double delta) {
		_waitLeft -= delta;
//		GD.Print("Process " + _waitLeft);
		if (_waitLeft > 0) return;
		
		controlled.Behaviour.CurrentState = (int)SearchAndChaseBehaviourResource.BStates.Roam;
	}
}

public partial class RoamState : State {
	[Export]
	public float Speed { get; set; } = 2;
	[Export]
	public float RandomPointRadius { get; set; } = 15;
	
	private bool _setPath = false;
//	public Vector3 Target
	public override void Process(EnemyBase controlled, double delta) {
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
		var diff = (float)Mathf.Wrap(targetAngle - controlled.MeshNode.Rotation.Y, -Math.PI, Math.PI);
		controlled.CreateTween().TweenProperty(controlled.MeshNode, "rotation", new Vector3(controlled.MeshNode.Rotation.X, controlled.MeshNode.Rotation.Y + diff, controlled.MeshNode.Rotation.Z), .1f);		
		controlled.MoveAndSlide();
	}
	
	public override void Stop() {
		base.Stop();
		
		_setPath = false;
	}
}


public partial class SearchAndChaseBehaviourResource : EnemyBehaviourResource
{
	public enum BStates {
		Idle,
		Roam,
		Chase
	}
	
	[Export]
	public Dictionary<string, string> AnimationMap { get; set; } = new();
	
//	[Export]
	public AnimationTree AnimationsNode { get; set; }
	
	[Export]
	public NodePath AnimationsNodePath { get; set; }


 	override public int CurrentState {
		set {
			base.CurrentState = value;

			foreach (var animKey in AnimationMap.Values)
				AnimationsNode.Set("parameters/conditions/" + animKey, false);
			var key = (BStates)value;
			AnimationsNode.Set("parameters/conditions/" + AnimationMap[key.ToString()], true);
		}
	}

	public override void OnBodyDetected(Node3D body) {
		
		if (body is not Player) return;

		if (!HasValue("player"))
			Blackboard.Add("player", body as Player);
	}
	
	public override void Ready(EnemyBase enemy) {
		base.Ready(enemy);
		AnimationsNode = enemy.GetNode<AnimationTree>(AnimationsNodePath);
		States[(int)BStates.Idle] = new IdleState();
		States[(int)BStates.Roam] = new RoamState();
		
		CurrentState = (int)BStates.Idle;
	}
}

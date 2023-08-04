using Godot;
using System;

using Godot.Collections;

public partial class IdleState : State {
	private double _waitFor = 1;
	
	public override void Start() {
//		CreateTimer();
	}
	
	public override void Process(EnemyBase controlled, double delta) {
		_waitFor -= delta;
//		GD.Print("Process " + _waitFor);
		if (_waitFor > 0) return;
		
		controlled.Behaviour.CurrentState = (int)SearchAndChaseBehaviourResource.BStates.Roam;
	}
}

public partial class RoamState : State {
	
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


 	override public int CurrentState {
		set {
			base.CurrentState = value;

			foreach (var animKey in AnimationMap.Values)
				AnimationsNode.Set("parameters/conditions/" + animKey, false);
			var key = (BStates)value;
			AnimationsNode.Set("parameters/conditions/" + AnimationMap[key.ToString()], true);
			GD.Print("parameters/conditions/" + AnimationMap[key.ToString()]);
		}
	}

	public override void OnBodyDetected(Node3D body) {
		
		if (body is not Player) return;

		if (!HasValue("player"))
			Blackboard.Add("player", body as Player);
	}
	
	public override void Ready() {
		base.Ready();
		States[(int)BStates.Idle] = new IdleState();
		States[(int)BStates.Roam] = new RoamState();
		
		CurrentState = (int)BStates.Idle;
	}
}

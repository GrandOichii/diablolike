using Godot;
using System;

using Godot.Collections;


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
	
	[Export]
	public IdleStateResource IdleState { get; set; }
	
	[Export]
	public RoamStateResource RoamState { get; set; }


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
		States[(int)BStates.Idle] = IdleState;
		States[(int)BStates.Roam] = RoamState;
		
		CurrentState = (int)BStates.Idle;
	}
}

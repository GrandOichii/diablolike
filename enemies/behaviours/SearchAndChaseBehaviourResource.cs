using Godot;
using System;

using Godot.Collections;

//public partial class IdleState : State {
//
//}


public partial class SearchAndChaseBehaviourResource : EnemyBehaviourResource
{
	public enum BStates {
		Idle,
		Roam,
		Chase
	}
	
//	[Export]
//	public Dictionary<string, string> AnimationMap { get; set; } = new();
	
//	[Export]
//	public NodePath AnimationsNode { get; set; }
//
//	private AnimationTree AnimationTreeNode { get; set; }

// 	override public int CurrentState {
//		set {
//			base.CurrentState = value;
////			foreach (var animKey in AnimationMap.Values)
////				AnimationsNode.Set("parameters/conditions/" + animKey, false);
////			AnimationsNode.Set("parameters/conditions/" + AnimationMap[(int)value], true);
//		}
//	}

//
//	public SearchAndChaseBehaviourResource() : base() {
//		CurrentState = (int)BStates.Idle;
//
//		States[(int)BStates.Idle] = new IdleState();
//
////		GD.Print(AnimationsNode);
//	}

	public override void OnBodyDetected(Node3D body) {
		if (body is not Player) return;

//		Blackboard.Add("player", body as Player);
	}
}

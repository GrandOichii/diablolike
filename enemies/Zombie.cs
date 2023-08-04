using Godot;
using System;

//[Tool]
public partial class Zombie : EnemyBase
{
//	protected override void OnDetectionAreaBodyEntered(Node3D body)
//	{
//		base.OnDetectionAreaBodyEntered(body);
//		if (body is not Player) return;
//	}

	public override void _Ready() {
		// TODO VERY BAD
		var animTree = GetNode<AnimationTree>("%AnimationTree");
		(Behaviour as SearchAndChaseBehaviourResource).AnimationsNode = animTree;
		
		base._Ready();
	}
}

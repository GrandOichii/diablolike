using Godot;
using System;


public partial class IdleStateResource : StateResource {
	[Export]
	private double WaitFor { get; set; } = 3;

	private double _waitLeft = 1;
	
	public override void Start() {
		_waitLeft = WaitFor;
	}
	
	public override void Process(EnemyBase controlled, bool isActive, double delta) {
		if (!isActive) return;

		_waitLeft -= delta;
		if (_waitLeft > 0) return;
		
		controlled.Behaviour.CurrentState = (int)SearchAndChaseBehaviourResource.BStates.Roam;
	}
}

using Godot;
using System;


public partial class IdleStateResource : StateResource {
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

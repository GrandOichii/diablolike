using Godot;
using System;

using Godot.Collections;

public partial class State : Node {
	public virtual void Start() {}
	public virtual void Process(EnemyBase controlled, double delta) {}
	public virtual void Stop() {}
}

public partial class EnemyBehaviourResource : Resource
{
	private int _currentState = 0;
	virtual public int CurrentState {
		get => _currentState;
		set {
			States[_currentState].Stop();
			_currentState = value;
			States[_currentState].Start();
		}
	}
	public Dictionary<int, State> States { get; set; } = new();
	public Dictionary<string, Variant> Blackboard = new();
//
	public void Process(EnemyBase controlled, double delta) {
		States[CurrentState].Process(controlled, delta);
	}
//
	public virtual void OnBodyDetected(Node3D body) {
	}
//
	public bool HasValue(string key) => Blackboard.ContainsKey(key);
	
	public virtual void Ready(EnemyBase enemy) {
		
	}
}

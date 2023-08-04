using Godot;
using System;

using Godot.Collections;


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
	public Dictionary<int, StateResource> States { get; set; } = new();
	public Dictionary<string, Variant> Blackboard = new();
//
	public void Process(EnemyBase controlled, double delta) {
		foreach (var pair in States)
			pair.Value.Process(controlled, pair.Key == CurrentState, delta);
	}
//
	public virtual void OnBodyDetected(Node3D body) {
	}
//
	public bool HasValue(string key) => Blackboard.ContainsKey(key);
	
	public virtual void Ready(EnemyBase enemy) {
		
	}
}

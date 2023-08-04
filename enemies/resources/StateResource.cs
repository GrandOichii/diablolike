using System;
using Godot;

public partial class StateResource : Resource {
	public virtual void Start() {}
	public virtual void Process(EnemyBase controlled, double delta) {}
	public virtual void Stop() {}
}

using System;
using Godot;

public partial class StateResource : Resource {
	public virtual void Start() {}
	public virtual void Process(EnemyBase controlled, bool isActive, double delta) {}
	public virtual void Stop() {}
}

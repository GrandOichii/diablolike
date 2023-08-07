using Godot;
using System;

public partial class DamageResource : Resource
{
	[Export]
	public float MinDamage { get; set; }
	
	[Export]
	public float MaxDamage { get; set; }
}

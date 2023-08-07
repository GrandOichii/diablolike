using Godot;
using System;

public partial class WeaponResource : EquipableItemResource
{
	[Export]
	public DamageResource OnHitDamage { get; set; }
	
	[Export]
	public PackedScene WeaponScene { get; set; }
}

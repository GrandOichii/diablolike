using Godot;
using System;

using Godot.Collections;

public enum EquipSlot {
	Head,
	Body,
	Legs,
	Ring,
	Weapon,
}

public partial class EquipableItemResource : ItemResource
{
	[Export]
	public EquipSlot Slot { get; set; }
	
	[Export]
	public Dictionary<string, int> PropertyMod { get; set; }  = new();
	
	// TODO figure out how to actually export PlayerProperty, currently doesn't work
	public virtual void OnEquip(Player player) {
		foreach (var pair in PropertyMod) {
			var propName = Enum.Parse<PlayerProperty>(pair.Key);
			// GD.Print(propName);
			var p = player.Properties[propName];
			p.Value += pair.Value;
		}
	}
	
	public virtual void OnUneqiup(Player player) {
		foreach (var pair in PropertyMod) {
			var propName = Enum.Parse<PlayerProperty>(pair.Key);
			var p = player.Properties[propName];
			p.Value -= pair.Value;
		}
	}

	// [Export]
	// public Dictionar
}

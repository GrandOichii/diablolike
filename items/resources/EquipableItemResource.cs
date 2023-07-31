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

	// [Export]
	// public Dictionar
}

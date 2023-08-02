using Godot;
using System;

public partial class EquipmentSlot : TextureRect
{
	[Export]
	public EquipSlot Slot { get; set; }
	
	public Texture2D Placeholder { get; set; }
	
	public override void _Ready() {
		Placeholder = Texture;
	}

	private void OnPlayerItemEquip(EquipableItemResource item, EquipSlot slot, CharacterBody3D player)
	{
		if (slot != Slot) return;
		
		Texture = item.InventoryIcon;
		// Replace with function body.
	}
	
	private void OnPlayerItemUnequip(Resource item, EquipSlot slot, CharacterBody3D player)
	{
		if (slot != Slot) return;
		
		Texture = Placeholder;
		// Replace with function body.
	}
}




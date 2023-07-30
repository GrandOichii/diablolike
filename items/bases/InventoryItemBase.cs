using Godot;
using System;

public partial class InventoryItemBase : ItemBase
{
	[Export]
	public string InventoryName { get; set; }
	[Export]
	public Texture2D InventoryIcon { get; set; }
	
	public override void OnPickUp(Player player) {
		// TODO make another child of ItemBase: Non-pickupable, will destroy itself on pickup
		player.AddToInventory(this);
		base.OnPickUp(player);
//		player.FocusedItems.Remove(this);

	}

}

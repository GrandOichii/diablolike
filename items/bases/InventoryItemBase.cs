using Godot;
using System;
using System.Linq;

using System.Collections.Generic;

public partial class InventoryItemBase : ItemBase
{
	[Export]
	public ItemResource Item { get; set; }
	
	
	public override void OnPickUp(Player player) {
		player.AddToInventory(this.Item);
		base.OnPickUp(player);
	}
	

}

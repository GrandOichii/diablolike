using Godot;
using System;

public partial class RestoreManaAction : RemoveFromInventoryItemAction
{
	[Export]
	public int Amount { get; set; }
	
	public override void Do(ItemResource item, Player player, int inventoryIdx) {
		player.Mana += Amount;

		base.Do(item, player, inventoryIdx);
	}
}

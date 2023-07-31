using Godot;
using System;

public partial class HealAction : RemoveFromInventoryItemAction
{
	[Export]
	public int Amount { get; set; }
	
	public override void Do(ItemResource item, Player player, int inventoryIdx) {
		player.Health += Amount;
		
		base.Do(item, player, inventoryIdx);
	}
}

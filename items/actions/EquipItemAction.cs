using Godot;
using System;

public partial class EquipItemAction : RemoveFromInventoryItemAction
{
	public override void Do(ItemResource item, Player player, int inventoryIdx) {
		EquipableItemResource eItem;
		switch (item) {
			case EquipableItemResource ei:
				eItem = ei;
				break;
			default:
				GD.Print("Warn: Tried to equip non-equipable item");
				return;
		}
		// GD.Print("Equip " + item.InventoryName + " to " + eItem.Slot);
		player[eItem.Slot] = eItem;
		
		base.Do(item, player, inventoryIdx);
//		eItem.OnEquip(player);
	}
}

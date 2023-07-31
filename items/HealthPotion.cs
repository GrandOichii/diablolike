using Godot;
using System;

//public partial class HealItemAction : ItemAction {
//
//	public override void Do(InventoryItemBase item, Player player) {
//		GD.Print("HELLO");
//	}
//}

public partial class HealthPotion : InventoryItemBase
{
	public override string ItemName => "Health Potion";
	
	public override void _Ready() {
		base._Ready();
		
//		Item.Actions.Add("Drink", new HealItemAction());
	}
}

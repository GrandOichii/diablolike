using Godot;
using System;

public partial class GoldPouch : ItemBase
{
	[Export]
	public int Amount { get; set; }
	
	public override string ItemName { get => "Gold Pouch (" + Amount + ")"; }

	public override void OnPickUp(Player player) {
		base.OnPickUp(player);
		
		player.Gold += Amount;
	}
}

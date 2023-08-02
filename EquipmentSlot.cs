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
}

using System;
using Godot;
using System.Linq;

using Godot.Collections;
//using System.Collections.Generic;

public partial class ItemResource : Resource {
	[Export]
	public string InventoryName { get; set; }
	[Export]
	public Texture2D InventoryIcon { get; set; }
	[Export(PropertyHint.MultilineText)]
	public string Description { get; set; } 
	[Export]
	public Dictionary<string, ItemAction> Actions { get; set; } = new();
//	public List<string> Keys => Actions.Keys.ToList();
}

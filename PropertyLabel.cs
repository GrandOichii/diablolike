using Godot;
using System;

public partial class PropertyLabel : Label
{
	[Export]
	public PlayerProperty Property { get; set; }
	
	private void OnPlayerPropertyChanged(PlayerProperty property, int v)
	{
		if (property != Property) return;
		
		Text = "" + v;
	}
}

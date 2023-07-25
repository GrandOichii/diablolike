using Godot;
using System;

public partial class DebugLabelContainer : VBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnPlayerUpdateLabel(int id, string text)
	{
		var label = GetChild<Label>(id);
		label.Text = text;
	}
}

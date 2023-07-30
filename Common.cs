using Godot;
using System;

//public partial class Common : Node
//{
//	// Called when the node enters the scene tree for the first time.
//	public override void _Ready()
//	{
//	}
//
//	// Called every frame. 'delta' is the elapsed time since the previous frame.
//	public override void _Process(double delta)
//	{
//	}
//}

public interface IItem {
	public void OnPickUp(Player player);
	public void OnEnterFocus(Player player);
	public void OnLeaveFocus(Player player);
	public void SetViewed(bool v);
}

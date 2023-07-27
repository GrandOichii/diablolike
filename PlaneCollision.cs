using Godot;
using System;

public partial class PlaneCollision : CollisionShape3D
{
//	public override void _Input(InputEvent e) {
//		InputEventMouseButton mouseEvent;
//		switch (e) {
//			case InputEventMouseButton me:
//				mouseEvent = me;
//				break;
//			default:
//				return;
//		}
//		if (mouseEvent.IsPressed() && mouseEvent.ButtonIndex == MouseButton.Left) {
//			GD.Print(mouseEvent.Position);
//
//		}
//	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

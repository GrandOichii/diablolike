using Godot;
using System;

public partial class ItemBase : StaticBody3D, IItem
{

	public virtual string ItemName { get; } = "<NO_ITEM_NAME>";
	
	private Label3D NameLabel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		NameLabel = GetNode<Label3D>("%NameLabel");
		NameLabel.Text = ItemName;
		
		NameLabel.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
	public void OnEnterFocus(Player player){
		NameLabel.Visible = true;
	}
	
	public void OnLeaveFocus(Player player){
		NameLabel.Visible = false;
	}
	
	public void SetViewed(bool v) {
		var t = ItemName;
		if (v) t = "[E] " + t;
		NameLabel.Text = t;
	}
	
	public virtual void OnPickUp(Player player) {
		player.FocusedItems.Remove(this);
		this.Free();
	}
}

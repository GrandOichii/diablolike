using Godot;
using System;

public partial class PropertyBar : TextureProgressBar
{
	[Export]
	public PlayerProperty Property { get; set; }
	[Export]
	public PlayerProperty MaxProperty { get; set; }
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	

	private void OnPlayerPropertyChanged(PlayerProperty property, long v)
	{
		if (property == Property) {
//				create_tween().tween_property(self, 'value', mana, .2).set_trans(Tween.TRANS_CIRC).set_ease(Tween.EASE_OUT)
			CreateTween().TweenProperty(this, "value", v, .2f).SetTrans(Tween.TransitionType.Circ).SetEase(Tween.EaseType.Out);
		}
		if (property == MaxProperty) {
			MaxValue = v;
		}
	}

}


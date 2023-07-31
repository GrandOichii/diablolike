extends TextureProgressBar

func _on_player_mana_changed(mana):
	create_tween().tween_property(self, 'value', mana, .2).set_trans(Tween.TRANS_CIRC).set_ease(Tween.EASE_OUT)

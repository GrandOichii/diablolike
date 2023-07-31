extends TextureProgressBar

func _on_player_health_changed(health):
	create_tween().tween_property(self, 'value', health, .2).set_trans(Tween.TRANS_CIRC).set_ease(Tween.EASE_OUT)

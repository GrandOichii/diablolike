extends TextureProgressBar

func _on_player_health_changed(health):
	create_tween().tween_property(self, 'value', health, .2).set_trans(Tween.TRANS_CIRC).set_ease(Tween.EASE_OUT)


func _on_player_max_health_changed(maxHealth):
	print('NEW: ' + str(maxHealth))
	max_value = maxHealth

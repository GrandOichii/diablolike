extends StaticBody3D


func _on_player_clicked_move_to(pos):
	position.x = pos.x
	position.z = pos.z
	
	visible = true

extends StaticBody3D


func _on_player_clicked_move_to(position):
	self.position.x = position.x
	self.position.z = position.z
	
	visible = true

extends MarginContainer

@onready var item_list_node: ItemList = %ItemList

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	pass

func _on_player_toggle_open_inventory(_player, open):
	visible = true
	var target = 0
	if open:
		target = DisplayServer.window_get_size().y
	create_tween() \
		.tween_property(self, 'position', Vector2(position.x, target), .2) \
		.set_trans(Tween.TRANS_CIRC) \
		.finished.connect(func(): visible = not open)


func _on_player_add_item_to_inventory(_player, item):
	item_list_node.add_item(item.InventoryName, item.InventoryIcon)

extends MarginContainer

class_name InventoryPanel

@onready var item_list_node: ItemList = %ItemList
@onready var item_name_label: Label = %ItemNameLabel
@onready var item_description_text: RichTextLabel = %ItemDescriptionLabel
@onready var item_action_container: VBoxContainer = %ItemActionContainer
@onready var player_node := %Player

func _ready():
	visible = false
	
	item_list_node.clear()
	remove_item_info()

func _process(_delta):
	pass

func _on_player_toggle_open_inventory(_player, open):
	visible = true
	var target = 0
	if open:
		target = DisplayServer.window_get_size().y
		remove_item_info()
	create_tween() \
		.tween_property(self, 'position', Vector2(position.x, target), .2) \
		.set_trans(Tween.TRANS_CIRC) \
		.finished.connect(func(): on_finish_inventory_toggle(open))
		
func on_finish_inventory_toggle(open):
	visible = not open
	
	if not open:
		remove_item_info()
		
func remove_item_info():
	item_name_label.text = ''
	item_description_text.text = ''
	clear_item_actions()
		
func clear_item_actions():
	for n in item_action_container.get_children():
		n.queue_free()

func _on_player_add_item_to_inventory(_player, item):
	var i = item_list_node.add_item(item.Item.InventoryName, item.Item.InventoryIcon)
	item_list_node.set_item_metadata(i, item.Item)

func _on_item_list_item_selected(index):
	var item = item_list_node.get_item_metadata(index)
	item_name_label.text = item.InventoryName
#	item_description_text.
	item_description_text.text = item.Description
#	item_description_text.set_text(item.Description)
#	print('set text to ' + item.Description)
	
	var action_dict = item.Actions
#	var action_keys = item.Actions.keys()
	for key in action_dict:
		var b = Button.new()
		b.text = key
		b.pressed.connect(func(): action_dict[key].Do(item, player_node, index))
		item_action_container.add_child(b)

#	for action_key in action_keys:
#	print(action_keys)

func _on_player_remove_item_from_inventory(item, idx):
	if item_list_node.is_selected(idx):
		item_list_node.deselect_all()
		remove_item_info()
	item_list_node.remove_item(idx)
	

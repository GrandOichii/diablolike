extends Node3D

@export var resource: Resource
@export var node_path: Node

func _ready():
	print(resource.node_export)
	print(node_path)
	print($ChildNode)



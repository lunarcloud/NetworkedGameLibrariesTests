extends Node

func _on_MagratheaButton_pressed():
	$"/root/GlobalData".server = "204.48.22.8"
	# warning-ignore:return_value_discarded
	get_tree().change_scene("res://ChatScreen.tscn")


func _on_SearchButton_pressed():
	# warning-ignore:return_value_discarded
	get_tree().change_scene("res://ServerDiscoveryScreen.tscn")

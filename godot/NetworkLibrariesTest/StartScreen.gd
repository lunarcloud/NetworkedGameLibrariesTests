extends Control

func _on_ChatButton_pressed():
	# warning-ignore:return_value_discarded
	get_tree().change_scene("res://ConnectScreen.tscn")


func _on_HostButton_pressed():
	# warning-ignore:return_value_discarded
	get_tree().change_scene("res://ServerHostingScreen.tscn")

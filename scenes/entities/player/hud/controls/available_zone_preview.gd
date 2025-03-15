extends Node2D
func _draw() -> void:
	var parent = get_parent()
	
	if parent != null:
		
		var availableZone = parent.AvailableZone
		var posY = get_viewport_rect().size.y - availableZone.y
		var rect = Rect2(Vector2(0, posY), Vector2(availableZone.x, availableZone.y))
		draw_rect(rect, Color(Color.CORNFLOWER_BLUE, 0.2))

extends KinematicBody2D

var motion = Vector2()
var gravity = 15.0
var jumpIndex = 0
const UP = Vector2(0, -1)

func _physics_process(delta):
	
	motion.y += gravity
	
	if Input.is_action_pressed("ui_right"):
		motion.x = min(motion.x + 30, 300)
	elif Input.is_action_pressed("ui_left"):
		motion.x = max(motion.x - 30, -300)
	else:
		motion.x = lerp(motion.x, 0, 0.2)
					
	if is_on_floor():
		motion.y = 0;
		jumpIndex = 0;
		
	if jumpIndex < 2:
		if Input.is_action_just_pressed("ui_up"):
			jumpIndex += 1;
			motion.y = -400;
			
	elif Input.is_action_pressed("ui_down"):
		
		if motion.y < 0:
			motion.y = motion.y + 35;
		if motion.y >= 0:
			 motion.y = motion.y + 10;
		    
		
	move_and_slide(motion, UP)	
	
	pass
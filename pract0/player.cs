using Godot;
using System;

public partial class player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	
	public int jump_max = 2;
	public int jump_count = 1;

	public override void _Input(InputEvent @event)
	{
		Vector2 velocity = Velocity;
		// Handle Jump
		if (jump_count != jump_max)
		{
			jump_count += 1;
			if (@event.IsActionPressed("jump"))
			{
				velocity.Y = JumpVelocity;
				Velocity = velocity;
			}
		}
		
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Keeps player on floor
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;
		
		// Jump resetter
		if ((IsOnFloor()))
			jump_count = 0;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			Velocity = velocity;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			Velocity = velocity;
		}	
		MoveAndSlide();
	}
}

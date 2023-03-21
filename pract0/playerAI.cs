using Godot;
using System;

public partial class playerAI : CharacterBody2D
{
	public const float Speed = 75.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	
	// Variables for deciding movement
	private Vector2 startPosition;
	private float distanceThreshold = 75.0f;
	public int randVal = 0;
	private int ctrRight = 0;
	private int ctrLeft = 0;
	
	public RayCast2D raycast;
	
	public override void _Ready()
	{
		raycast = GetNode<RayCast2D>("RayCast2D");
		startPosition = GlobalPosition;
		randVal = RandomMove();
		// unused
		var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		collisionShape.SetDeferred("disabled", false);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		
		if (raycast.IsColliding())
		{
			GD.Print("HIT");
		}
		
		PutGravity(delta);
			
		// Check for collision with walls and change direction // pending
		
		MoveResetter();
		
		RandomLimit();

		MoveRightLeft(delta, randVal);

		MoveAndSlide();
	}
	
	public void PutGravity(double delta)
	{
		Vector2 velocity = Velocity;
		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;
		Velocity = velocity; 
	}
	
	public void MoveResetter()
	{
		// Calculates how far is the distance between the character's entry point 
		// and character's current postion
		float distance = GlobalPosition.DistanceTo(startPosition);
		
		// The primary driving point for changing direction
		if (distance > distanceThreshold)
		{
			startPosition = GlobalPosition; // Resets the relative distance
			randVal = RandomMove();
			
			if (randVal == 1)
				ctrRight += 1;
			else if (randVal == 2)
				ctrLeft += 1;
		}
	}

	public int RandomMove()
	{
		int randVal;
		var random = new RandomNumberGenerator();
		random.Randomize();
		randVal = random.RandiRange(1, 2);
		return randVal;
	}
	
	public void RandomLimit()
	{
		// Limits the randomness of the enemy movement
		// Also the secondary driving point for changing direction
		if (ctrRight == 2)
		{
			ctrRight = 0;
			randVal = 2;
		}
		if (ctrLeft == 2)
		{
			ctrLeft = 0;
			randVal = 1;
		}
	}
	
	public void MoveRightLeft(double delta, int direction)
	{
		Vector2 velocity = Velocity;

		if (!IsOnFloor())
			PutGravity(delta);
		
		// Selects the direction based on a random num gen
		switch (direction)
		{
			case 1:
				velocity.X = 1 * Speed;
				break;
			case 2:
				velocity.X = -1 * Speed;
				break;
		}
		Velocity = velocity;
	}
}

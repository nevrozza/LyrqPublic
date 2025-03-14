using Godot;
using Lyrq.utils.cs;

namespace Lyrq.scenes.entities.player.controllers;

public class DashController(
	Player player
	)
{
	public bool IsDashing = false;
	public double DashTimer = 0;

	public void Input(InputEvent @event)
	{
		if (@event.IsActionPressed(InputBindings.Movement.Dash) && IsDashAvailable())
		{
			StartDash();
		}
	}
	
	// [Input]
	public void StartDash()
	{
		
		Vector3 direction = GetDashDirection();
		
		player.Velocity += direction * player.DashRange;
		player.MoveAndSlide();
		
		IsDashing = true;
		DashTimer = 0.0;
		GD.Print("DASH!");
	}

	// [PhysicsProcess]
	public void UpdateDashTimer(double delta)
	{
		if (IsDashing)
		{
			DashTimer += delta;
			GD.Print(DashTimer);
			if (DashTimer >= player.DashDelay)
			{
				IsDashing = false;
				GD.Print("Dash completed.");
			}
		}
	}

	private Vector3 GetDashDirection()
	{
		float yaw = player.Rotation.Y;

		Vector3 direction = (player.Velocity == Vector3.Zero) ? new Vector3(
			Mathf.Sin(yaw),
			0,
			Mathf.Cos(yaw)
		): player.Velocity;
		
		return new Vector3(
			direction.X,
			0,
			direction.Z
		).Normalized();
	}

	public bool IsDashAvailable()
	{
		return !IsDashing;
	}
}

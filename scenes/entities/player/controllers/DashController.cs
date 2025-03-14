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

	/* Эта шняга возвращает чисто направление (Y всегда 0), без скорости.
	 Скорость была | Оно вернёт
			2      |     1
		   -5      |    -1
			0      |     0
	 */
	private Vector3 GetDashDirection()
	{
		Vector3 direction = player.Velocity.Normalized();

		return new Vector3(
			Mathf.Sign(direction.X),
			0,
			Mathf.Sign(direction.Z)
		);
	}

	public bool IsDashAvailable()
	{
		return player.Velocity != Vector3.Zero && !IsDashing;
	}
}

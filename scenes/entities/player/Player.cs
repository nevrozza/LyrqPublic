using Godot;
using Lyrq.scenes.entities.player.controllers;
using Lyrq.utils.cs;

namespace Lyrq.scenes.entities.player;

public partial class Player : CharacterBody3D
{
	[Export] public float MovementSpeed = 6.5f;

	[Export] public float DashRange = 100.0f;
	[Export] public float DashDelay = 2.5f;

	public DashController DashController;
	public MovementPhysicsController MovementPhysicsController;

	public override void _Ready()
	{
		DashController = new DashController(player: this);
		MovementPhysicsController = new MovementPhysicsController(player: this);
	}

	public override void _Input(InputEvent @event)
	{
		DashController.Input(@event);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		MovementPhysicsController.MovementProcess(delta);
		DashController.UpdateDashTimer(delta);
	}
}

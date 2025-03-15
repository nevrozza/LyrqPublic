using Godot;
using Lyrq.utils.cs;

namespace Lyrq.scenes.entities.player.controllers;

public partial class MovementPhysicsController(
    Player player
) : Node // Нужна Node для Input.GetVector
{
    private Vector2 _movementInput = Vector2.Zero;

    // [PhysicsProcess]
    public void MovementProcess(double delta)
    {
        // эти 2 шняги редачат velocity player'а
        MovementInput();
        FallingLogic(delta);

        player.MoveAndSlide();
    }

    // Логика падения персонажа [PhysicsProcess]
    private void FallingLogic(double delta)
    {
        if (!player.IsOnFloor() && !player.DashController.IsDashing)
        {
            player.Velocity += player.GetGravity() * (float)delta;
        }
        else
        {
            player.Velocity = player.Velocity with
            {
                Y = 0
            };
        }
    }

    // Обработка нажатий *WASD* [PhysicsProcess]
    private void MovementInput()
    {
        _movementInput = Input.GetVector(
            negativeX: InputBindings.Movement.Right,
            positiveX: InputBindings.Movement.Left,
            negativeY: InputBindings.Movement.Backward,
            positiveY: InputBindings.Movement.Forward
        );
        // Так и должно быть, что z [3D] = y [2D]
        player.Velocity = player.Velocity with
        {
            X = _movementInput.X * player.MovementSpeed,
            Z = _movementInput.Y * player.MovementSpeed
        };
    }
}
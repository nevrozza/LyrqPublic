using Godot;
using Lyrq.utils.cs;

namespace Lyrq.scenes.entities.player.smoothCamera;

public partial class SmoothCamera : Camera3D
{
	[Export] private float _positionSpeed = 1f;
	[Export] private float _rotationSpeed = 1f;
	[Export] private Node3D _target;
	[Export] public Vector3 Offset;
	
	public void SetTarget(Node3D newTarget)
	{
		_target = newTarget;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (_target != null)
		{
			Vector3 targetPosition = _target.Transform.Origin + Offset;
			Transform = Transform with
			{
				Origin = Transform.Origin.Lerp(targetPosition, (float)(delta*_positionSpeed))
			};
			SmoothLookAt(_target.Transform.Origin, (float)(delta*_rotationSpeed));
		}   
	}
	
	private void SmoothLookAt(Vector3 targetPosition, float weight)
	{
		Vector3 direction = (targetPosition - Transform.Origin).Normalized();
		Basis targetBasis = Basis.LookingAt(direction);
		Transform = Transform with
		{
			Basis = Transform.Basis.Slerp(targetBasis, weight)
		};
	}
}

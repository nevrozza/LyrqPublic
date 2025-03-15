using Godot;
using Godot.NativeInterop;
using Lyrq.utils.cs;

namespace Lyrq.scenes.entities.player.hud.controls.joystick;

public partial class TouchJoystick : Node2D
{
	private TouchScreenButton _button;
	private Sprite2D _handle;

	[Export] private float _bigCircleScale = 1f;
	[Export] private bool _isStatic = false;
	[Export] public Vector2 AvailableZone = new Vector2(x: 1000f, y: 500f);

	private float _maxDragOffset;
	private Vector2 _joystickCenterPos = Vector2.Zero;

	private int _touchIndex = -1;


	public override void _Ready()
	{
		_button = GetNode<TouchScreenButton>("Stick/TouchScreenButton");
		_handle = GetNode<Sprite2D>("Stick/Handle");
		_button.Scale = new Vector2(_bigCircleScale, _bigCircleScale);
		if (_button.Shape is CircleShape2D shape)
		{
			_maxDragOffset = shape.Radius * _bigCircleScale;
			_joystickCenterPos = GlobalPosition;

			BigCircleSetCenteredPos();
			DeactivateJoystick();
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventScreenTouch touch)
		{
			// Сетапим НАЧАЛО движения (индекс пальца, позиция стика)  // _touchIndex == -1 - stick is inactive
			if (touch.IsPressed() && IsInAvailableZone(touch.Position) && _touchIndex == -1)
			{
				Vector2 touchPos = touch.Position;
				ActivateJoystick(touch.Index);
				if (!_isStatic)
				{
					_joystickCenterPos = touchPos;

					HandleSetCenteredPos();
					BigCircleSetCenteredPos();
				}
				else
				{
					CalculateAndMove(touchPos);
				}
			}
			// Отпустили стик (тем же пальцем)
			else if (touch.IsReleased() && _touchIndex == touch.Index)
			{
				DeactivateJoystick();
			}
		}

		// Рассчитываем типа мы стик двигаем
		if (@event is InputEventScreenDrag drag && _touchIndex == drag.Index)
		{
			CalculateAndMove(drag.Position);
		}
	}

	private void BigCircleSetCenteredPos() => _button.GlobalPosition = _joystickCenterPos.Plus(-_maxDragOffset);
	private void HandleSetCenteredPos() => _handle.GlobalPosition = _joystickCenterPos;

	private bool IsInAvailableZone(Vector2 touchPos)
	{
		float minY = GetViewport().GetVisibleRect().Size.Y - AvailableZone.Y;
		return touchPos.Y > minY && touchPos.X < AvailableZone.X;
	}

	private void CalculateAndMove(Vector2 touchPos)
	{
		Vector2 offset = ((touchPos - _joystickCenterPos) / _maxDragOffset).LimitLength();
		MoveHandle(offset);
	}


	private void DeactivateJoystick()
	{
		DeactivateHandle();

		_touchIndex = -1;
		if (!_isStatic)
		{
			_button.Visible = false;
			_handle.Visible = false;
		}
	}

	private void ActivateJoystick(int touchIndex)
	{
		ActivateHandle();
		_touchIndex = touchIndex;
		_button.Visible = true;
		_handle.Visible = true;
	}

	private void MoveHandle(Vector2 offset)
	{
		
		Input.ActionPress(InputBindings.Movement.Forward, (-offset.Y).CoerceAtLeast(0));
		Input.ActionPress(InputBindings.Movement.Backward, offset.Y.CoerceAtLeast(0));
		Input.ActionPress(InputBindings.Movement.Left, (-offset.X).CoerceAtLeast(0));
		Input.ActionPress(InputBindings.Movement.Right, offset.X.CoerceAtLeast(0));
		
		
		_handle.GlobalPosition = _joystickCenterPos + offset * _maxDragOffset;
	}

	private void ActivateHandle()
	{
		Texture2D texture =
			ResourceLoader.Load<CompressedTexture2D>("res://graphics/ui/hud/controls/joystick/handleFilled.png");
		_handle.Texture = texture;
	}

	private void DeactivateHandle()
	{
		MoveHandle(Vector2.Zero);
		Texture2D texture =
			ResourceLoader.Load<CompressedTexture2D>("res://graphics/ui/hud/controls/joystick/handle.png");
		_handle.Texture = texture;
	}
}

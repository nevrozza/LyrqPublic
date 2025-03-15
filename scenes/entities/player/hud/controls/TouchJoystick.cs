using Godot;
using Lyrq.utils.cs;

namespace Lyrq.scenes.entities.player.hud.controls;

public partial class TouchJoystick : CanvasLayer
{
	private TouchScreenButton _button;
	private Sprite2D _handle;

	[Export] private bool _isStatic = false;
	[Export] public Vector2 AvailableZone = new Vector2(x: 1000f, y: 500f);

	private float _maxDragOffset;
	private Vector2 _joystickCenterPos = Vector2.Zero;
	
	private bool _isJoystickActive = true;


	public override void _Ready()
	{
		_button = GetNode<TouchScreenButton>("Stick/TouchScreenButton");
		_handle = GetNode<Sprite2D>("Stick/Handle");

		if (_button.Shape is CircleShape2D shape)
		{
			_maxDragOffset = shape.Radius * _button.Scale.X;
			// Если он статичный, то центр никогда не двигается! находится здесь, т.к. в Ready багуется..
			if (_isStatic)
			{
				_joystickCenterPos = _button.Position.Plus(_maxDragOffset);
				DeactivateHandle();
			}
		}
	}
	
	public override void _Input(InputEvent @event)
	{
		// Сетапим НАЧАЛО движения (индекс пальца, позиция стика) 
		if (@event is InputEventScreenTouch touch)
		{
			if (touch.IsPressed() && IsInAvailableZone(touch.Position))
			{
				Vector2 touchPos = touch.Position;
				ActivateJoystick();
				if (!_isStatic)
				{
					_handle.Position = touchPos;
					_joystickCenterPos = touchPos;
					_button.Position = _joystickCenterPos.Plus(-_maxDragOffset);
				}
				else
				{
					CalculateAndMove(touchPos);
				}
			}
			else
			{
				DeactivateJoystick();
			}
		}

		// Рассчитываем типа мы стик двигаем
		if (@event is InputEventScreenDrag drag && _isJoystickActive)
		{
			CalculateAndMove(drag.Position);
		}
	}

	private bool IsInAvailableZone(Vector2 touchPos)
	{
		float minY = GetViewport().GetVisibleRect().Size.Y - AvailableZone.Y;
		return touchPos.Y > minY && touchPos.X < AvailableZone.X;
	}

	private void CalculateAndMove(Vector2 handlePos)
	{
		Vector2 offset = ((handlePos - _joystickCenterPos) / _maxDragOffset).LimitLength();
		MoveHandle(offset);
		// TODO: Movement logic
	}


	private void DeactivateJoystick()
	{
		DeactivateHandle();
		
		_isJoystickActive = false;
		if (!_isStatic)
		{
			_button.Visible = false;
			_handle.Visible = false;
		}
	}

	private void ActivateJoystick()
	{
		ActivateHandle();
		_isJoystickActive = true;
		_button.Visible = true;
		_handle.Visible = true;
	}

	private void MoveHandle(Vector2 offset)
	{
		_handle.Position = _joystickCenterPos + offset * _maxDragOffset;
	}

	private void ActivateHandle()
	{
		Texture2D texture =
			ResourceLoader.Load<CompressedTexture2D>("res://graphics/ui/hud/controls/joystick/handleFilled.png");
		_handle.Texture = texture;
	}

	private void DeactivateHandle()
	{
		_handle.Position = _joystickCenterPos;
		Texture2D texture =
			ResourceLoader.Load<CompressedTexture2D>("res://graphics/ui/hud/controls/joystick/handle.png");
		_handle.Texture = texture;
	}
}

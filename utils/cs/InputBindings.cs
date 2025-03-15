namespace Lyrq.utils.cs;

public abstract record InputBindings
{
	public abstract record Movement
	{
		public const string Forward =  "move_forward";
		public const string Backward = "move_backward";
		public const string Left = "move_left";
		public const string Right = "move_right";
		public const string Dash = "dash";
	}

	public abstract record Test
	{
		public const string Esc = "ui_cancel";
	}
}

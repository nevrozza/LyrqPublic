namespace Lyrq.utils.cs;

public abstract record InputBindings
{
	public abstract record Movement
	{
		public const string Forward =  "ui_up" /*"move_forward"*/;
		public const string Backward = "ui_down"/*"move_backward"*/;
		public const string Left = "ui_left" /*"move_left"*/;
		public const string Right = "ui_right" /*"move_right"*/;
		public const string Dash = "ui_select" /*"dash"*/;
	}

	public abstract record Test
	{
		public const string Esc = "ui_cancel";
	}
}

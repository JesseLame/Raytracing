namespace Template
{
	class MyApplication
	{
		// member variables
		public Surface screen;
		// initialize
		public void Init()
		{
			
		}
		// tick: renders one frame
		public void Tick()
		{
			screen.Clear( 0 );
			screen.Print( "hello world", 2, 2, 0xffffff );
			screen.Line( 2, 20, 160, 20, 0xff0000 );
			//raytracer wordt opgeroepen

			screen.Box(screen.width / 2, screen.height / 6 * 5, screen.width / 2 + 1, screen.height / 6 * 5 + 1, color(255, 255, 255));

		}

		//keyboard & mouse input

		int color(int red, int green, int blue)
		{
			return (red << 16) + (green << 8) + blue;
		}
	}
}
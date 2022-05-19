namespace Template
{
	class MyApplication
	{
		// member variables
		public Surface screen;
		public Scene scene;
	
		// initialize
		public void Init()
		{
			scene = new Scene();

		}
		// tick: renders one frame
		public void Tick()
		{
			screen.Clear( 0 );
			screen.Print( "hello world", 2, 2, 0xffffff );
			screen.Line( 2, 20, 160, 20, 0xff0000 );
		}
	}
}
namespace Template
{
	class MyApplication
	{
		// member variables
		public Surface screen;
		public Scene scene;
		public Raytracer raytracer;
	
		// initialize
		public void Init()
		{
			raytracer = new Raytracer();

		}
		// tick: renders one frame
		public void Tick()
		{
			screen.Clear( 0 );
			//screen.Print( "hello world", 2, 2, 0xffffff );
			//screen.Line( 2, 20, 160, 20, 0xff0000 );

			raytracer.Render();
		}
	}
}
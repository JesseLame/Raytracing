using System;
namespace Template
{
	class MyApplication
	{
		// member variables
		public Surface screen, debugscreen, rtscreen;
		public Scene scene;
		public Camera camera;
		public OpenTK.Vector3[] pixels;

		
	
		// initialize
		public void Init()
		{
			scene = new Scene();

			debugscreen = new Surface(10, 10);
			debugscreen.Clear(0x0000ff);
			rtscreen = new Surface(512, 512);
			rtscreen.Clear(0xffff00);
			pixels = new OpenTK.Vector3[512 * 512];

			foreach(Primitive p in scene.GetPrimitives())
            {
				p.debugDraw();
            }
			debugscreen.Box(debugscreen.width / 2, debugscreen.height / 6 * 5, debugscreen.width / 2 + 1, debugscreen.height / 6 * 5 + 1, color(0,0,255));
			debugscreen.Line(debugscreen.width / 2 - 4, debugscreen.height / 6 * 4, debugscreen.width / 2 + 5, debugscreen.height / 6 * 4, color(255, 255, 255));

			

		}
		// tick: renders one frame
		public void Tick()
		{
			/*debugscreen.CopyTo(screen, 0, 0);
			rtscreen.CopyTo(screen, 512, 0);
			for (int y = 0; y < 512; y++)
			{
				bool debug = false;
				for (int x = 0; x < 512; x++)
				{
					if (debug == true && ((x & 7) == 0))
					{
						//draw debug stuff
					}
					
					//figure out this color using raytracer
					float u = (float)x / 512;
					float v = (float)y / 512;

					//find p on virtual screen plane
					Vector3 = camera.GetP(u, v);


					//create ray
					Ray r = new Ray(())
					r.position = camera.GetEye();
					r.d = Vector3.Normalize(p - r.0);
					r.t = 1e37f;

					// find nearest intersection
					foreach (primitive item in ..)
					{
						T = intersection();
						if (T < r.t)
						{
							...
                            }

					}

				}
			}*/
			//finalize screen
			for (int i = 0; i < 512 * 512; i++)
			{
				OpenTK.Vector3 hdr = pixels[i];
				int red = Math.Min((int)(hdr.X * 256), 255);
				int green = Math.Min((int)(hdr.Y * 256), 255);
				int blue = Math.Min((int)(hdr.Z * 256), 255);
				rtscreen.pixels[i] = (red << 16) + (green << 8) + blue;

			}
		}
		int color(int red, int green, int blue)
		{
			return (red << 16) + (green << 8) + blue;
		}
	}
}
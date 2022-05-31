using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Windows.Input;

// The template provides you with a window which displays a 'linear frame buffer', i.e.
// a 1D array of pixels that represents the graphical contents of the window.

// Under the hood, this array is encapsulated in a 'Surface' object, and copied once per
// frame to an OpenGL texture, which is then used to texture 2 triangles that exactly
// cover the window. This is all handled automatically by the template code.

// Before drawing the two triangles, the template calls the Tick method in MyApplication,
// in which you are expected to modify the contents of the linear frame buffer.

// After (or instead of) rendering the triangles you can add your own OpenGL code.

// We will use both the pure pixel rendering as well as straight OpenGL code in the
// tutorial. After the tutorial you can throw away this template code, or modify it at
// will, or maybe it simply suits your needs.

namespace Template
{
	public class OpenTKApp : GameWindow
	{
		static int screenID;            // unique integer identifier of the OpenGL texture
		static MyApplication app;       // instance of the application
		static bool terminated = false; // application terminates gracefully when this is true
		Raytracer raytracer;
		protected override void OnLoad(EventArgs e)
		{
			// called during application initialization
			GL.ClearColor(0, 0, 0, 0);
			GL.Enable(EnableCap.Texture2D);
			GL.Disable(EnableCap.DepthTest);
			GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
			ClientSize = new Size(1280, 800);
			raytracer = new Raytracer();
			raytracer.screen = new Surface(Width, Height);
			Sprite.target = raytracer.screen;
			screenID = raytracer.screen.GenTexture();
			raytracer.Init();
		}
		protected override void OnUnload(EventArgs e)
		{
			// called upon app close
			GL.DeleteTextures(1, ref screenID);
			Environment.Exit(0);      // bypass wait for key on CTRL-F5
		}
		protected override void OnResize(EventArgs e)
		{
			// called upon window resize. Note: does not change the size of the pixel buffer.
			GL.Viewport(0, 0, Width, Height);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
		}



		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			// called once per frame; app logic
			var keyboard = OpenTK.Input.Keyboard.GetState();
			if (keyboard[OpenTK.Input.Key.Escape]) terminated = true;
		}

		

		protected override void OnKeyPress(OpenTK.KeyPressEventArgs e)
		{
			float movement = 1f;

			Matrix3 rotLeft = Matrix3.CreateRotationY(-0.1f);
			Matrix3 rotRight = Matrix3.CreateRotationY(0.1f);
			Matrix3 rotUp = Matrix3.CreateRotationX(0.1f);
			Matrix3 rotDown = Matrix3.CreateRotationX(-0.1f);

			//rotate left
			if (e.KeyChar == 'j')
			{
				raytracer.camera.lookDirection = Vector3.Transform(raytracer.camera.lookDirection, rotLeft);
			}
			//Rotate right
			else if (e.KeyChar == 'l')
			{
				raytracer.camera.lookDirection = Vector3.Transform(raytracer.camera.lookDirection, rotRight);
			}
			//Rotate up
			else if (e.KeyChar == 'i')
			{
				raytracer.camera.upDirection = Vector3.Transform(raytracer.camera.upDirection, rotUp);
			}
			//Rotate down
			else if (e.KeyChar == 'k')
			{
				raytracer.camera.upDirection = Vector3.Transform(raytracer.camera.upDirection, rotDown);
			}

			//To left
			else if (e.KeyChar == 'a')
			{
				raytracer.camera.position -= new Vector3(movement, 0, 0);

				Console.WriteLine("A");
			}
			//To the right
			else if (e.KeyChar == 'd')
			{
				raytracer.camera.position += new Vector3(movement, 0, 0);
				Console.WriteLine("D");
			}

			//Forward
			else if (e.KeyChar == 'w')
			{
				raytracer.camera.position += new Vector3(0, 0, movement);
				Console.WriteLine("W");
			}

			//Backward
			else if (e.KeyChar == 's')
			{
				raytracer.camera.position -= new Vector3(0, 0, movement);
				Console.WriteLine("S");
			}

			//Up
			else if (e.KeyChar == 'q')
			{
				raytracer.camera.position += new Vector3(0, movement, 0);
				Console.WriteLine("Q");
			}

			//Down
			else if (e.KeyChar == 'e')
			{
				raytracer.camera.position -= new Vector3(0, movement, 0);
				Console.WriteLine("E");
			}

			Console.WriteLine("Update");
			raytracer.camera.SetScreenPlane();
		}

		protected override void OnRenderFrame( FrameEventArgs e )
		{
			GL.ClearColor(Color.Black);
			GL.Enable(EnableCap.Texture2D);
			GL.Disable(EnableCap.DepthTest);
			GL.Color3(1.0f, 1.0f, 1.0f);

			
			//raytracer.debugOutput();
			raytracer.Render();
			//check if we have to exit
			if (terminated)
			{
				Exit();
				return;
			}

			//renders the rays
			int box = Math.Min(Width, Height);
			GL.Viewport(0, 0, box, box);
			//raytracer.Render();

			// clear window contents            
			GL.Clear(ClearBufferMask.ColorBufferBit);

			// setup camera
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();

			//prepares to draw the screen texture
			GL.ClearColor(Color.Black);
			GL.Enable(EnableCap.Texture2D);
			GL.Disable(EnableCap.DepthTest);
			GL.Color3(1.0f, 1.0f, 1.0f);

			// convert Game.screen to OpenGL texture
			GL.BindTexture(TextureTarget.Texture2D, screenID);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
						   raytracer.screen.width, raytracer.screen.height, 0,
						   OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
						   PixelType.UnsignedByte, raytracer.screen.pixels
						 );


			// draw screen filling quad
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-1.0f, -1.0f);
			GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(1.0f, -1.0f);
			GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(1.0f, 1.0f);
			GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-1.0f, 1.0f);
			GL.End();

			//reverts the needed properties to convert the screen to texture
			GL.Enable(EnableCap.DepthTest);
			GL.Disable(EnableCap.Texture2D);
			GL.Clear(ClearBufferMask.DepthBufferBit);

			

			//renders the debug
			GL.PushAttrib(AttribMask.ViewportBit); //Push current viewport attributes to a sta ck
			GL.Viewport(Width >> 1, 0, box, box); //Create a new viewport bottom left for the debug output.
			//raytracer.debugOutput();
			raytracer.drawDebug();
			//we want to texture stuff again and restor our viewport

			GL.PopAttrib();//Reset to the old viewport.
			GL.Enable(EnableCap.Texture2D);

			//tell openTK we are gonna work on or next frame
			SwapBuffers();

			//write how long the frame took to complete
			//Console.WriteLine(processedframes / (stopwatch.ElapsedMilliseconds * 0.001f));
		}

		public static void Main( string[] args )
		{
			// entry point
			using( OpenTKApp app = new OpenTKApp() ) { app.Run( 30.0, 0.0 ); }
		}
	}
}
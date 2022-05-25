﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Template
{
    class Raytracer
    {
        public Scene scene;
        public Camera camera;
        public Surface screen;

        public Raytracer()
        {
            scene = new Scene();
        }

        public void Init()
        {
            camera = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 1, 0), screen);
        }

        public void Render()
        {
            screen.Clear(0);
            for(int x = 0; x < screen.width; x++)
            {
                for(int y = 0; y < screen.height; y++)
                {
                    //Ray ray = camera.Ray(x, y);
                    //Vector3 color = scene.ClosestIntersection(ray);

                    //screen.pixels[x + y * screen.width] = MixColor((int)(color.X * 255), (int)(color.Y * 255), (int)(color.Z * 255));
                    screen.pixels[x + y * screen.width] = MixColor(255,0,0);
                }
            }
        }

        public void drawDebug()
        {
            screen.Clear(0);

            GL.Disable(EnableCap.Texture2D);

            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 m = Matrix4.CreateScale(1 / 10.0f);
            GL.LoadMatrix(ref m);

            int c2x = screen.width / 4;
            int c2y = ((screen.height / 6) * 5);

            //screen.Box((int)camera.position.X, (int)camera.position.Y, (int)camera.position.X + 1, (int)camera.position.Y + 1, MixColor(0, 255, 0));
            screen.Box(c2x - 2, c2y - 2, c2x + 2, c2y + 2, MixColor(255, 255, 255));

            //screen.Line(screen.width / 5, (screen.height / 6) * 4, (screen.width / 5) + screen.width / 10, (screen.height / 6) * 4, MixColor(0, 255, 0));

            Vector2 shiftVector = new Vector2();
            shiftVector.X = c2x - camera.position.X;
            shiftVector.Y = c2y - camera.position.Z;

            double alpha = (screen.width / 10) /
                Math.Sqrt(Math.Pow(camera.plane2.X - camera.plane1.X, 2) +
                Math.Pow(camera.plane2.Z - camera.plane1.Z, 2));

            Vector2 toLeft = new Vector2((camera.position.X - camera.plane1.X), (camera.position.Z - camera.plane1.Z));
            Vector2 toRight = new Vector2((camera.position.X - camera.plane2.X), (camera.position.Z - camera.plane2.Z)); ;



            screen.Line((int)(c2x + toLeft.X * alpha), (int)(c2y + toLeft.Y * alpha),
                (int)(c2x + toRight.X * alpha), (int)(c2y + toRight.Y * alpha), MixColor(255, 255, 255));

            foreach (Primitive p in scene.primitives)
            {
                p.drawDebug(screen);
            }
        }

        public void debugOutput()
        {
            //this should be made procedural to the screensize           

            //we dont want to texture our lines
            GL.Disable(EnableCap.Texture2D);

            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 m = Matrix4.CreateScale(1 / 10.0f);
            GL.LoadMatrix(ref m);


            //Draw the camera -- must be a point according to the assignment
            GL.Color3(0.8f, 0.2f, 0.2f);
            GL.Begin(PrimitiveType.Points);
            GL.Vertex2(camera.position.Xz);
            GL.End();

            //Draw camera end
            Vector2 sv1 = (camera.plane1.Xz - camera.position.Xz) * 30.0f;
            Vector2 sv2 = (camera.plane2.Xz - camera.position.Xz) * 30.0f;

            GL.Color3(0.8f, 0.5f, 0.5f);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(camera.position.Xz);
            GL.Vertex2((camera.position.Xz + sv1));

            GL.Vertex2(camera.position.Xz);
            GL.Vertex2((camera.position.Xz + sv2));

            GL.Color3(0.2f, 1.0f, 0.7f);

            //draw the rays of the 255 row with an interval of 64

            //for (int x = 0; x <= screen.width; x += 64)
            //{
            //    Ray currentray = camera.getRay(x, WIDTH >> 1);
            //    debugRay(camera.position, currentray, MAXDEPTH);
            //}

            GL.Color3(0.4f, 1.0f, 0.4f);
            GL.Vertex2(camera.plane1.Xz);
            GL.Vertex2(camera.plane2.Xz);
            GL.End();

            //foreach (Primitive primitive in scene.allPrimitives)
            //{
            //    primitive.debugOutput();
            //}
        }


        public int MixColor(int red, int green, int blue)
        {
            return (red << 16) + (green << 8) + blue;
        }
    }
}

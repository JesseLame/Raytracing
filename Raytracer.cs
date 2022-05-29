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
            camera = new Camera(new Vector3(0,0,-6), new Vector3(0, 0, 1), new Vector3(0, 1, 0), screen);
        }

        public void Render()
        {
            screen.Clear(0);
            for(int x = 0; x < screen.width; x++)
            {
                for(int y = 0; y < screen.height; y++)
                {
                    Ray ray = camera.Ray(x, y);
                    Intersection inter = scene.ClosestIntersection(ray);
                    Vector3 color = calcPixelColor(inter);

                    screen.pixels[x + y * screen.width] = MixColor((int)(color.X * 255), (int)(color.Y * 255), (int)(color.Z * 255));
                }
            }
        }

        //berekent bij Punt I van intersection met ray de kleur van de pixel 
        public Vector3 calcPixelColor(Intersection inter)
        {
            Vector3 pixelColor = inter.nearestPrimetive.color;
            float distanceLight;
           //vector from light to intersection point
            Vector3 l;

            Vector3 i = inter.point;
            

            //Start with black 
            //If there are no light sources it remains black
            Vector3 retColor = new Vector3(0, 0, 0);

            //No intersection pizel should be black
            if (inter.nearestPrimetive == null)
                return retColor;

            retColor = inter.nearestPrimetive.material.calcLight(i,inter, scene, camera.position);
            //retColor = calculateillumination(i, inter.nearestPrimetive, inter);
            return retColor;
        }

        


        public bool hasLight(Vector3 i)
        {
            float distanceLight;
            Vector3 l;
            Vector3 lightEnergy;

            foreach(Light light in scene.lights)
            {
                l = light.position - i;
                distanceLight = l.Length;
                l.Normalize();

                Intersection inter = scene.ClosestIntersection(new Ray(l, i, distanceLight));
                if ((inter.distance < distanceLight - 1) && 1 < inter.distance)
                { }
                else
                {
                    //lightColor(light, distanceLight);
                }
            }


            return true;
        }

        //Calc of light energy
        //public Vector3 lightColor(Light light, float radius)
        //{
        //    Vector3 lightEnergy = light.position * (float)(1 / Math.Pow(radius, 2));

        //}

        

        public void drawDebug()
        {
            screen.Clear(0);


            GL.Disable(EnableCap.Texture2D);

            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 m = Matrix4.CreateScale(1 / 10.0f);
            GL.LoadMatrix(ref m);

            GL.Begin(PrimitiveType.Points);
            GL.Vertex2(camera.position.Xz);
            GL.Color3(0, 255, 0);
            foreach(Light light in scene.lights )
            {
                GL.Vertex2(light.position.Xz);
            }
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(camera.plane1.Xz);
            GL.Vertex2(camera.plane2.Xz);
            GL.End();

            foreach (Primitive p in scene.primitives)
            {
                p.drawDebug(screen);
            }

            foreach(Light l in scene.lights)
            {
                l.drawDebug();
            }

            GL.Begin(PrimitiveType.Lines);
            for (int i = 0; i <= screen.width; i += 64)
            {
                GL.Color3(new Vector3(1, 0, 0));
                Ray ray = camera.Ray(i, screen.height / 2);
                Intersection inter = scene.ClosestIntersection(ray);

                GL.Vertex2(camera.position.Xz);
                GL.Vertex2(camera.position.X + ray.direction.X * inter.distance, camera.position.Z + ray.direction.Z * inter.distance);
                if(inter.nearestPrimetive != null)
                    drawShadowRay(ray, inter);
            }
            GL.End();

            
        }

        public void drawShadowRay(Ray ray, Intersection inter)
        {
            float distanceLight;
            Vector3 l;
            Vector3 i = inter.point;

            //Start with black 
            //If there are no light sources it remains black
            Vector3 retColor = new Vector3(0, 0, 0);

            //GL.Begin(PrimitiveType.Lines);
            GL.Color3(new Vector3(1, 1, 1));
            foreach (Light light in scene.lights)
            {
                l = light.position - i;
                distanceLight = l.Length;
                l.Normalize();

                Intersection obstacles = scene.ClosestIntersection(new Ray(l, i, distanceLight));

                //Ray has a light source
                if (!inter.nearestPrimetive.material.shadow(obstacles.distance, distanceLight))
                {
                    GL.Vertex2(camera.position.X + ray.direction.X * inter.distance, camera.position.Z + ray.direction.Z * inter.distance);
                    //GL.Vertex2(inter.point.Xz);
                    GL.Vertex2(light.position.Xz);
                }
            }
            //GL.End();
        }


        public int MixColor(int red, int green, int blue)
        {
            return (Math.Min(red, 255) << 16) + (Math.Min(green, 255) << 8) + Math.Min(blue, 255);
        }
    }
}

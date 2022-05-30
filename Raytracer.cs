using System;
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
        int MAXNUMBERBOUNCES = 5;

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
                    //Vector3 color = trace(ray);

                    //Intersection inter = scene.ClosestIntersection(ray);
                    Vector3 color = calcPixelColor(ray, MAXNUMBERBOUNCES);

                    screen.pixels[x + y * screen.width] = MixColor((int)(color.X * 255), (int)(color.Y * 255), (int)(color.Z * 255));
                }
            }
        }

        public Vector3 trace(Ray ray)
        {
            Intersection inter = scene.ClosestIntersection(ray);
            Vector3 color = calcPixelColor(ray, MAXNUMBERBOUNCES);

            if (inter.nearestPrimetive == null)
                return Vector3.Zero;

            if (inter.nearestPrimetive.material.isMirror && ray.numberofBounces < 20)
            {
                Ray secondaryRay = new Ray(ray.direction - (2 * Vector3.Dot(ray.direction, inter.normal) * inter.normal),inter.point,0);
                secondaryRay.numberofBounces = ray.numberofBounces + 1;
                color = color * trace(secondaryRay);
            }

            return color;
        }

        //berekent bij Punt I van intersection met ray de kleur van de pixel 
        public Vector3 calcPixelColor(Ray ray, int numberOfbounces)
        {
            Intersection inter = scene.ClosestIntersection(ray);

           //vector from light to intersection point
            Vector3 l;

            Vector3 i = inter.point;
            

            //Start with black 
            //If there are no light sources it remains black
            Vector3 retColor = new Vector3(0, 0, 0);

            //No intersection pizel should be black
            //Or tomany bounces als return black
            if (inter.nearestPrimetive == null || numberOfbounces == 0)
                return retColor;

            retColor = inter.nearestPrimetive.material.calcLight(i,inter, scene, camera.position);

            if(inter.nearestPrimetive.material.isMirror)
            {                
                Ray secondaryRay = makeSecondaryRay(ray, inter);
                Vector3 reflectionColor = calcPixelColor(secondaryRay, numberOfbounces - 1);
                retColor = inter.nearestPrimetive.material.reflaction * reflectionColor + (1 - inter.nearestPrimetive.material.reflaction) * retColor;
            }

            return retColor;
        }

        public Ray makeSecondaryRay(Ray ray, Intersection inter)
        {
            return new Ray(ray.direction - (2 * Vector3.Dot(ray.direction, inter.normal) * inter.normal), inter.point * 0.1f, 10f);
        }

        public void drawDebug()
        {
            screen.Clear(0);


            GL.Disable(EnableCap.Texture2D);

            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 m = Matrix4.CreateScale(1 / 10.0f);
            GL.LoadMatrix(ref m);

            GL.Begin(PrimitiveType.Points);
            GL.Vertex2(camera.position.Xz);
            GL.Color3(0, 1, 0);
            foreach(Light light in scene.lights )
            {
                GL.Vertex2(light.position.Xz);
            }
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Color3(1, 1, 1);
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
                {
                    drawShadowRay(ray, inter);

                    if (inter.nearestPrimetive.material.isMirror)
                        drawSecondaryRay(ray, inter);
                }                
            }
            GL.End();

            
        }

        public void drawSecondaryRay(Ray ray, Intersection inter)
        {

            Ray secondRay = makeSecondaryRay(ray, inter);
            GL.Color3(new Vector3(1, 1, 0));
            GL.Vertex2(inter.point.Xz);
            Vector3 point = secondRay.origin + secondRay.direction * secondRay.distance;
            GL.Vertex2(point.Xz);

        }

        public void drawShadowRay(Ray ray, Intersection inter)
        {
            float distanceLight;
            Vector3 l;
            Vector3 i = inter.point;

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
        }


        public int MixColor(int red, int green, int blue)
        {
            return (Math.Min(red, 255) << 16) + (Math.Min(green, 255) << 8) + Math.Min(blue, 255);
        }
    }
}

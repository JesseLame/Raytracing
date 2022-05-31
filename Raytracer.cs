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
                    //Create a primary ray
                    Ray ray = camera.Ray(x, y);
                    //Calculate the pixel color
                    Vector3 color = calcPixelColor(ray, MAXNUMBERBOUNCES);

                    screen.pixels[x + y * screen.width] = MixColor((int)(color.X * 255), (int)(color.Y * 255), (int)(color.Z * 255));
                }
            }
        }


        //berekent bij Punt I van intersection met ray de kleur van de pixel 
        public Vector3 calcPixelColor(Ray ray, int numberOfbounces)
        {
            //Closest intersection for the ray
            Intersection inter = scene.ClosestIntersection(ray);
            //Intersection point minus epsilon so there is no itersection with itself
            Vector3 i = inter.point - 0.001f * ray.direction;
            

            //Start with black 
            //If there are no light sources it remains black
            Vector3 retColor = new Vector3(0, 0, 0);

            //No intersection pizel should be black
            //Or to many bounces als return black
            if (inter.nearestPrimetive == null || numberOfbounces == 0)
                return retColor;

            //Calculte the color based on the material
            retColor = inter.nearestPrimetive.material.calcLight(i, inter, scene, camera.position);

            //If material is mirror recursevilt call calcPixelColor with max bounces of 5
            //Set number of bounces in MAXNUMBEROFBOUNCES
            if(inter.nearestPrimetive.material.isMirror)
            {                
                Ray secondaryRay = makeSecondaryRay(ray, inter);
                Vector3 reflectionColor = calcPixelColor(secondaryRay, numberOfbounces - 1);
                retColor = inter.nearestPrimetive.material.reflaction * reflectionColor; 
            }

            
            return retColor;
        }

        //Create a secondary ray for mirror bounces based on the slides
        public Ray makeSecondaryRay(Ray ray, Intersection inter)
        {
            return new Ray(ray.direction - (2 * Vector3.Dot(ray.direction, inter.normal) * inter.normal), inter.point, 10f);
        }

        //Draw the debug screen
        public void drawDebug()
        {
            screen.Clear(0);

            GL.Disable(EnableCap.Texture2D);

            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 m = Matrix4.CreateScale(1 / 10.0f);
            GL.LoadMatrix(ref m);

            //Draw the camera
            GL.Begin(PrimitiveType.Points);
            GL.Color3(1, 1, 1);
            GL.Vertex2(camera.position.Xz);
            GL.End();

            //Draw the camera screen
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(1, 1, 1);
            GL.Vertex2(camera.plane1.Xz);
            GL.Vertex2(camera.plane2.Xz);
            GL.End();

            //For all the primitives draw 
            foreach (Primitive p in scene.primitives)
            {
                p.drawDebug(screen);
            }

            //Draw all the lights
            foreach(Light l in scene.lights)
            {
                l.drawDebug();
            }

            //Draw primary rays with interval of 64
            GL.Begin(PrimitiveType.Lines);
            for (int i = 0; i <= screen.width; i += 64)
            {
                GL.Color3(new Vector3(1, 0, 0));
                Ray ray = camera.Ray(i, screen.height / 2);
                Intersection inter = scene.ClosestIntersection(ray);

                GL.Vertex2(camera.position.Xz);
                GL.Vertex2(camera.position.X + ray.direction.X * inter.distance, camera.position.Z + ray.direction.Z * inter.distance);
                
                //If there is an intersection
                if(inter.nearestPrimetive != null)
                {
                    //Draw shadowray
                    drawShadowRay(ray, inter);

                    //For mirror draw secondary ray
                    if (inter.nearestPrimetive.material.isMirror)
                        drawSecondaryRay(ray, inter);
                }                
            }
            GL.End();

            
        }

        //Draw the secondary ray for mirrors
        public void drawSecondaryRay(Ray ray, Intersection inter)
        {
            Ray secondRay = makeSecondaryRay(ray, inter);
            GL.Color3(new Vector3(1, 1, 0));
            GL.Vertex2(inter.point.Xz);
            Vector3 point = secondRay.origin + secondRay.direction * secondRay.distance;
            GL.Vertex2(point.Xz);
        }

        //Draw shadow ray
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

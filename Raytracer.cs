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

        public Raytracer()
        {
            scene = new Scene();
        }

        public void Init()
        {
            camera = new Camera(new Vector3(0,0,-10), new Vector3(0, 0, 1), new Vector3(0, 1, 0), screen);
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
            Vector3 lightEnergy;

            foreach (Light light in scene.lights)
            {
                
                l = light.position - i;
                distanceLight = l.Length;
                l.Normalize();

                Intersection shadowIntersect = scene.ClosestIntersection(new Ray(l, i, distanceLight));
             //pixel wordt belicht door lichtbron; er is geen obstakel
                if (!((shadowIntersect.distance < distanceLight - 1) && 1 < shadowIntersect.distance))
                {
                    
                    return reflectedLight(light, distanceLight, pixelColor, shadowIntersect, l);
                }
                //licht wordt tegengehouden door ander object
                else
                {
                    return inter.nearestPrimetive.color;
                }
            }
            return new Vector3(0,1,0);

        }

       /* 
        public void hasLight(Vector3 i)
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
                    lightColor(light, distanceLight);
                }
            }


            return true;
        }
*/
        //reflected light
        public Vector3 reflectedLight(Light light, float radius, Vector3 color, Intersection inter, Vector3 l)
        {
            Vector3 result = new Vector3();
            //waarom staat light.position in de lightEnergy?
            Vector3 lightEnergy =light.intensity ;

            //reflected light => entry-wise product
            result.X = lightEnergy.X * color.X;
            result.Y = lightEnergy.Y * color.Y;
            result.Z = lightEnergy.Z * color.Z;

            //1/r^2 * ELight o Kd * max(0, N * L)
            /*float a = (float)(1 / Math.Pow(radius, 2));
            float b = Math.Max(0, Vector3.Dot(inter.normal, -l));*/
            result = (float)(1 / Math.Pow(radius, 2)) * result * Math.Max(0, Vector3.Dot(inter.normal, -l));
            return result;

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


            //int c2x = screen.width / 4;
            //int c2y = ((screen.height / 6) * 5);

            ////screen.Box((int)camera.position.X, (int)camera.position.Y, (int)camera.position.X + 1, (int)camera.position.Y + 1, MixColor(0, 255, 0));
            //screen.Box(c2x - 2, c2y - 2, c2x + 2, c2y + 2, MixColor(255, 255, 255));

            ////screen.Line(screen.width / 5, (screen.height / 6) * 4, (screen.width / 5) + screen.width / 10, (screen.height / 6) * 4, MixColor(0, 255, 0));

            //Vector2 shiftVector = new Vector2();
            //shiftVector.X = c2x - camera.position.X;
            //shiftVector.Y = c2y - camera.position.Z;

            //double alpha = (screen.width / 10) /
            //    Math.Sqrt(Math.Pow(p2.X - p1.X, 2) +
            //    Math.Pow(p2.Z - p1.Z, 2));

            //Vector2 toLeft = new Vector2((camera.position.X - p1.X), (camera.position.Z - p1.Z));
            //Vector2 toRight = new Vector2((camera.position.X - p2.X), (camera.position.Z - p2.Z)); ;



            //screen.Line((int)(c2x + toLeft.X * alpha), (int)(c2y + toLeft.Y * alpha),
            //    (int)(c2x + toRight.X * alpha), (int)(c2y + toRight.Y * alpha), MixColor(255, 255, 255));

            foreach (Primitive p in scene.primitives)
            {
                p.drawDebug(screen);
            }

            GL.Begin(PrimitiveType.Lines);
            GL.Color3(new Vector3(1,0,0));
            for (int i = 0; i <= screen.width; i += 128)
            {
                Ray ray = camera.Ray(i, screen.height / 2);
                Intersection inter = scene.ClosestIntersection(ray);

                GL.Vertex2(camera.position.Xz);
                GL.Vertex2(camera.position.X + ray.direction.X * inter.distance, camera.position.Z + ray.direction.Z * inter.distance);
            }
            GL.End();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    public class Scene
    {
        public List<Primitive> primitives;
        List<Light> lights;

        Primitive floorPlane;

        public Scene()
        {
            primitives = new List<Primitive>();
            lights = new List<Light>();

            floorPlane = new Plane(new Vector3(0, 1, 0), 4f, new Vector3(0, 0, 0));

            Primitive sphere1 = new Sphere(new Vector3(-3.5f, 0, 6f), 1.5f, new Vector3(1, 0, 0));
            Primitive sphere2 = new Sphere(new Vector3(0f, 0, 6f), 1.5f, new Vector3(0, 1, 0));
            Primitive sphere3 = new Sphere(new Vector3(3.5f, 0, 6f), 1.5f, new Vector3(0, 0, 1));

            //primitives.Add(floorPlane);
            primitives.Add(sphere1);
            primitives.Add(sphere2);
            primitives.Add(sphere3);
        }

        public Vector3 ClosestIntersection(Ray ray)
        {
            float closest = Int32.MaxValue;
            Intersection closestIntersect;
            //Return black when there are no intersections
            Vector3 retColor = new Vector3(0,0,0);

            foreach(Primitive p in primitives)
            {
               if(p.intersects(ray) < closest)
                {
                    closest = p.intersects(ray);
                    closestIntersect = new Intersection(closest, p, new Vector3());
                    retColor = closestIntersect.nearestPrimetive.color;
                }
            }                

            return retColor;
        }
    }
}

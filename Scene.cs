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
        public List<Light> lights;

        Primitive floorPlane;

        public Scene()
        {
            primitives = new List<Primitive>();
            lights = new List<Light>();

            floorPlane = new Plane(new Vector3(0, 1, 0),new Vector3(), new Vector3(), new Vector3(0, -2, 6) ,new Diffuse(new Vector3(1,1,0)));

            Primitive sphere1 = new Sphere(new Vector3(-3.5f, 0, 6f), 1.5f,new Diffuse(new Vector3(1, 0, 0)));
            Primitive sphere2 = new Sphere(new Vector3(0f, 0, 6f), 1.5f, new Diffuse(new Vector3(0, 1, 0)));
            Primitive sphere3 = new Sphere(new Vector3(3.5f, 0, 6f), 1.5f,new Glossy(new Vector3(1, 1,1), new Vector3(0, 0, 1), 2));
            //Primitive sphere4 = new Sphere(new Vector3(6.5f, 0, 6f), 1.5f, new Diffuse(new Vector3(0, 0, 1)));
            //Primitive sphere4 = new Sphere(new Vector3(3.5f, 0, 6f), 1.5f, new Diffuse(new Vector3(0, 0, 1)));

            //Light light = new Light(new Vector3(-5, 4, 5.5f), new Vector3(1,1,1));
            Light light = new Light(new Vector3(0, 8, -2), new Vector3(10,10,10));
            //Light light = new Light(new Vector3(-5, 0, 2), new Vector3(1, 1, 1));

            lights.Add(light);

            //Primitive sphere1 = new Sphere(new Vector3(3, 1, 0), 1.5f, new Vector3(1, 0, 0));
            //Primitive sphere2 = new Sphere(new Vector3(0f, 1, -1), 1.5f, new Vector3(0, 1, 0));
            //Primitive sphere3 = new Sphere(new Vector3(-5,1, 0), 1.5f, new Vector3(0, 0, 1));
            //Primitive sphere4 = new Sphere(new Vector3(-2, 1, 0), 1.5f, new Vector3(0, 0, 1));

            primitives.Add(floorPlane);
            primitives.Add(sphere1);
            primitives.Add(sphere2);
            primitives.Add(sphere3);
            //primitives.Add(sphere4);
        }

        public Intersection ClosestIntersection(Ray ray)
        {
            float closest = Int32.MaxValue;
            Intersection closestIntersect = new Intersection(int.MaxValue,null);
            //Return black when there are no intersections
            Vector3 retColor = new Vector3(0,0,0);

            foreach(Primitive p in primitives)
            {
               if(p.intersects(ray) < closest)
                {
                    closest = p.intersects(ray);
                    closestIntersect = new Intersection(closest, p);
                    closestIntersect.point = ray.origin + ray.direction * closestIntersect.distance;
                    //retColor = closestIntersect.nearestPrimetive.color;
                }
            }
            closestIntersect.calculateNormal();
            return closestIntersect;
        }
    }
}

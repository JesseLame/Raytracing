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

            floorPlane = new Plane(new Vector3(0, 1, 0), new Vector3(0, -2, 6) ,new Diffuse(new Vector3(1,1,1), true));

            Primitive sphere1 = new Sphere(new Vector3(-3.5f, 1, 6f), 1.5f,new Diffuse(new Vector3(1, 0, 0), true));
            Primitive sphere2 = new Sphere(new Vector3(0f, 0, 6f), 1.5f, new Mirror(new Vector3(1, 1, 1), 1));
            Primitive sphere3 = new Sphere(new Vector3(3.5f, 0, 5f), 1.5f,new Glossy(1, new Vector3(0, 0, 1), 2, false));
            //Primitive sphere4 = new Sphere(new Vector3(6.5f, 0, 6f), 1.5f, new Diffuse(new Vector3(0, 0, 1)));
            //Primitive sphere4 = new Sphere(new Vector3(3.5f, 0, 6f), 1.5f, new Diffuse(new Vector3(0, 0, 1)));


            Light light1 = new Light(new Vector3(0, 8, -2), new Vector3(5,5,5));
            Light light2 = new Light(new Vector3(-5, 0, 2), new Vector3(2.5f, 2.5f, 2.5f));

            lights.Add(light1);
            lights.Add(light2);

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

        //Returns the closest intersection
        //If there is no intersection returns intersection with primitive null and distance maxValue
        public Intersection ClosestIntersection(Ray ray)
        {
            float closest = Int32.MaxValue;
            Intersection closestIntersect = new Intersection(int.MaxValue,null);

            foreach(Primitive p in primitives)
            {
               if(p.intersects(ray) < closest)
                {
                    closest = p.intersects(ray);
                    closestIntersect = new Intersection(closest, p);
                    closestIntersect.point = ray.origin + ray.direction * closestIntersect.distance;
                }
            }
            closestIntersect.calculateNormal();
            return closestIntersect;
        }
    }
}

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
        List<Primitive> primitives;
        List<Light> lights;

        Primitive floorPlane;

        public Scene()
        {
            primitives = new List<Primitive>();
            lights = new List<Light>();

            floorPlane = new Plane(new Vector3(0, 1, 0), 4f, new Vector3(0, 0, 0));

            Primitive sphere1 = new Sphere(new Vector3(-3.5f, -1.5f, 6f), 1.5f, new Vector3(1, 0, 0));
            Primitive sphere2 = new Sphere(new Vector3(0f, -1.5f, 6f), 1.5f, new Vector3(0, 1, 0));
            Primitive sphere3 = new Sphere(new Vector3(3.5f, -1.5f, 6f), 1.5f, new Vector3(0, 0, 1));

            primitives.Add(floorPlane);
            primitives.Add(sphere1);
            primitives.Add(sphere2);
            primitives.Add(sphere3);
        }

        public List<Primitive> GetPrimitives()
        {
            return primitives;
        }
    }
}

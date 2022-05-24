using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    class Primitive
    {
        Vector3 position;
        Vector3 color;

        public Primitive(Vector3 p,  Vector3 c)
        {
            position = p;
            color = c;
        }
        
    }

    class Sphere : Primitive
    {
        float radius;

        public Sphere(Vector3 position, float radius, Vector3 color) : base(position, color)
        {
            this.radius = radius;
        }

        public void debugdraw()
        {
            
        }
    }

    class Plane : Primitive
    {
        float distance;

        public Plane(Vector3 normal, float distance, Vector3 color) : base(normal, color)
        {
            this.distance = distance;
        }
            public debugDraw()
            {

            }
    }
}

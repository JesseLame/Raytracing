using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace INFOGR2022Template
{
    class Primitive
    {
    }

    class Sphere : Primitive
    {
        Vector3 position;
        float radius;

        public Sphere(Vector3 p, float r)
        {
            position = p;
            radius = r;
        }

    }

    class Plane : Primitive
    {

    }
}

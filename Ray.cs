using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    public class Ray
    {
        public Vector3 direction;
        public Vector3 origin;
        public float distance;
        public int numberofBounces;

        public Ray(Vector3 direction, Vector3 origin, float distance)
        {
            this.direction = direction.Normalized();
            this.origin = origin;
            this.distance = distance;
            numberofBounces = 1;
        }
    }
}

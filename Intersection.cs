using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    public class Intersection
    {
        public float distance;
        public Primitive nearestPrimetive;
        public Vector3 normal;
        public Vector3 point;

        public Intersection(float distance, Primitive nearestPrimetive, Vector3 normal)
        {
            this.distance = distance;
            this.nearestPrimetive = nearestPrimetive;
            this.normal = normal;
        }
    }
}

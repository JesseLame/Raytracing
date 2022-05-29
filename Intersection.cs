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

        public Intersection(float distance, Primitive nearestPrimetive)
        {
            this.distance = distance;
            this.nearestPrimetive = nearestPrimetive;
            //this.normal = normal;
        }

        public void calculateNormal()
        {
            if (nearestPrimetive == null)
                return;

            if (nearestPrimetive.GetType() == typeof(Sphere))
                normal = (point - nearestPrimetive.position).Normalized();
            else
                normal = nearestPrimetive.normal;
        }

        public void calculateNormal()
        {
            normal = (nearestPrimetive.position - point);
            normal.Normalize();
        }
    }
}

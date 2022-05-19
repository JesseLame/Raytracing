using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    class Ray
    {
        Vector3 direction;
        Vector3 origin;
        float distance;

        public Ray(Vector3 direction, Vector3 origin, float distance)
        {
            this.direction = direction;
            this.origin = origin;
            this.distance = distance;
        }
    }
}

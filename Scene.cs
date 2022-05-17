using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOGR2022Template
{
    class Scene
    {
        List<Primitive> primitives;
        List<Light> lights;

        public Scene()
        {

        }

        Intersection Intersect()
        {
            foreach ( Primitive p in primitives )
            {
                //dichtsbijzijnde intersection
                return;
            }
        }
    }
}

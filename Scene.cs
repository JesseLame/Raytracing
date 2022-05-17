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
            primitives = new List<Primitive>();
            lights = new List<Light>();
        }
    }
}

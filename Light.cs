using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOGR2022Template
{

    class Light 
    {
        int locx, locy, locz;
        float red, green, blue;

        public Light(int x, int y, int z, float r, float g, float b)
        {
            locx = x;
            locy = y;
            locz = z;
            red = r;
            green = g;
            blue = b;
        }

    }
    

}

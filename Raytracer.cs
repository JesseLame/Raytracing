using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOGR2022Template
{
    class Raytracer
    {
        static Scene scene;
        static Camera camera;
        //static Surface display;

        public Raytracer ()
        {

        }

        void Render()
        {
           // which uses the camera to loop over the pixels of the screen plane and to generate a ray for each pixel,
           // which is then used to find the nearest intersection.The result is then visualized by plotting a pixel.
           // For the middle row of pixels(typically line 256 for a 512x512 window), it generates debug output by visualizing every Nth ray(where N is e.g. 10).
        }
    }
}

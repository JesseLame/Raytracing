using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace INFOGR2022Template
{
    class Camera
    {
        Vector3 position;
        Vector3 lookDirection;
        Vector3 upDirection;
        Vector3 plane1, plane2, plane3, plane4;

        public Camera()
        {
            position = new Vector3(0, 0, 0);
            lookDirection = new Vector3(0, 1, 0);
            upDirection = new Vector3(0, 1, 0);
            

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace Template
{
    class Camera
    {
        public Surface screen;

        public Vector3 position;
        Vector3 lookDirection;
        Vector3 upDirection;
        public Vector3 plane1, plane2, plane3, plane4;

        Vector3 planeVec;

        float distToCamera = 1.5f;

        public Camera(Vector3 pos, Vector3 look, Vector3 up, Surface s)
        {
            position = pos;
            lookDirection = look.Normalized();
            upDirection = up.Normalized();
            screen = s;

            SetScreenPlane();
        }

        public void SetScreenPlane()
        {
            planeVec = Vector3.Cross(lookDirection, upDirection);

            Vector3 planeCenter = position + (lookDirection * distToCamera);

            //Left uppercorner
            plane1 = planeCenter + planeVec + upDirection;
            plane1 = plane1 * 100;

            //Right uppercorner
            plane2 = planeCenter - planeVec + upDirection;
            plane2 = plane2 * 100;

            //Right downcorner
            plane3 = planeCenter - planeVec - upDirection;
            plane3 = plane3 * 100;

            //Left down corner
            plane4 = planeCenter + planeVec - upDirection;
            plane4 = plane4 * 100;
        }

        public Ray Ray(int x, int y)
        {
            Vector3 u = plane1 - plane2;
            Vector3 v = plane4 - plane1;

            float a = x / screen.width;
            float b = y / screen.height;

            Vector3 p = plane1 + (a * u) + (b * v);

            Vector3 dir = p - position;

            return new Ray(dir, position, 1f);
        }
    }
}

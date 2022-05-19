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
        Vector3 position;
        Vector3 lookDirection;
        Vector3 upDirection;
        Vector3 plane1, plane2, plane3, plane4;

        Vector3 planeVec;

        float distToCamera = 1.5f;

        public Camera(Vector3 pos, Vector3 look, Vector3 up)
        {
            position = pos.Normalized();
            lookDirection = look.Normalized();
            upDirection = up.Normalized();

            SetScreenPlane();
        }

        public void SetScreenPlane()
        {
            planeVec = Vector3.Cross(lookDirection, upDirection);

            Vector3 planeCenter = position + (lookDirection * distToCamera);

            //Left uppercorner
            plane1 = planeCenter + planeVec + upDirection;

            //Right uppercorner
            plane2 = planeCenter - planeVec + upDirection;

            //Right downcorner
            plane3 = planeCenter - planeVec - upDirection;

            //Left down corner
            plane4 = planeCenter + planeVec - upDirection;                
        }

        public Ray Ray(int x, int y)
        {
            Vector3 u = plane1 - plane2;
            Vector3 v = plane4 - plane1;

            float a = x; // / WIDTH;
            float b = y; // / HEIGHT;

            Vector3 dir = plane1 + (a * u) + (b * v);

            return new Ray(dir, position, 1f);
        }
    }
}

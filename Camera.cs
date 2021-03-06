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
        public Vector3 lookDirection;
        public Vector3 upDirection;
        public Vector3 plane1, plane2, plane3, plane4;

        Vector3 planeVec;

        public float distToCamera = 1.5f;
        public float viewDegree = 60f;

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
            plane1 = planeCenter + planeVec * degreeToRad(viewDegree) + upDirection;

            //Right uppercorner
            plane2 = planeCenter - planeVec * degreeToRad(viewDegree) + upDirection;

            //Right downcorner
            plane3 = planeCenter - planeVec * degreeToRad(viewDegree) - upDirection;

            //Left down corner
            plane4 = planeCenter + planeVec * degreeToRad(viewDegree) - upDirection;
        }

        public float degreeToRad(float degree)
        {
            return degree * ((float)Math.PI / 180);
        }
       
        //Shoot ray through screen
        public Ray Ray(int x, int y)
        {
            Vector3 u = plane2 - plane1;
            Vector3 v = plane3 - plane2;

            float a = (float)x / (float)screen.width;
            float b = (float)y / (float)screen.height;

            Vector3 p = plane1 + (a * u) + (b * v);

            Vector3 dir = p - position;

            return new Ray(dir, position, 1f);
        }
    }
}

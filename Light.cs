using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Template
{

    public class Light 
    {
        public Vector3 position;
        public Vector3 intensity;

        public Light(Vector3 position, Vector3 intensity)
        {
            this.position = position;
            this.intensity = intensity;
        }

        public void drawDebug()
        {
            GL.Begin(PrimitiveType.Points);
            GL.Color3(intensity);
            for (int i = 0; i < 360; i++)
            {
                GL.Vertex2(position.Xz);
            }
            GL.End();

            //GL.Begin(PrimitiveType.LineLoop);
            //GL.Color3(1,1,0);
            //for (int i = 0; i < 360; i++)
            //{
            //    double a = position.X + 0.1f * Math.Cos(i);
            //    double b = position.Z + 0.1f * Math.Sin(i);
            //    Vector2 v = new Vector2();
            //    v.X = (float)a - position.X;
            //    v.Y = (float)b - position.Z;

            //    Vector2 u = v.PerpendicularRight;
            //    u.Normalize();
            //    GL.Vertex2(a, b);
            //}
            //GL.End();
        }
    }
    

}

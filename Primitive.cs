using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Template
{
    public abstract class Primitive
    {
        public Material material;
        public Vector3 position;
        public Vector3 normal;

        public Primitive(Material material)
        {
            this.material = material;
        }

        public abstract float intersects(Ray ray);

        public abstract void drawDebug(Surface screen);

    }

    public class Sphere : Primitive
    {
        float radius;

        public Sphere(Vector3 position, float radius, Material material) : base(material)
        {
            this.radius = radius;
            this.position = position;
        }

        public override void drawDebug(Surface screen)
        {
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(material.color);
            for (int i = 0; i < 360; i++)
            {
                double a = position.X + radius * Math.Cos(i);
                double b = position.Z + radius * Math.Sin(i);
                Vector2 v = new Vector2();
                v.X = (float)a - position.X;
                v.Y = (float)b - position.Z;

                Vector2 u = v.PerpendicularRight;
                u.Normalize();
                GL.Vertex2(a , b);
            }
            GL.End();
        }

        public override float intersects(Ray ray)
        {
            Vector3 c = position - ray.origin;
            float t = Vector3.Dot(c, ray.direction);
            Vector3 tVec = ray.origin + t * ray.direction;

            float y = (float)Math.Sqrt(Math.Pow(tVec.X - position.X, 2) + Math.Pow(tVec.Y - position.Y, 2));
            float x = (float)Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(y, 2));

            float t1 = Vector3.Dot(position - ray.origin, ray.direction) - x;
            float t2 = Vector3.Dot(position - ray.origin, ray.direction) + x;

            if (t1 < Int32.MaxValue && t1 >= 0)
                return t1;
            /*else if (t1 < 0 && t2 > 0)
                return t2;*/

            return Int32.MaxValue;
        }
    }

    

    class Plane : Primitive
    {
        float distance;

        public Plane(Vector3 normal, Vector3 position, Material material) : base( material)
        {
            this.normal = normal.Normalized();
            this.position = position;
            this.distance = Vector3.Dot(normal, position);
        }

        public override void drawDebug(Surface screen)
        {
            
        }

        public override float intersects(Ray ray)
        {
            //Kinda woring
            float denominator = Vector3.Dot(ray.direction, normal);

            float t = (distance  - Vector3.Dot(ray.origin, normal)) / denominator;

            if(t > 0)
                return t;
            
            return Int32.MaxValue;
        }
    }
}

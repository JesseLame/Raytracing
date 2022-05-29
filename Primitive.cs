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
        //Vector3 position;
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
            //Vector3 t = ray.origin + Vector3.Dot(position - ray.origin, ray.direction) * ray.direction;
            //float y = (float)Math.Sqrt(Math.Pow(t.X - position.X, 2) + Math.Pow(t.Y - position.Y, 2) + Math.Pow(t.Z - position.Z, 2));
            //float x = (float)Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(y, 2));

            //float t1 = Vector3.Dot(position - ray.origin, ray.direction) - x;
            //float t2 = Vector3.Dot(position - ray.origin, ray.direction) + x;


            //float delta = (float)(Math.Pow(Vector3.Dot(ray.direction, ray.origin - position), 2) - (Math.Pow(Math.Sqrt(Math.Pow((position.X - ray.origin.X), 2) + Math.Pow(position.Y - ray.origin.Y, 2)),2) - Math.Pow(radius, 2)));
            //float t = -(float)((Vector3.Dot(ray.direction, (ray.origin - position)) + Math.Sqrt(delta)));


            Vector3 c = position - ray.origin;
            float t = Vector3.Dot(c, ray.direction);
            Vector3 tVec = ray.origin + t * ray.direction;

            //Vector3 yVec = tVec - position;

            float y = (float)Math.Sqrt(Math.Pow(tVec.X - position.X, 2) + Math.Pow(tVec.Y - position.Y, 2));
            float x = (float)Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(y, 2));

            float t1 = Vector3.Dot(position - ray.origin, ray.direction) - x;
            float t2 = Vector3.Dot(position - ray.origin, ray.direction) + x;

            if (t1 < Int32.MaxValue)
                return t1;

            return Int32.MaxValue;
        }
    }

    

    class Plane : Primitive
    {
        Vector3 plane1, plane2, plane3, plane4;
        Vector3 side1, side2;

        public Plane(Vector3 normal, Vector3 side1, Vector3 side2, Vector3 position, Material material) : base( material)
        {
            this.normal = normal.Normalized();
            this.position = position;
            this.side1 = side1;
            this.side2 = side2;
        }

        public override void drawDebug(Surface screen)
        {
            
        }

        public override float intersects(Ray ray)
        {
            //Kinda woring
            //float denominator = Vector3.Dot(ray.direction, -normal);

            //if (denominator > 0.0001f)
            //{
            //    float t = Vector3.Dot(position - ray.origin, normal) / denominator;

            //    Vector3 p = ray.origin + ray.direction * t;

            //    return p.Length;
            //}
            //else
            //{
            //    return Int32.MaxValue;
            //}

            float dotproduct = Vector3.Dot(ray.direction, -normal);

            if (dotproduct > 0.0001f)
            {
                return Vector3.Dot(position - ray.origin, -normal) / dotproduct;
            }
            return int.MaxValue;


            //if (denominator > 0)
            //{
            //    float a = Vector3.Dot((position - ray.origin), normal) / denominator;
            //    Vector3 p = ray.origin + a * ray.direction;


            //}
        }
    }
}

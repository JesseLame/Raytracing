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
        public Vector3 color;

        public Primitive(Vector3 c)
        {
            color = c;
        }

        public abstract float intersects(Ray ray);

        public abstract void drawDebug(Surface screen);

    }

    class Sphere : Primitive
    {
        Vector3 position;
        float radius;

        public Sphere(Vector3 position, float radius, Vector3 color) : base(color)
        {
            this.radius = radius;
            this.position = position;
        }

        public override void drawDebug(Surface screen)
        {
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(color);
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

        public int MixColor(int red, int green, int blue)
        {
            return (red << 16) + (green << 8) + blue;
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

            //Vector3 q = c - t * ray.direction;
            //float p2 = q.LengthSquared;
            //if (p2 > radius)
            //    return Int32.MaxValue;

            //float rt = (float)Math.Sqrt(Math.Pow(c.Length, 2) - Math.Pow(q.Length, 2));
            //t -= (float)Math.Sqrt(Math.Pow(radius, 2) - p2);
            //if ((t < rt) && t > 0)
            //    return t;

            //return Int32.MaxValue;


        }
    }

    

    class Plane : Primitive
    {
        float distance;
        Vector3 normal;
        Vector3 pointOnPlane;

        public Plane(Vector3 normal, float distance, Vector3 color) : base(color)
        {
            this.distance = distance;
            this.normal = normal.Normalized();
            //(-AD, -BD, -CD) point on plane where normal vector = (A,B,C) and D = distance
            pointOnPlane = new Vector3(-(this.normal.X * distance), -(this.normal.Y * distance), -(this.normal.Z * distance));
        }

        public override void drawDebug(Surface screen)
        {
            throw new NotImplementedException();
        }

        public override float intersects(Ray ray)
        {
            //        p0, p1: Define the line.
            //        p_co, p_no: define the plane:
            //        p_co Is a point on the plane(plane coordinate).
            //        p_no Is a normal vector defining the plane direction;
            //        (does not need to be normalized).

            //# point-normal plane
            //              def isect_line_plane_v3(p0, p1, p_co, p_no, epsilon= 1e-6):
            //    u = p1 - p0
            //    dot = p_no * u
            //    if abs(dot) > epsilon:
            //        w = p0 - p_co
            //        fac = -(plane * w) / dot
            //        return p0 + (u * fac)

            //    return None
            float epsilon = 1e-6f;
            float dot = Vector3.Dot(normal, ray.direction);
            if (Math.Abs(dot) > epsilon)
            {
                Vector3 w = ray.origin - pointOnPlane;
                Vector3 fac = -(normal * w) / dot;
                Vector3 intersection = ray.origin + (ray.direction * fac);
                return (float)Math.Sqrt(Math.Pow(intersection.X - ray.origin.X, 2) + Math.Pow(intersection.Y - ray.origin.Y, 2) + Math.Pow(intersection.Z - ray.origin.Z, 2));
            } else
            {
                return Int32.MaxValue;
            }
        }
    }
}

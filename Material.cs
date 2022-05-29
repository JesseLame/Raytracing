using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Template
{
    public abstract class Material
    {
        public Vector3 color;
        public Vector3 ambientCoefficient;
        public Vector3 ambientLight;

        public Material(Vector3 color)
        {
            this.color = color;
            ambientLight = new Vector3(0.2f, 0.2f, 0.2f);
            //RGB val never lower then 0
            ambientCoefficient = new Vector3(Math.Max(0, color.X - 0.2f), Math.Max(0, color.Y - 0.2f), Math.Max(0, color.Z - 0.2f));
        }

        public Vector3 calcLight(Vector3 i, Intersection inter, Scene scene, Vector3 cameraOrigin)
        {
            float distanceLight;
            Vector3 l;
            Vector3 retColor = new Vector3(0, 0, 0);

            foreach (Light light in scene.lights)
            {
                Vector3 illumination = Vector3.Zero;
                l = light.position - i;
                distanceLight = l.Length;
                l.Normalize();

                Intersection obstacles = scene.ClosestIntersection(new Ray(l, i, distanceLight));

                //Pixel has a light source
                //Calculate the light intensity
                if (!shadow(obstacles.distance, distanceLight))
                {
                    Vector3 nearistPColor = inter.nearestPrimetive.material.color;

                    illumination.X = light.intensity.X * nearistPColor.X;
                    illumination.Y = light.intensity.Y * nearistPColor.Y;
                    illumination.Z = light.intensity.Z * nearistPColor.Z;

                    float dot = Vector3.Dot(inter.normal, l);
                    illumination = (1 / distanceLight) * illumination * Math.Max(0, dot);

                    illumination += this.colorCalc(illumination, distanceLight, light, inter, l, cameraOrigin);
                    
                }

                //Add for every light to the return color
                retColor += illumination;
            }

            retColor.X += ambientLight.X * ambientCoefficient.X;
            retColor.Y += ambientLight.Y * ambientCoefficient.Y;
            retColor.Z += ambientLight.Z * ambientCoefficient.Z;

            return retColor;
        }

        public abstract Vector3 colorCalc(Vector3 retColor, float distanceLight, Light light, Intersection inter, Vector3 l, Vector3 origin);

        public bool shadow(float obstacleDis, float lightDis)
        {
            return ((obstacleDis < lightDis - 1f) && (1f < obstacleDis));
        }
    }

    public class Glossy : Material
    {
        Vector3 specularCoefficient;
        float exponent;
        

        public Glossy(Vector3 specularCoefficient, Vector3 color, float exponent) : base(color)
        {
            this.specularCoefficient = specularCoefficient;
            this.exponent = exponent;
        }

        public override Vector3 colorCalc(Vector3 color, float distanceLight, Light light, Intersection inter, Vector3 l, Vector3 origin)
        {
            float dotLN = Vector3.Dot(-l, -inter.normal);
            Vector3 r = -l - 2 * dotLN * -inter.normal;
            Vector3 v = origin-inter.point;

            float dotVR = Vector3.Dot(v.Normalized(), r.Normalized());

            color.X = light.intensity.X * specularCoefficient.X;
            color.Y = light.intensity.Y * specularCoefficient.Y;
            color.Z = light.intensity.Z * specularCoefficient.Z;

            color = (1 / distanceLight) * color * (float)Math.Pow(Math.Max(0, dotVR),exponent);

            return color;
        }
    }

    public class Diffuse : Material
    {
        public Diffuse(Vector3 color) : base(color)
        {
        }

        public override Vector3 colorCalc(Vector3 retColor, float distanceLight, Light light, Intersection inter, Vector3 l, Vector3 origin)
        {
            return retColor;                
        }
    }
}

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
        public float reflaction;
        public bool isMirror;
        public bool hasPattern;

        public Material(Vector3 color)
        {
            this.color = color;
            ambientLight = new Vector3(0.2f, 0.2f, 0.2f);
            //RGB val never lower then 0
            ambientCoefficient = new Vector3(Math.Max(0, color.X - 0.2f), Math.Max(0, color.Y - 0.2f), Math.Max(0, color.Z - 0.2f));
        }

        //Color calculation that is done for every material
        public Vector3 calcLight(Vector3 i, Intersection inter, Scene scene, Vector3 cameraOrigin)
        {
            float distanceLight;
            Vector3 l;
            //We start with black
            Vector3 retColor = new Vector3(0, 0, 0);

            //For every light calc the color and add it to the return color
            foreach (Light light in scene.lights)
            {
                Vector3 illumination = Vector3.Zero;
                l = light.position - i;
                distanceLight = l.Length;
                l.Normalize();

                //Shoot a ray from intersection the the light source and check if there are any obstacles
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
                    //Calc the illumination done for all materials
                    illumination = (1 / distanceLight) * illumination * Math.Max(0, dot);

                    //Special color calc is added colorcalc is different for every material
                    illumination += this.colorCalc(illumination, distanceLight, light, inter, l, cameraOrigin);

                }

                //Add for every light to the return color
                retColor += illumination;
            }

            //Add the ambient light
            retColor.X += ambientLight.X * ambientCoefficient.X;
            retColor.Y += ambientLight.Y * ambientCoefficient.Y;
            retColor.Z += ambientLight.Z * ambientCoefficient.Z;

            //Return color
            return retColor;
        }

        //Color calc for every material
        public abstract Vector3 colorCalc(Vector3 retColor, float distanceLight, Light light, Intersection inter, Vector3 l, Vector3 origin);

        //Return true if there should be a shadow
        public bool shadow(float obstacleDis, float lightDis)
        {
            return ((obstacleDis < lightDis));
        }

        //Pattern creator
        public Vector3 pattern(Vector3 retColor, Intersection inter, Vector3 origin)
        {
            if(inter.nearestPrimetive.GetType() == typeof(Plane))
            {

                float distance = (inter.point - origin).Length;
                if ((int)distance % 2 == 0)
                {
                    return Vector3.Zero;
                }
            } 
            else
            {
                Vector3 p = inter.point - inter.nearestPrimetive.position;
                Vector3 normalUp = new Vector3(0, 1, 0);
                float dot = Vector3.Dot(normalUp, p);
                if ((int)(dot *10) % 2 == 0)
                {
                    return Vector3.Zero;
                }
            }
            return retColor;
        }
    }

    public class Glossy : Material
    {
        float specularCoefficient;
        float exponent;
        

        public Glossy(float specularCoefficient, Vector3 color, float exponent, bool hasPattern) : base(color)
        {
            this.specularCoefficient = specularCoefficient;
            this.exponent = exponent;
            this.isMirror = false;
            this.hasPattern = hasPattern;
        }

        public override Vector3 colorCalc(Vector3 color, float distanceLight, Light light, Intersection inter, Vector3 l, Vector3 origin)
        {
            float dotLN = Vector3.Dot(-l, -inter.normal);
            Vector3 r = -l - 2 * dotLN * -inter.normal;
            Vector3 v = origin-inter.point;

            float dotVR = Vector3.Dot(v.Normalized(), r.Normalized());

            color = light.intensity * specularCoefficient;

            if (hasPattern)
                color = pattern(color, inter, origin);

            color = (1 / distanceLight) * color * (float)Math.Pow(Math.Max(0, dotVR),exponent);

            return color;
        }
    }

    public class Mirror : Material
    {

        public Mirror(Vector3 color, float reflaction) : base(color)
        {
            this.reflaction = reflaction;
            this.isMirror = true;
            this.hasPattern = false;
        }

        //Mirror doesnt need extra color calc and never has a pattern
        public override Vector3 colorCalc(Vector3 color, float distanceLight, Light light, Intersection inter, Vector3 l, Vector3 origin)
        {
            return color;
        }
    }

    public class Diffuse : Material
    {
        public Diffuse(Vector3 color, bool hasPattern) : base(color)
        {
            this.isMirror = false;
            this.hasPattern = hasPattern;
        }

        //Diffuse o ly checks if a pattern should be made
        public override Vector3 colorCalc(Vector3 retColor, float distanceLight, Light light, Intersection inter, Vector3 l, Vector3 origin)
        {
            if (hasPattern)
                retColor = pattern(retColor, inter, origin);

            return retColor;                
        }
    }

}

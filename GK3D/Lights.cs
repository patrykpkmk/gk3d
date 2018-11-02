using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GK3D
{
    public class Lights
    {
        public Vector3 DirectionalLightDirection { get; set; }
        public Vector3 SpotlightOneLightPosition { get; set; }
        public Vector3 SpotlightOneSpotDirection { get; set; }

        public Vector3 SpotlightTwoLightPosition { get; set; }
        public Vector3 SpotlightTwoSpotDirection { get; set; }
        public Vector4 DirectionalLightAmbientColor { get; set; }
        public Vector4 DirectionalLightDiffuseColor { get; set; }
        public Vector4 DirectionalLightSpecularColor { get; set; }


        public Lights()
        {
            DirectionalLightDirection = new Vector3(0f, 0f, 1f);
            DirectionalLightDirection.Normalize();

            SpotlightOneLightPosition = new Vector3(0f, 25f, -25f);
            SpotlightOneSpotDirection = new Vector3(0f, -1f, 2f);

            SpotlightTwoLightPosition = new Vector3(-20f, 0f, 0);
            SpotlightTwoSpotDirection = new Vector3(1f, 0f, 0);

            DirectionalLightAmbientColor = Color.White.ToVector4();
            DirectionalLightDiffuseColor = Color.White.ToVector4();
            DirectionalLightSpecularColor = Color.White.ToVector4();
        }
    }
}
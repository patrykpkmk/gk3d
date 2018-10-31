using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GK3D
{
    public class HalfCylinder
    {
        VertexPositionColor[] vertices; //later, I will provide another example with VertexPositionNormalTexture

        short[] indices; //my laptop can only afford Reach, no HiDef :(
        float radius;
        private float height;
        int nvertices, nindices;
        BasicEffect effect;
        GraphicsDevice graphics;

        int m = 90;
        private double angle;


        public HalfCylinder(float rad, float h, GraphicsDevice graphics)
        {
            height = h;
            this.graphics = graphics;
            nvertices = 2 * m + 2;
            //traingles * 3
            nindices = 3 * (2 * m+2 + 2 * (m - 1));
            radius = rad;
            angle = Math.PI / (m - 1);
            effect = new BasicEffect(this.graphics);

            CreateVertices();
            CreateIndices();
            effect.VertexColorEnabled = true;
        }

        private void CreateVertices()
        {
            vertices = new VertexPositionColor[nvertices];
            Vector3 center = new Vector3(0, 0, 0);
            int i = 0;
            for (double alfa = 0; alfa <= Math.PI; alfa += angle)
            {
                float x = (float) (center.X + radius * Math.Cos(alfa));
                float y = (float) (center.Y - radius * Math.Sin(alfa));
                vertices[i++] = new VertexPositionColor(new Vector3(x, y, height), Color.Green);
                vertices[i++] = new VertexPositionColor(new Vector3(x, y, -height), Color.Green);
            }
            vertices[i++] = new VertexPositionColor(new Vector3(0, 0, height), Color.Green);
            vertices[i++] = new VertexPositionColor(new Vector3(0, 0, -height), Color.Green);
        }

        private void CreateIndices()
        {
            indices = new short[nindices];
            int i = 0;
            for (short v = 2; v <= 2 * m - 1; v++)
            {
                indices[i++] = (short) (v - 2);
                indices[i++] = (short) (v - 1);
                indices[i++] = v;
            }

            indices[i++] = (short)(nvertices - 2);
            indices[i++] = (short)(nvertices - 1);
            indices[i++] = 0;
            
            indices[i++] = (short)(nvertices - 1);
            indices[i++] = 0;
            indices[i++] = 1;

            indices[i++] = (short)(nvertices - 4);
            indices[i++] = (short)(nvertices - 3);
            indices[i++] = (short)(nvertices - 2);

            indices[i++] = (short)(nvertices - 3);
            indices[i++] = (short)(nvertices - 2);
            indices[i++] = (short)(nvertices - 1);



            //Top surface
            for (short v = 2; v < m*2; v+=2)
            {
                indices[i++] = (short)(v - 2);
                indices[i++] = v;
                indices[i++] = (short)(nvertices-2);
            }
            //Bottom surface
            for (short v = 3; v < m * 2; v += 2)
            {
                indices[i++] = (short)(v - 2);
                indices[i++] = v;
                indices[i++] = (short)(nvertices - 1);
            }
        }

        public void Draw(Matrix view, Matrix projection) // the camera class contains the View and Projection Matrices
        {
            effect.View = view;
            effect.Projection = projection;

            //TODO: Fill mode Solid or WireFrame
            graphics.RasterizerState = new RasterizerState() {FillMode = FillMode.WireFrame};

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0,
                    nvertices, indices, 0, indices.Length / 3);
            }
        }
    }
}
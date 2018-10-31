using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GK3D
{
    public class HalfSphere
    {
        VertexPositionColor[] vertices; //later, I will provide another example with VertexPositionNormalTexture

        short[] indices; //my laptop can only afford Reach, no HiDef :(
        float radius;
        int nvertices, nindices;
        BasicEffect effect;
        GraphicsDevice graphics;

        private int m =8;

        public HalfSphere(float Radius, GraphicsDevice graphics)
        {
            radius = Radius;
            this.graphics = graphics;
            effect = new BasicEffect(this.graphics);
            nvertices = m * m; // 90 vertices in a circle, 90 circles in a sphere
            nindices = m * m * 6;
            CreateSphereVertices();
            CreateIndices();
            effect.VertexColorEnabled = true;
        }

        private void CreateSphereVertices()
        {
            vertices = new VertexPositionColor[nvertices];
            Vector3 center = new Vector3(0, 0, 0);
            Vector3 rad = new Vector3((float)Math.Abs(radius), 0, 0);
            for (int x = 0; x < m; x++) //90 circles, difference between each is 4 degrees
            {
                float difx = 360.0f / m;
                for (int y = 0; y <= m/2; y++) //90 veritces, difference between each is 4 degrees 
                {
                    float dify = 360.0f / m;
                    Matrix zrot = Matrix.CreateRotationZ(MathHelper.ToRadians(y * dify)); //rotate vertex around z
                    Matrix yrot = Matrix.CreateRotationY(MathHelper.ToRadians(x * difx)); // rotate circle around y
                    Vector3 point = Vector3.Transform(Vector3.Transform(rad, zrot), yrot); //transformation

                    vertices[x + y * m] = new VertexPositionColor(point, Color.Green);
                }
            }
        }

        private void CreateIndices()
        {
            indices = new short[nindices];
            int i = 0;
            for (int x = 0; x < m; x++)
            {
                for (int y = 0; y < m; y++)
                {
                    int s1 = x == (m-1) ? 0 : x + 1;
                    int s2 = y == (m-1) ? 0 : y + 1;
                    short upperLeft = (short)(x * m + y);
                    short upperRight = (short)(s1 * m + y);
                    short lowerLeft = (short)(x * m + s2);
                    short lowerRight = (short)(s1 * m + s2);
                    indices[i++] = upperLeft;
                    indices[i++] = upperRight;
                    indices[i++] = lowerLeft;
                    indices[i++] = lowerLeft;
                    indices[i++] = upperRight;
                    indices[i++] = lowerRight;
                }
            }
        }

        public void Draw(Matrix view, Matrix projection) // the camera class contains the View and Projection Matrices
        {
            effect.View = view;
            effect.Projection = projection;

            //TODO: Fill mode Solid or WireFrame
            graphics.RasterizerState = new RasterizerState() { FillMode = FillMode.Solid };

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0,
                    nvertices, indices, 0, indices.Length / 3);
            }
        }
    }
}

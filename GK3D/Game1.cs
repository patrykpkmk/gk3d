using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GK3D
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Camera camera;
        private Matrix worldMatrix;
        private Matrix viewMatrix;
        private Matrix projectionMatrix;

        private TimeSpan frameTime;
        private DateTime lastFrameTimeUpdate;


        int planetoidSphereRadius;

        HalfCylinder halfCylinder;
        private Model robotModel;
        private Model rocketModel;
        private Model revolverModel;

        Sphere planetoidSphere;
        HalfSphere halfSphere;

        private Model appleModel;

        private Vector3 appleStelliteOneTransaltion;
        private Vector3 appleStelliteTwoTransaltion;


        private Effect phongEffect;
        private Effect phongEffectForModels;
        private Vector3 viewVector;
        private Matrix[] appleModelTransforms;
        private Matrix[] robotModelTransforms;
        private Matrix[] rocketModelTransforms;
        private Matrix[] revolverModelTransforms;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your drawing code here

            //CAMERA MATRIX
            camera = new Camera();

            viewMatrix = Matrix.CreateLookAt(
                camera.CameraPosition, camera.CameraTarget, camera.CameraUpVector);


            //WORLD MATRIX
            worldMatrix = Matrix.CreateWorld(camera.CameraTarget, Vector3.
                Forward, Vector3.Up);

            //PROJECTION MATRIX
            // We want the aspect ratio of our display to match
            // the entire screen's aspect ratio:
            float aspectRatio =
                graphics.PreferredBackBufferWidth / (float) graphics.PreferredBackBufferHeight;
            // Field of view measures how wide of a view our camera has.
            // Increasing this value means it has a wider view, making everything
            // on screen smaller. This is conceptually the same as "zooming out".
            // It also 
            float fieldOfView = MathHelper.ToRadians(45f);
            // Anything closer than this will not be drawn (will be clipped)
            float nearClipPlane = 1f;
            // Anything further than this will not be drawn (will be clipped)
            float farClipPlane = 1000f;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                fieldOfView, aspectRatio, nearClipPlane, farClipPlane);


            //basicEffect = new BasicEffect(graphics.GraphicsDevice);
            //basicEffect.Alpha = 1f;
            //this.basicEffect.VertexColorEnabled = true;
            //basicEffect.GraphicsDevice.Clear(Color.CornflowerBlue);

            planetoidSphereRadius = 8;
            planetoidSphere = new Sphere(planetoidSphereRadius);
            halfSphere = new HalfSphere(planetoidSphereRadius);
            halfCylinder = new HalfCylinder(3, 3);

            appleStelliteOneTransaltion = new Vector3(10, 10, 0);
            appleStelliteTwoTransaltion = new Vector3(-10, 0, 0);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here


            appleModel = Content.Load<Model>("apple");
            appleModelTransforms = new Matrix[appleModel.Bones.Count];
            appleModel.CopyAbsoluteBoneTransformsTo(appleModelTransforms);

            robotModel = Content.Load<Model>("robot");
            robotModelTransforms = new Matrix[robotModel.Bones.Count];
            robotModel.CopyAbsoluteBoneTransformsTo(robotModelTransforms);

            rocketModel = Content.Load<Model>("rocket");
            rocketModelTransforms = new Matrix[rocketModel.Bones.Count];
            rocketModel.CopyAbsoluteBoneTransformsTo(rocketModelTransforms);

            revolverModel = Content.Load<Model>("Revolver");
            revolverModelTransforms = new Matrix[revolverModel.Bones.Count];
            revolverModel.CopyAbsoluteBoneTransformsTo(revolverModelTransforms);

            phongEffect = Content.Load<Effect>("Phong");
            phongEffectForModels = Content.Load<Effect>("Phong_model");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            frameTime = DateTime.Now - lastFrameTimeUpdate;
            if (frameTime.TotalMilliseconds == 0) return;
            var keyboardState = Keyboard.GetState();

            camera.Update(keyboardState, frameTime);
            viewMatrix = Matrix.CreateLookAt(camera.CameraPosition, camera.CameraTarget,
                camera.CameraUpVector);

            lastFrameTimeUpdate = DateTime.Now;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            InitializePhongEffectForSphereGrid();
            //InitializePhongEffectForAppleModel();
           // DrawSphereWithEffect();
           // DrawHalfSphereWithEffect();
            DrawHalfCylinderWithEffect();

            //DrawAppleSatteliteOne();
            //DrawAppleSatteliteTwo();
            //DrawRocket();
            //DrawRevolver();


            //TODO: Drawing shapes
            //Draw planetoidSphere
            //planetoidSphere.Draw(basicEffect.View, basicEffect.Projection);
            //halfSphere.Draw(basicEffect.View, basicEffect.Projection);
            //halfCylinder.Draw(basicEffect.View, basicEffect.Projection);
            //DrawGround();


            base.Draw(gameTime);
        }

        private void InitializePhongEffectForSphereGrid()
        {
            phongEffect.GraphicsDevice.Clear(Color.CornflowerBlue);
            phongEffect.GraphicsDevice.RasterizerState = new RasterizerState() {FillMode = FillMode.Solid};
            viewVector = camera.CameraTarget - camera.CameraPosition;
            viewVector.Normalize();

            phongEffect.Parameters["World"].SetValue(worldMatrix);
            phongEffect.Parameters["View"].SetValue(viewMatrix);
            phongEffect.Parameters["Projection"].SetValue(projectionMatrix);

            phongEffect.Parameters["World"].SetValue(worldMatrix);
            phongEffect.Parameters["View"].SetValue(viewMatrix);
            phongEffect.Parameters["Projection"].SetValue(projectionMatrix);

            phongEffect.Parameters["AmbientColor"].SetValue(Color.White.ToVector4());
            phongEffect.Parameters["AmbientIntensity"].SetValue(0.02f);

            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(worldMatrix));
            phongEffect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);

            Vector3 diffLightDir = new Vector3(-1, -1, 100);
            diffLightDir.Normalize();
            phongEffect.Parameters["DiffuseLightDirection"].SetValue(diffLightDir);
            phongEffect.Parameters["DiffuseColor"].SetValue(Color.White.ToVector4());
            phongEffect.Parameters["DiffuseIntensity"].SetValue(0.75f);

            phongEffect.Parameters["Shininess"].SetValue(100f);
            phongEffect.Parameters["SpecularColor"].SetValue(Color.White.ToVector4());
            phongEffect.Parameters["SpecularIntensity"].SetValue(0.5f);

            phongEffect.Parameters["ViewVector"].SetValue(viewVector);
        }

        private void InitializePhongEffectForAppleModel()
        {
            phongEffectForModels.GraphicsDevice.Clear(Color.CornflowerBlue);
            phongEffectForModels.GraphicsDevice.RasterizerState = new RasterizerState() {FillMode = FillMode.WireFrame};
            viewVector = camera.CameraTarget - camera.CameraPosition;
            viewVector.Normalize();

            phongEffectForModels.Parameters["ModelColor"].SetValue(Color.Red.ToVector4());

            phongEffectForModels.Parameters["View"].SetValue(viewMatrix);
            phongEffectForModels.Parameters["Projection"].SetValue(projectionMatrix);

            phongEffectForModels.Parameters["AmbientColor"].SetValue(Color.White.ToVector4());
            phongEffectForModels.Parameters["AmbientIntensity"].SetValue(0.02f);

            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(worldMatrix));
            phongEffectForModels.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);

            Vector3 diffLightDir = new Vector3(-1, -1, 1);
            diffLightDir.Normalize();
            phongEffectForModels.Parameters["DiffuseLightDirection"].SetValue(diffLightDir);
            phongEffectForModels.Parameters["DiffuseColor"].SetValue(Color.White.ToVector4());
            phongEffectForModels.Parameters["DiffuseIntensity"].SetValue(0.75f);

            phongEffectForModels.Parameters["Shininess"].SetValue(100f);
            phongEffectForModels.Parameters["SpecularColor"].SetValue(Color.White.ToVector4());
            phongEffectForModels.Parameters["SpecularIntensity"].SetValue(0.5f);

            phongEffectForModels.Parameters["ViewVector"].SetValue(viewVector);
        }


        private void DrawSphereWithEffect()
        {
            foreach (EffectPass pass in phongEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                phongEffect.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalColor>(
                    PrimitiveType.TriangleList,
                    planetoidSphere.vertices, 0,
                    planetoidSphere.vertices.Length, planetoidSphere.indices, 0, planetoidSphere.indices.Length / 3);
            }
        }

        private void DrawHalfSphereWithEffect()
        {
            foreach (EffectPass pass in phongEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                phongEffect.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalColor>(
                    PrimitiveType.TriangleList,
                    halfSphere.vertices, 0,
                    halfSphere.vertices.Length, halfSphere.indices, 0, halfSphere.indices.Length / 3);
            }
        }

        private void DrawHalfCylinderWithEffect()
        {
            foreach (EffectPass pass in phongEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                

                phongEffect.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalColor>(
                    PrimitiveType.TriangleList,
                    halfCylinder.vertices.ToArray(), 0,
                    halfCylinder.vertices.Count, halfCylinder.indices.ToArray(), 0, halfCylinder.indices.Count / 3);
            }
        }

        private void DrawAppleSatteliteOne()
        {
            foreach (ModelMesh mesh in appleModel.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    phongEffectForModels.Parameters["World"].SetValue(appleModelTransforms[mesh.ParentBone.Index] *
                                                                      Matrix.CreateTranslation(
                                                                          appleStelliteOneTransaltion) * worldMatrix);
                    part.Effect = phongEffectForModels;
                }
                mesh.Draw();
            }
        }

        private void DrawAppleSatteliteTwo()
        {
            foreach (ModelMesh mesh in appleModel.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    phongEffectForModels.Parameters["World"].SetValue(appleModelTransforms[mesh.ParentBone.Index] *
                                                                      Matrix.CreateTranslation(
                                                                          appleStelliteTwoTransaltion) * worldMatrix);

                    part.Effect = phongEffectForModels;
                }
                mesh.Draw();
            }
        }

        private void DrawRocket()
        {
            foreach (ModelMesh mesh in rocketModel.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    phongEffectForModels.Parameters["ModelColor"].SetValue(Color.Yellow.ToVector4());
                    phongEffectForModels.Parameters["World"].SetValue(rocketModelTransforms[mesh.ParentBone.Index] *
                                                                      Matrix.CreateScale(new Vector3(0.02f, 0.02f, 0.02f)) *
                                                                      Matrix.CreateTranslation(
                                                                          new Vector3(-1f, 17f, -15f)));
                    part.Effect = phongEffectForModels;
                }
                mesh.Draw();
            }
        }

        private void DrawRevolver()
        {
            foreach (ModelMesh mesh in revolverModel.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    phongEffectForModels.Parameters["ModelColor"].SetValue(Color.OrangeRed.ToVector4());
                    phongEffectForModels.Parameters["World"].SetValue(revolverModelTransforms[mesh.ParentBone.Index] *
                                                                      Matrix.CreateScale(new Vector3(0.01f, 0.01f, 0.01f)) *
                                                                      Matrix.CreateRotationY(MathHelper.ToRadians(45)) *
                                                                      Matrix.CreateRotationZ(MathHelper.ToRadians(-5)) *
                                                                      Matrix.CreateTranslation(
                                                                          new Vector3(-20f, 0f, -25f)));
                    part.Effect = phongEffectForModels;
                }
                mesh.Draw();
            }
        }
    }
}
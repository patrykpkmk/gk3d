using System;
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
        BasicEffect basicEffect;


        //Camera
        Vector3 cameraTarget;
        Vector3 cameraPosition;
        private Vector3 cameraUpVector;
        private Matrix worldMatrix;
        private Matrix viewMatrix;
        private Matrix projectionMatrix;

        private Vector3 cameraForward;
        private float cameraSpeed = 0.1f;
        private TimeSpan frameTime;
        private DateTime lastFrameTimeUpdate;


        Sphere planetoidSphere;
        int planetoidSphereRadius;
        HalfSphere halfSphere;
        HalfCylinder halfCylinder;
        private Model revolverModel;
        private Model robotModel;

        private Model appleSatelliteModelOne;




        private Effect phongEffect;
        private Effect phongModelEffect;
        private Vector3 viewVector;

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
            cameraPosition = new Vector3(-10f, -10f, 15f);
            cameraTarget = Vector3.Zero;
            cameraUpVector = new Vector3(0, 1, 0);
            viewMatrix = Matrix.CreateLookAt(
                cameraPosition, cameraTarget, cameraUpVector);
            cameraForward = new Vector3(0, 0, 1);

            //WORLD MATRIX
            worldMatrix = Matrix.CreateWorld(cameraTarget, Vector3.
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


            basicEffect = new BasicEffect(graphics.GraphicsDevice);
            basicEffect.Alpha = 1f;
            this.basicEffect.VertexColorEnabled = true;

            planetoidSphereRadius = 5;
            planetoidSphere = new Sphere(planetoidSphereRadius, graphics.GraphicsDevice);
            halfSphere = new HalfSphere(planetoidSphereRadius, graphics.GraphicsDevice);
            halfCylinder = new HalfCylinder(5, 10, graphics.GraphicsDevice);


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

            revolverModel = Content.Load<Model>("Revolver");
            robotModel = Content.Load<Model>("robot");
            appleSatelliteModelOne = Content.Load<Model>("apple");
 
            phongEffect = Content.Load<Effect>("Phong");
            phongModelEffect = Content.Load<Effect>("Phong_model");
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

            //// TODO: Add your update logic here
            frameTime = DateTime.Now - lastFrameTimeUpdate;
            if (frameTime.TotalMilliseconds == 0) return;

            var keyboardState = Keyboard.GetState();
            var cameraMoveStep = (float) (cameraSpeed * frameTime.TotalMilliseconds);

            //Forward/Backward
            if (keyboardState.IsKeyDown(Keys.F))
            {
                cameraPosition -= cameraForward * cameraMoveStep;
            }
            if (keyboardState.IsKeyDown(Keys.B))
            {
                cameraPosition += cameraForward * cameraMoveStep;
            }

            //Left/Right
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                Vector3 perpendicularToForwardAndUp = Vector3.Cross(cameraUpVector, cameraForward);
                perpendicularToForwardAndUp.Normalize();
                cameraPosition -= perpendicularToForwardAndUp * cameraMoveStep;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                Vector3 perpendicularToForwardAndUp = Vector3.Cross(cameraUpVector, cameraForward);
                perpendicularToForwardAndUp.Normalize();
                cameraPosition += perpendicularToForwardAndUp * cameraMoveStep;
            }

            //Up/Down
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                cameraPosition += cameraUpVector * cameraMoveStep;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                cameraPosition -= cameraUpVector * cameraMoveStep;
            }

            //Look Up/Look Down (spogladanie gora i dol)
            if (keyboardState.IsKeyDown(Keys.U))
            {
                var perpendicularToUpAndForward = Vector3.Cross(cameraUpVector, cameraForward);
                perpendicularToUpAndForward.Normalize();
                cameraForward = Vector3.Transform(cameraForward,
                    Matrix.CreateFromAxisAngle(perpendicularToUpAndForward, MathHelper.ToRadians(2 * cameraMoveStep)));
                cameraUpVector = Vector3.Transform(cameraUpVector,
                    Matrix.CreateFromAxisAngle(perpendicularToUpAndForward, MathHelper.ToRadians(2 * cameraMoveStep)));
                cameraForward.Normalize();
                cameraUpVector.Normalize();
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                var perpendicularToUpAndForward = Vector3.Cross(cameraUpVector, cameraForward);
                perpendicularToUpAndForward.Normalize();
                cameraForward = Vector3.Transform(cameraForward,
                    Matrix.CreateFromAxisAngle(perpendicularToUpAndForward, MathHelper.ToRadians(-2 * cameraMoveStep)));
                cameraUpVector = Vector3.Transform(cameraUpVector,
                    Matrix.CreateFromAxisAngle(perpendicularToUpAndForward, MathHelper.ToRadians(-2 * cameraMoveStep)));
                cameraForward.Normalize();
                cameraUpVector.Normalize();
            }

            //Look Left/Look Right (rozlgladanie sie na boki)
            if (keyboardState.IsKeyDown(Keys.L))
            {
                cameraForward = Vector3.Transform(cameraForward,
                    Matrix.CreateFromAxisAngle(cameraUpVector, MathHelper.ToRadians(-2 * cameraMoveStep)));
                cameraForward.Normalize();
            }
            if (keyboardState.IsKeyDown(Keys.R))
            {
                cameraForward = Vector3.Transform(cameraForward,
                    Matrix.CreateFromAxisAngle(cameraUpVector, MathHelper.ToRadians(2 * cameraMoveStep)));
                cameraForward.Normalize();
            }

            //Rotate
            if (keyboardState.IsKeyDown(Keys.A))
            {
                cameraUpVector = Vector3.Transform(cameraUpVector,
                    Matrix.CreateFromAxisAngle(cameraForward, MathHelper.ToRadians(-2 * cameraMoveStep)));
                cameraUpVector.Normalize();
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                cameraUpVector = Vector3.Transform(cameraUpVector,
                    Matrix.CreateFromAxisAngle(cameraForward, MathHelper.ToRadians(2 * cameraMoveStep)));
                cameraUpVector.Normalize();
            }


            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget,
                cameraUpVector);


            lastFrameTimeUpdate = DateTime.Now;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            InitializePhongEffect();
            InitializePhongModelEffect();
            DrawSphereWithEffect();
            DrawAppleWithEffect();

            //TODO: Drawing shapes
            //Draw planetoidSphere
            //  planetoidSphere.Draw(basicEffect.View, basicEffect.Projection);
            //halfSphere.Draw(basicEffect.View, basicEffect.Projection);
            //halfCylinder.Draw(basicEffect.View, basicEffect.Projection);
            //DrawGround();

            // DrawRevolver();
            //DrawRobot();


            base.Draw(gameTime);
        }

        private void InitializePhongEffect()
        {
            phongEffect.GraphicsDevice.Clear(Color.CornflowerBlue);
            phongEffect.GraphicsDevice.RasterizerState = new RasterizerState() {FillMode = FillMode.Solid};
            viewVector = cameraTarget - cameraPosition;
            viewVector.Normalize();

            phongEffect.Parameters["World"].SetValue(worldMatrix);
            phongEffect.Parameters["View"].SetValue(viewMatrix);
            phongEffect.Parameters["Projection"].SetValue(projectionMatrix);

            phongEffect.Parameters["World"].SetValue(worldMatrix);
            phongEffect.Parameters["View"].SetValue(viewMatrix);
            phongEffect.Parameters["Projection"].SetValue(projectionMatrix);

            phongEffect.Parameters["AmbientColor"].SetValue(Color.White.ToVector4());
            phongEffect.Parameters["AmbientIntensity"].SetValue(0.01f);

            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(worldMatrix));
            phongEffect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);

            Vector3 diffLightDir = new Vector3(-1, -1, 1);
            diffLightDir.Normalize();
            phongEffect.Parameters["DiffuseLightDirection"].SetValue(diffLightDir);
            phongEffect.Parameters["DiffuseColor"].SetValue(Color.White.ToVector4());
            phongEffect.Parameters["DiffuseIntensity"].SetValue(0.75f);

            phongEffect.Parameters["Shininess"].SetValue(100f);
            phongEffect.Parameters["SpecularColor"].SetValue(Color.White.ToVector4());
            phongEffect.Parameters["SpecularIntensity"].SetValue(0.5f);

            phongEffect.Parameters["ViewVector"].SetValue(viewVector);
        }

        private Matrix[] transforms;
        private void InitializePhongModelEffect()
        {
            phongModelEffect.GraphicsDevice.Clear(Color.CornflowerBlue);
            phongModelEffect.GraphicsDevice.RasterizerState = new RasterizerState() {FillMode = FillMode.Solid};
            viewVector = cameraTarget - cameraPosition;
            viewVector.Normalize();

            phongModelEffect.Parameters["ModelColor"].SetValue(Color.Red.ToVector4());


            transforms = new Matrix[appleSatelliteModelOne.Bones.Count];
            appleSatelliteModelOne.CopyAbsoluteBoneTransformsTo(transforms);

            phongModelEffect.Parameters["View"].SetValue(viewMatrix);
            phongModelEffect.Parameters["Projection"].SetValue(projectionMatrix);

            phongModelEffect.Parameters["AmbientColor"].SetValue(Color.White.ToVector4());
            phongModelEffect.Parameters["AmbientIntensity"].SetValue(0.01f);

            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(worldMatrix));
            phongModelEffect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);

            Vector3 diffLightDir = new Vector3(-1, -1, 1);
            diffLightDir.Normalize();
            phongModelEffect.Parameters["DiffuseLightDirection"].SetValue(diffLightDir);
            phongModelEffect.Parameters["DiffuseColor"].SetValue(Color.White.ToVector4());
            phongModelEffect.Parameters["DiffuseIntensity"].SetValue(0.75f);

            phongModelEffect.Parameters["Shininess"].SetValue(100f);
            phongModelEffect.Parameters["SpecularColor"].SetValue(Color.White.ToVector4());
            phongModelEffect.Parameters["SpecularIntensity"].SetValue(0.5f);

            phongModelEffect.Parameters["ViewVector"].SetValue(viewVector);



        }

        private void DrawSphereWithEffect()
        {
            //basicEffect.World = worldMatrix;
            //basicEffect.View = viewMatrix;
            //basicEffect.Projection = projectionMatrix;

            //foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            //{
            //    pass.Apply();
            //    GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList,
            //        planetoidSphere.vertices, 0,
            //        planetoidSphere.nvertices, planetoidSphere.indices, 0, planetoidSphere.indices.Length / 3);
            //}


            //PHONG
            foreach (EffectPass pass in phongEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                phongEffect.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalColor>(
                    PrimitiveType.TriangleList,
                    planetoidSphere.vertices, 0,
                    planetoidSphere.nvertices, planetoidSphere.indices, 0, planetoidSphere.indices.Length / 3);
            }
        }

        private void DrawAppleWithEffect()
        { 

            foreach (ModelMesh mesh in appleSatelliteModelOne.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    phongModelEffect.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(new Vector3(10,10,0)));
                    part.Effect = phongModelEffect;
                }




                mesh.Draw();
            }
        }

        #region Models

        public void DrawRevolver()
        {
            // A model is composed of "Meshes" which are
            // parts of the model which can be positioned
            // independently, which can use different textures,
            // and which can have different rendering states
            // such as lighting applied.
            foreach (var mesh in revolverModel.Meshes)
            {
                // "Effect" refers to a shader. Each mesh may
                // have multiple shaders applied to it for more
                // advanced visuals. 
                foreach (BasicEffect effect in mesh.Effects)
                {
                    // We could set up custom lights, but this
                    // is the quickest way to get somethign on screen:
                    effect.EnableDefaultLighting();
                    // This makes lighting look more realistic on
                    // round surfaces, but at a slight performance cost:
                    effect.PreferPerPixelLighting = true;

                    // The world matrix can be used to position, rotate
                    // or resize (scale) the model. Identity means that
                    // the model is unrotated, drawn at the origin, and
                    // its size is unchanged from the loaded content file.
                    effect.World = Matrix.Identity;

                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                }

                // Now that we've assigned our properties on the effects we can
                // draw the entire mesh
                mesh.Draw();
            }
        }

        public void DrawRobot()
        {
            // A model is composed of "Meshes" which are
            // parts of the model which can be positioned
            // independently, which can use different textures,
            // and which can have different rendering states
            // such as lighting applied.
            foreach (var mesh in robotModel.Meshes)
            {
                // "Effect" refers to a shader. Each mesh may
                // have multiple shaders applied to it for more
                // advanced visuals. 
                foreach (BasicEffect effect in mesh.Effects)
                {
                    // We could set up custom lights, but this
                    // is the quickest way to get somethign on screen:
                    effect.EnableDefaultLighting();
                    // This makes lighting look more realistic on
                    // round surfaces, but at a slight performance cost:
                    effect.PreferPerPixelLighting = true;

                    // The world matrix can be used to position, rotate
                    // or resize (scale) the model. Identity means that
                    // the model is unrotated, drawn at the origin, and
                    // its size is unchanged from the loaded content file.
                    effect.World = Matrix.Identity;

                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                }

                // Now that we've assigned our properties on the effects we can
                // draw the entire mesh
                mesh.Draw();
            }
        }

        #endregion
    }
}
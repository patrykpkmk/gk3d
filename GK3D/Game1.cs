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
        private Matrix cameraMatrix;
        private Matrix projectionMatrix;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BasicEffect effect;
        Sphere planetoidSphere;
        int planetoidSphereRadius;
        HalfSphere halfSphere;
        HalfCylinder halfCylinder;

        private Model revolverModel;
        private Model robotModel;

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
            planetoidSphereRadius = 25;
            planetoidSphere = new Sphere(planetoidSphereRadius, graphics.GraphicsDevice);
            halfSphere = new HalfSphere(planetoidSphereRadius, graphics.GraphicsDevice);
            halfCylinder = new HalfCylinder(5, 10, graphics.GraphicsDevice);

            effect = new BasicEffect(graphics.GraphicsDevice);
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

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            var cameraPosition = new Vector3(50, 50, 50);
            var cameraLookAtVector = Vector3.Zero;
            var cameraUpVector = Vector3.UnitZ;
            cameraMatrix = Matrix.CreateLookAt(
                cameraPosition, cameraLookAtVector, cameraUpVector);;

            effect.View = cameraMatrix;


            // We want the aspect ratio of our display to match
            // the entire screen's aspect ratio:
            float aspectRatio =
                graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
            // Field of view measures how wide of a view our camera has.
            // Increasing this value means it has a wider view, making everything
            // on screen smaller. This is conceptually the same as "zooming out".
            // It also 
            float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
            // Anything closer than this will not be drawn (will be clipped)
            float nearClipPlane = 1;
            // Anything further than this will not be drawn (will be clipped)
            float farClipPlane = 200;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

            effect.Projection = projectionMatrix;

            //TODO: Drawing shapes
            //Draw planetoidSphere
            //planetoidSphere.Draw(effect.View, effect.Projection);
            //halfSphere.Draw(effect.View, effect.Projection);
            //halfCylinder.Draw(effect.View, effect.Projection);
            //DrawGround();

          // DrawRevolver();
           DrawRobot();
            
            


            base.Draw(gameTime);
        }


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

                    effect.View = cameraMatrix;
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

                    effect.View = cameraMatrix;
                    effect.Projection = projectionMatrix;
                }

                // Now that we've assigned our properties on the effects we can
                // draw the entire mesh
                mesh.Draw();
            }
        }

    }
}
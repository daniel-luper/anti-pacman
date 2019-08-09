using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AntiPacman.Gameplay;
using AntiPacman.Engine;
using AntiPacman.Engine.States;

namespace AntiPacman
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        // DEVELOPER FUNCTIONALITY
        public bool DevMode { get; } = false;
        private Util util = new Util();

        // Resources for drawing.
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Vector2 baseScreenSize = new Vector2(224, 248);
        private Matrix globalTransformation;
        private float scale;


        public bool FullScreen { get; private set; }
        public Vector2 CenterDrawLocation { get; private set; }
        public int ScreenHeight { get; private set; }
        public int ScreenWidth { get; private set; }

        // Gameplay.
        private StateManager stateManager;
        private Map map;
        private bool wasUsingMap;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;
            IsMouseVisible = false;
            FullScreen = true;
        }

        protected override void Initialize()
        {
            ScreenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            ScreenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

            // Work out how much we need to scale our graphics to fill the screen.
            scale = ScreenHeight / baseScreenSize.Y;
            Vector3 screenScalingFactor = new Vector3(scale, scale, 1);
            globalTransformation = Matrix.CreateScale(screenScalingFactor);

            CenterDrawLocation = new Vector2(ScreenWidth / scale / 2, 0.0f);

            // Game state management.
            stateManager = new StateManager(this);

            // Create the map at the center of the screen.
            map = new Map(this, stateManager);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            map.Load();
            stateManager.CurrentState().Load();
        }

        protected override void Update(GameTime gameTime)
        {
            stateManager.CurrentState().Update(gameTime);

            // Handle map updates
            if (stateManager.CurrentState().IsUsingMap)
            {
                wasUsingMap = true;
                map.Update(gameTime);
            }
            else if (wasUsingMap)
            {
                map.Reset();
                wasUsingMap = false;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, globalTransformation);

            if (stateManager.CurrentState().IsUsingMap)
                map.Draw(spriteBatch);
            stateManager.CurrentState().Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void FlipWindowMode()
        {
            if (FullScreen)
            {
                graphics.PreferredBackBufferWidth = (int)baseScreenSize.X * 4;
                graphics.PreferredBackBufferHeight = (int)baseScreenSize.Y * 4;
                IsMouseVisible = true;
                FullScreen = false;
            }
            else
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                IsMouseVisible = false;
                FullScreen = true;
            }
            graphics.ToggleFullScreen();

            // Update everything for new screen values
            ScreenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            ScreenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

            scale = ScreenHeight / baseScreenSize.Y;
            Vector3 screenScalingFactor = new Vector3(scale, scale, 1);
            globalTransformation = Matrix.CreateScale(screenScalingFactor);

            CenterDrawLocation = new Vector2(ScreenWidth / scale / 2, 0.0f);

            stateManager.Dispose();

            stateManager = new StateManager(this);
            stateManager.CurrentState().Initialize();
            stateManager.CurrentState().Load();
            stateManager.CurrentState().PostLoad();
            stateManager.Push(new OptionState(this, stateManager));

            map = new Map(this, stateManager);
            map.Load();
        }
    }
}

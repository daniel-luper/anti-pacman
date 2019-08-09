using AntiPacman.Engine.Animation;
using AntiPacman.Gameplay;
using AntiPacman.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace AntiPacman.Engine.States
{
    class IntroState : State
    {
        private Util util;
        private Clock timer;
        private const int wait = 250;
        private Vector2 startDrawLocation;

        // Pacman
        private Texture2D pacSheet;
        private TextureAnimation pacTextureAnimation;
        private AnimatedObject pacman;
        private Vector2 pacStartPosition;

        // Ghost
        private Texture2D ghostTexture;
        private TextureAnimation ghostTextureAnimation;
        private AnimatedObject ghost;
        private Vector2 ghostStartPosition;

        public IntroState(Game game, StateManager stateManager) : base(game, stateManager)
        {
            util = new Util();
            timer = new Clock();
            startDrawLocation = new Vector2(game.CenterDrawLocation.X - 112, 116);
        }

        public override void Load()
        {
            // Load pacman.
            pacSheet = game.Content.Load<Texture2D>(@"Textures/Game/pacman-sheet");
            pacTextureAnimation = new TextureAnimation(pacSheet, 16, 16, new TimeSpan(0, 0, 0, 0, 66));
            pacman = new AnimatedObject(pacTextureAnimation);
            pacStartPosition = new Vector2(startDrawLocation.X - 5, startDrawLocation.Y);

            // Load ghost.
            ghostTexture = game.Content.Load<Texture2D>(@"Textures/Game/ghost_01");
            ghostTextureAnimation = new TextureAnimation(ghostTexture, ghostTexture.Width, ghostTexture.Height, new TimeSpan(1, 0, 0));
            ghost = new AnimatedObject(ghostTextureAnimation);
            ghostStartPosition = new Vector2(startDrawLocation.X - 60, startDrawLocation.Y + 1);
        }

        public override void PostLoad()
        {
            // Animation scripting.
            pacman.AddAnimation(pacStartPosition, new Vector2(240, 0), 60);
            ghost.AddAnimation(ghostStartPosition, new Vector2(260, 0), 65);

            // Timeline:
            // 1. Visual glitch / reboot sound
            // 2. Closeup on pacman
            // 3. Pacman chases ghost from left to right with hype music
            // 4. Glitch sound and portion of map appears
            // 5. Ghost is chilling alone, eating dots, when pacman shows up
            // 6. Ghost eats energizer and everything goes back to classic pacman (except pacman doesnt eat the dots)
        }

        public override void Update(GameTime gameTime)
        {
            timer.Update(gameTime);
            pacman.Update(gameTime);
            ghost.Update(gameTime);

            // Move on to the actual game.
            if ((pacman.IsComplete() && ghost.IsComplete()) || 
                (Keyboard.GetState().IsKeyDown(Keys.Space) && timer.ElapsedTime >= wait))
            {
                stateManager.Pop();
                stateManager.Push(new PlayState(game, stateManager));
            }   
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw characters.
            pacman.Draw(spriteBatch, 3);
            ghost.Draw(spriteBatch, -1);

            // Fill rest of screen with black.
            if (startDrawLocation.X != 0)
            {
                util.DrawRectangle(new Rectangle(0, 0, (int)startDrawLocation.X, game.ScreenHeight),
                    Color.Black, spriteBatch, game);
                util.DrawRectangle(new Rectangle((int)startDrawLocation.X + 224, 0, (int)startDrawLocation.X * 2, game.ScreenHeight),
                    Color.Black, spriteBatch, game);
            }
        }     
    }
}

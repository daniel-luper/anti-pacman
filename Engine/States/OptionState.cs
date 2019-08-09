using AntiPacman.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AntiPacman.Engine.States
{
    enum OptionSelection
    {
        Fullscreen,
        Volume,
        Controls
    }

    public class OptionState : State
    {
        private Util util = new Util();
        private Clock timer = new Clock();
        private const int wait = 150;

        private Vector2 startDrawLocation;
        private Texture2D textureFullScreen;
        private Texture2D textureWindowed;
        private SoundEffect select;
        private SoundEffect back;
        private SoundEffect enter;

        private Rectangle dot;
        private Point dotLocation;
        private OptionSelection selection;

        public OptionState(Game game, StateManager stateManager) : base(game, stateManager)
        {
            dot = new Rectangle(new Point(-5, -5), new Point(5, 5));
            dotLocation = new Point(26, 78);
        }

        public override void Load()
        {
            textureFullScreen = game.Content.Load<Texture2D>("Textures/Options/options-yes");
            textureWindowed = game.Content.Load<Texture2D>("Textures/Options/options-no");
            startDrawLocation = new Vector2(game.CenterDrawLocation.X - textureFullScreen.Width / 2, game.CenterDrawLocation.Y);
            dotLocation.X += (int)startDrawLocation.X;

            // Load sound effects.
            select = game.Content.Load<SoundEffect>("Audio/UI/select");
            back = game.Content.Load<SoundEffect>("Audio/UI/back");
            enter = game.Content.Load<SoundEffect>("Audio/UI/enter");
        }

        public override void Initialize()
        {
            timer.Reset();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
            {
                back.Play();
                stateManager.Pop();
            }
            else if ((state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.Space)) && timer.ElapsedTime >= wait)
            {
                timer.Reset();
                if (selection == OptionSelection.Fullscreen)
                {
                    enter.Play();
                    game.FlipWindowMode();
                }
                else if (selection == OptionSelection.Volume)
                {
                    enter.Play();
                }
            }
            else if (state.IsKeyDown(Keys.Left) && timer.ElapsedTime >= wait)
            {
                timer.Reset();
                if (selection == OptionSelection.Fullscreen)
                {
                    enter.Play();
                    game.FlipWindowMode();
                }
                else if (selection == OptionSelection.Volume)
                {
                    enter.Play();
                }
            }

            if (state.IsKeyDown(Keys.Down) && timer.ElapsedTime >= wait)
            {
                select.Play();
                timer.Reset();
                if (selection == OptionSelection.Controls)
                {
                    dotLocation = new Point(dotLocation.X, dotLocation.Y - 30);
                    selection = selection - 2;
                }
                else
                {
                    dotLocation = new Point(dotLocation.X, dotLocation.Y + 15);
                    selection = selection + 1;
                }
            }
            if (state.IsKeyDown(Keys.Up) && timer.ElapsedTime >= wait)
            {
                select.Play();
                timer.Reset();
                if (selection == OptionSelection.Fullscreen)
                {
                    dotLocation = new Point(dotLocation.X, dotLocation.Y + 30);
                    selection = selection + 2;
                }
                else
                {
                    dotLocation = new Point(dotLocation.X, dotLocation.Y - 15);
                    selection = selection - 1;
                }
            }
            dot = new Rectangle(dotLocation, dot.Size);

            // Update timer.
            timer.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (game.FullScreen)
                spriteBatch.Draw(textureFullScreen, startDrawLocation, Color.White);
            else
                spriteBatch.Draw(textureWindowed, startDrawLocation, Color.White);
            util.DrawRectangle(dot, Color.White, spriteBatch, game);
        }
    }
}

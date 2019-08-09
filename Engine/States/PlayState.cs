using AntiPacman.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AntiPacman.Engine.States
{
    public class PlayState : State
    {
        private Clock timer;
        private const int wait = 150;

        public PlayState(Game game, StateManager stateManager) : base(game, stateManager)
        {
            timer = new Clock();
        }

        public override void Initialize()
        {
            IsUsingMap = true;
            timer.Reset();
        }

        public override void Load()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                stateManager.Pop();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AntiPacman.Engine
{
    public abstract class State
    {
        protected Game game;
        protected StateManager stateManager;

        public bool IsUsingMap { get; protected set; }

        public State(Game game, StateManager stateManager)
        {
            this.game = game;
            this.stateManager = stateManager;
        }

        public virtual void Initialize()
        {
            IsUsingMap = false;
        }

        public virtual void PostLoad()
        {
        }

        public abstract void Load();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        public virtual void Dispose()
        {
        }
    }
}

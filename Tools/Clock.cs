using Microsoft.Xna.Framework;

namespace AntiPacman.Tools
{
    public class Clock
    {
        public float ElapsedTime { get; private set; }

        public Clock()
        {
            ElapsedTime = 0;
        }

        public void Reset()
        {
            ElapsedTime = 0;
        }

        public void Update(GameTime gameTime)
        {
            ElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
    }
}

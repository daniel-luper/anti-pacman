using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace AntiPacman.Gameplay
{
    class ActionAnimation
    {
        private Util util = new Util();
        private TextureAnimation animatedTexture;

        // Vector stuff.
        private Vector2 translation;
        private float magnitude;
        private Vector2 direction;
        private Vector2 position;

        // Time stuff
        private float rate;
        private float deltaTime;
        private float timeRemaining;

        public bool IsComplete { get; private set; }

        /// <summary>
        /// Paramater "rate" is in pixels per second
        /// </summary>
        public ActionAnimation(TextureAnimation textureAnimation, Vector2 startPosition, Vector2 translation, float rate)
        {
            animatedTexture = textureAnimation;
            position = startPosition;
            this.translation = translation;
            magnitude = util.Distance(Vector2.Zero, translation);
            this.rate = rate;

            // Calculate the animation duration based on r = d/t formula.
            timeRemaining = magnitude / rate;

            // Normalize our translation vector into a unit vector to make it easier to use.
            direction = new Vector2(translation.X / magnitude, translation.Y / magnitude);

            IsComplete = false;
        }

        public void Update(GameTime gameTime)
        {
            // Manage time
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeRemaining -= deltaTime;
            if (timeRemaining <= 0)
                IsComplete = true;

            // Animate
            position = new Vector2(position.X + direction.X * deltaTime * rate, position.Y);
            if (animatedTexture != null)
                animatedTexture.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, int animationRow)
        {
            if (animationRow == -1)
                animatedTexture.Draw(spriteBatch, position);
            else
                animatedTexture.Draw(spriteBatch, position, animationRow);
        }
    }
}

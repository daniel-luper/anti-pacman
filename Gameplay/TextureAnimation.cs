using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AntiPacman.Gameplay
{
    public class TextureAnimation
    {
        private Texture2D spriteSheet;
        private int frameWidth;
        private int frameHeight;
        private int horizontalCount;
        private int verticalCount;
        private TimeSpan frameDuration;
        private TimeSpan timeToNext;
        private Point currentFrame;

        public TextureAnimation(Texture2D spriteSheet, int frameWidth, int frameHeight, TimeSpan frameDuration)
        {
            this.spriteSheet = spriteSheet;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            horizontalCount = spriteSheet.Width / frameWidth;
            verticalCount = spriteSheet.Height / frameHeight;
            this.frameDuration = frameDuration;
            timeToNext = frameDuration;
            currentFrame = Point.Zero;
        }

        public void Update(GameTime gameTime)
        {
            timeToNext -= gameTime.ElapsedGameTime;

            // Move to next frame.
            if (timeToNext <= TimeSpan.Zero)
            {
                // Change rows.
                if (currentFrame.X == horizontalCount - 1)
                {
                    if (currentFrame.Y == verticalCount - 1)
                        currentFrame.Y = 0;
                    else
                        currentFrame.Y++;
                    currentFrame.X = 0;
                }
                // Change columns.
                else
                {
                    currentFrame.X++;
                }
                timeToNext += frameDuration;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(spriteSheet, position, new Rectangle(
                currentFrame.X * frameWidth,
                currentFrame.Y * frameHeight,
                frameWidth,
                frameHeight), 
                Color.White);
        }

        // Draw a specific row
        public void Draw(SpriteBatch spriteBatch, Vector2 position, int row)
        {
            if (row == -1)
                currentFrame.X = 2;
            else
                currentFrame.Y = row;

            spriteBatch.Draw(spriteSheet, position, new Rectangle(
                currentFrame.X * frameWidth,
                currentFrame.Y * frameHeight,
                frameWidth,
                frameHeight),
                Color.White);
        }
    }
}

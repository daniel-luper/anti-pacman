using AntiPacman.Gameplay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace AntiPacman.Engine.Animation
{
    class AnimatedObject
    {
        private Util util = new Util();

        private TextureAnimation textureAnimation;
        private List<ActionAnimation> actions;

        public AnimatedObject(TextureAnimation textureAnimation)
        {
            this.textureAnimation = textureAnimation;
            actions = new List<ActionAnimation>
            {
                new ActionAnimation(null, Vector2.Zero, Vector2.Zero, 0)
            };
        }

        public void AddAnimation(Vector2 startPosition, Vector2 translation, float rate)
        {
            actions.Insert(1, new ActionAnimation(textureAnimation, startPosition, translation, rate));
        }

        public void Update(GameTime gameTime)
        {
            if (ActiveAction().IsComplete)
                actions.RemoveAt(actions.Count - 1);

            if (actions[0] != null)
                ActiveAction().Update(gameTime);
            else
                util.DevMsg("Out of actions");
        }

        /// <summary>
        /// Parameter "animationRow" can be set to -1 to draw every row.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, int animationRow)
        {
            ActiveAction().Draw(spriteBatch, animationRow);
        }

        public bool IsComplete()
        {
            if (actions.Count == 1)
                return true;
            else
                return false;
        }

        private ActionAnimation ActiveAction()
        {
            return actions[actions.Count - 1];
        }
    }
}

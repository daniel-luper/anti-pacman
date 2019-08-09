using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AntiPacman.Engine.States
{
    public class ScoreState : State
    {
        private Util util = new Util();

        private Texture2D texture;
        private Vector2 startDrawLocation;
        private List<String> lines;
        private const String textFilePath = @"Data/highscores.txt";
        private Color[] textColor = new Color[]
        {
            new Color(255, 0, 0),
            new Color(255, 184, 222),
            new Color(0, 255, 222),
            new Color(255, 184, 71),
            new Color(255, 255, 0)
        };

        private SoundEffect back;
        private SpriteFont font;

        public ScoreState(Game game, StateManager stateManager) : base(game, stateManager)
        {
            lines = new List<String>();
        }

        public override void Load()
        {
            // Load background texture
            texture = game.Content.Load<Texture2D>("Textures/Scoreboard/scoreboard");
            startDrawLocation = new Vector2(game.CenterDrawLocation.X - texture.Width / 2, game.CenterDrawLocation.Y);

            // Load scores
            lines = util.ReadFile(textFilePath);

            // Change then save scores here...
            util.WriteToFile(lines, textFilePath);

            // Load sound effect
            back = game.Content.Load<SoundEffect>("Audio/UI/back");

            // Load font
            font = game.Content.Load<SpriteFont>("Fonts/emulogic");
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                back.Play();
                stateManager.Pop();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, startDrawLocation, Color.White);
            for (int i = 0; i < lines.Count; i++)
            {
                spriteBatch.DrawString(font, lines[i], new Vector2(startDrawLocation.X + 26, startDrawLocation.Y + i*13 + 86), textColor[i % 5]);
            }
        }
    }
}

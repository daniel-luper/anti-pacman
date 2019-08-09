using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AntiPacman.Engine.States
{
    public class EndState : State
    {
        private Util util = new Util();
        private const float scale = 2.0f;
        private Matrix globalTransformation;
        private int screenWidth;
        private int screenHeight;
        private int borders;

        private SpriteFont font;
        private const String textFilePath = @"Data/bsod.txt";
        private List<String> lines = new List<String>();
        private int maxLineWidth;

        public EndState(Game game, StateManager stateManager) : base(game, stateManager)
        {
            // Work out transformation matrix
            Vector3 screenScalingFactor = new Vector3(scale, scale, 1);
            globalTransformation = Matrix.CreateScale(screenScalingFactor);

            screenWidth = game.ScreenWidth;
            screenHeight = game.ScreenHeight;
            borders = screenWidth / 50;
            maxLineWidth = screenWidth / 2 - borders;
        }

        public override void Load()
        {
            // Load font
            font = game.Content.Load<SpriteFont>(@"Fonts/fixedsys");

            // Load and process text.
            lines = util.ReadFile(textFilePath);
            lines = WrapText(lines);
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                stateManager.Pop();
                stateManager.CurrentState().Initialize();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Change draw settings
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, globalTransformation);

            // Fill BSoD background with blue.
            util.DrawRectangle(new Rectangle(0, 0, screenWidth, screenHeight), Color.Blue, spriteBatch, game);

            // Write text to screen.
            for (int i = 0; i < lines.Count; i++)
                spriteBatch.DrawString(font, lines[i], new Vector2(borders, i * 22 + borders), Color.White);
        }

        private List<String> WrapText(List<String> text)
        {
            List<String> wrappedText = new List<String>();
            foreach (String line in text)
            {
                if (font.MeasureString(line).X > maxLineWidth)
                {
                    string[] words = line.Split(' ');
                    List<StringBuilder> newLines = new List<StringBuilder>
                    {
                        new StringBuilder()
                    };
                    int num = 0;
                    float linewidth = 0f;
                    float spaceWidth = font.MeasureString(" ").X;
                    for (int i = 0; i < words.Length; i++)
                    {
                        Vector2 size = font.MeasureString(words[i]);
                        if (linewidth + size.X < maxLineWidth)
                        {
                            linewidth += size.X + spaceWidth;
                        }
                        else
                        {
                            num++;
                            newLines.Add(new StringBuilder());
                            linewidth = size.X + spaceWidth;
                        }
                        newLines[num].Append(words[i]);
                        newLines[num].Append(" ");
                    }
                    foreach (StringBuilder newLine in newLines)
                    {
                        wrappedText.Add(newLine.ToString());
                    }
                }
                else
                {
                    wrappedText.Add(line);
                }
            }
            return wrappedText;
        }
    }
}

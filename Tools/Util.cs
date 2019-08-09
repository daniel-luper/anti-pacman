using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AntiPacman
{
    public class Util
    {
        #region MATH
        public int NearestMultiple(int factor, float value)
        {
            int nearestMultiple = (int)Math.Round((value / (double)factor), MidpointRounding.AwayFromZero) * factor;
            Console.WriteLine(value + "'s nearest multiple of " + factor + " is " + nearestMultiple);
            return nearestMultiple;
        }

        public float Distance(float a, float b)
        {
            return Math.Abs(a - b);
        }

        public float Distance(Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }
        #endregion

        #region GRAPHICS
        public void DrawRectangle(Rectangle rectangle, Color color, SpriteBatch spriteBatch, Game game)
        {
            Texture2D rect = new Texture2D(game.GraphicsDevice, rectangle.Width, rectangle.Height);

            Color[] data = new Color[rect.Width * rect.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = color;
            rect.SetData(data);

            Vector2 coor = new Vector2(rectangle.Location.X, rectangle.Location.Y);
            spriteBatch.Draw(rect, coor, Color.White);
        }
        #endregion

        #region READ/WRITE
        public List<String> ReadFile(String path)
        {
            List<String> lines = new List<String>();
            using (StreamReader reader = new StreamReader(path))
            {
                do
                    lines.Add(reader.ReadLine());
                while (!reader.EndOfStream);
            }
            return lines;
        }

        public void WriteToFile(List<String> lines, String path)
        {
            using (StreamWriter writer = new StreamWriter(File.Create(path)))
            {
                foreach (string line in lines)
                    writer.WriteLine(line);
            }
        }
        #endregion

        #region CONSOLE MESSAGES
        // Write error message to console 
        public void ErrMsg(string text)
        {
            Console.WriteLine("ERROR: " + text);
        }

        // Write debugging message to console
        public void DevMsg(string text, dynamic variable)
        {
            Console.WriteLine("DEV MSG: " + text + variable);
        }
        public void DevMsg(string text)
        {
            Console.WriteLine("DEV MSG: " + text);
        }
        public void DevMsg(dynamic variable)
        {
            Console.WriteLine("DEV MSG: " + variable);
        }

        // Write new line to console
        public void NewLine()
        {
            Console.WriteLine("");
        }
        #endregion
    }
}
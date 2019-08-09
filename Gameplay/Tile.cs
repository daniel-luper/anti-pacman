using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AntiPacman.Gameplay.GameObjects;

namespace AntiPacman.Gameplay
{
    public class Tile
    {
        private Util util = new Util();

        // Tile properties
        public int Index { get; private set; }
        public int Line { get; set; }
        public bool IsWall { get; private set; }
        public bool IsEmpty { get; private set; }
        public bool HasPacdot { get; set; }
        public bool HasEnergizer { get; set; }
        public Vector2 Position { get; private set; }
        public Vector2 Center { get; private set; }

        // Collectibles
        public Pacdot Pacdot { get; private set; }
        public Energizer Energizer { get; private set; }

        // Developer mode only
        public Rectangle Bounds { get; private set; }
        public Color DevColor { get; set; }

        // Texture data extraction resources
        private Color[] rawData;
        private Rectangle extractRegion;

        public Tile(Game game, int column, int row, Texture2D colorMap)
        {
            // Data extraction
            rawData = new Color[1];
            extractRegion = new Rectangle(column, row, 1, 1);
            colorMap.GetData<Color>(0, extractRegion, rawData, 0, 1);

            // Tile property initialization
            Index = row * 28 + column;
            Line = row;
            IsWall = false;
            IsEmpty = false;
            HasPacdot = false;
            HasEnergizer = false;
            Reset();

            // Calculate position
            Position = new Vector2(column * 8, row * 8);
            Center = new Vector2(Position.X + 4.0f, Position.Y + 4.0f);

            Bounds = new Rectangle((int)Position.X, (int)Position.Y, 8, 8);

            // Collectibles
            Pacdot = new Pacdot(game, this);
            Energizer = new Energizer(game, this);

            // Developer mode only
            if (IsWall)
                DevColor = Color.DarkRed;
            else
                DevColor = Color.Green;
        }

        public void Reset()
        {
            if (rawData[0] == Color.Black)
                IsWall = true;
            else if (rawData[0] == Color.White)
                HasPacdot = false;
            else if (rawData[0] == Color.Blue)
                HasPacdot = true;
            else if (rawData[0] == Color.Red)
                HasEnergizer = true;
            else
                Console.WriteLine("ERROR: This tile type does not exist");
        }

        public bool HasPoint(Point point)
        {
            if (util.Distance(point.X, Center.X) <= 4 && util.Distance(point.Y, Center.Y) <= 4)
                return true;
            else
                return false;
        }

    }
}

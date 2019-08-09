using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AntiPacman.Gameplay
{
    public abstract class GameObject
    {
        protected Game game;
        protected Texture2D texture;

        protected Vector2 drawLocation;
        protected Vector2 drawTunnelLocationLeft;
        protected Vector2 drawTunnelLocationRight;
        public Rectangle Bounds { get; protected set; }

        public GameObject(Game game)
        {
            this.game = game;
        }

        public virtual void Load(string textureName)
        {
            texture = game.Content.Load<Texture2D>(@"Textures/Game/" + textureName);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 startDrawLocation)
        {
            spriteBatch.Draw(texture, startDrawLocation + drawLocation, Color.White);
        }
    }

    public enum Direction
    {
        None = -1,
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3
    }

    public struct Position
    {
        // The tile the entity is on.
        public Point Tile { get; set; }

        // The entity's actual location.
        public Point Location { get; set; }

        // How many pixels the entity is off its nominal tile.
        public int DeltaPixel
        {
            get { return Math.Abs(Tile.X - Location.X) + Math.Abs(Tile.Y - Location.Y); }
        }

        // Constructor
        public Position(Tile tile)
        {
            Tile = tile.Center.ToPoint();
            Location = tile.Center.ToPoint();
        }
    }
}

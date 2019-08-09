using System;
using Microsoft.Xna.Framework;

namespace AntiPacman.Gameplay.GameObjects
{
    public class Energizer : GameObject
    {
        private Tile tile;
        private const int sideLength = 8;

        public Energizer(Game game, Tile tile) : base(game)
        {
            this.tile = tile;
            drawLocation = tile.Position;
            Bounds = new Rectangle((int)drawLocation.X, (int)drawLocation.Y, sideLength, sideLength);
        }
    }
}

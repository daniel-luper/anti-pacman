using Microsoft.Xna.Framework;

namespace AntiPacman.Gameplay.GameObjects
{
    public class Pacdot : GameObject
    {
        private Tile tile;
        private const int sideLength = 2;

        public Pacdot(Game game, Tile tile) : base(game)
        {
            this.tile = tile;
            drawLocation = tile.Position;
            Bounds = new Rectangle((int)drawLocation.X + 3, (int)drawLocation.Y + 3, sideLength, sideLength);
        }
    }
}

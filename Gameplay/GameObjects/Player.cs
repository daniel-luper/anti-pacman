using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AntiPacman.Gameplay.GameObjects
{
    public class Player : GameObject
    {
        private Util util = new Util();

        private Direction direction = Direction.None;
        private Direction nextDirection = Direction.None;
        private const int speed = 60;
        private const int animationSpeed = 66;
        private int distance;
        private Position position;

        private TextureAnimation animation;

        private Tile[] tiles;
        private Tile currentTile;
        private Tile nextTile;
        private Tile[] surroundingTiles = new Tile[4];

        public Rectangle MiniBounds { get; set; }

        public Player(Game game, Tile[] tiles) : base(game)
        {
            this.tiles = tiles;
        }

        public void Initialize()
        {
            // Tiles.
            currentTile = tiles[23 * 28 + 14];
            surroundingTiles[(int)Direction.Up] = tiles[currentTile.Index - 28];
            surroundingTiles[(int)Direction.Down] = tiles[currentTile.Index + 28];
            surroundingTiles[(int)Direction.Left] = tiles[currentTile.Index -1];
            surroundingTiles[(int)Direction.Right] = tiles[currentTile.Index + 1];
            nextTile = currentTile;

            // Player movement.
            position = new Position(currentTile);
            position.Location = new Point(position.Location.X - 3, position.Location.Y);
            drawLocation = new Vector2(position.Location.X - 8, position.Location.Y - 8);
            direction = Direction.None;
            nextDirection = Direction.None;

            // Animation
            animation = new TextureAnimation(texture, 16, 16, new System.TimeSpan(0, 0, 0, 0, animationSpeed));

            // Collision boxes.
            Bounds = new Rectangle((int)drawLocation.X + 1, (int)drawLocation.Y + 1, 13, 13);
            MiniBounds = new Rectangle(Bounds.X + 2, Bounds.Y + 2, 9, 9);
        }

        public void HandleInput()
        {
            // Gather input data.
            var state = Keyboard.GetState();

            if (game.DevMode)
                nextDirection = Direction.None;

            // Set desired direction.
            if (state.IsKeyDown(Keys.Up))
                nextDirection = Direction.Up;
            if (state.IsKeyDown(Keys.Down))
                nextDirection = Direction.Down;
            if (state.IsKeyDown(Keys.Left))
                nextDirection = Direction.Left;
            if (state.IsKeyDown(Keys.Right))
                nextDirection = Direction.Right;
        }

        public void Update(GameTime gameTime)
        {
            // Update tiles.
            for (int i = 0; i < surroundingTiles.Length; i++)
            {
                if (surroundingTiles[i].HasPoint(position.Location))
                {
                    currentTile = surroundingTiles[i];
                    surroundingTiles[(int)Direction.Up] = tiles[currentTile.Index - 28];
                    surroundingTiles[(int)Direction.Down] = tiles[currentTile.Index + 28];
                    surroundingTiles[(int)Direction.Left] = tiles[currentTile.Index - 1];
                    surroundingTiles[(int)Direction.Right] = tiles[currentTile.Index + 1];
                }
            }
            if (surroundingTiles[(int)Direction.Left].Line != currentTile.Line)
                surroundingTiles[(int)Direction.Left] = tiles[currentTile.Index + 27];
            if (surroundingTiles[(int)Direction.Right].Line != currentTile.Line)
                surroundingTiles[(int)Direction.Right] = tiles[currentTile.Index -27];
            position.Tile = currentTile.Center.ToPoint();
            
            // Distance that the player can travel this frame.
            distance = (int)(speed * gameTime.ElapsedGameTime.TotalSeconds);

            // Change directions.
            if (nextDirection != Direction.None && nextDirection != direction)
            {
                if (!surroundingTiles[(int)nextDirection].IsWall && (position.DeltaPixel == 0 || 
                    (direction == Direction.None && position.DeltaPixel != 0)))
                    direction = nextDirection;
            }

            // Move player in current direction.
            if ((direction != Direction.None && !game.DevMode) || (game.DevMode && direction != Direction.None && nextDirection != Direction.None))
            {
                nextTile = surroundingTiles[(int)direction];
                if (!nextTile.IsWall || position.DeltaPixel > 0)
                    position.Location = Move(direction, distance, position.Location);
                else
                    direction = Direction.None;
            }

            drawLocation = new Vector2(position.Location.X - 8, position.Location.Y - 8);
            Bounds = new Rectangle((int)drawLocation.X + 1, (int)drawLocation.Y + 1, 13, 13);
            MiniBounds = new Rectangle(Bounds.X + 4, Bounds.Y + 4, 5, 5);

            // Go through tunnel
            if (Bounds.Right <= 0)
            {
                position.Location = new Point(position.Location.X + 224, position.Location.Y);
            }
            else if (Bounds.Left >= 224)
            {
                position.Location = new Point(position.Location.X - 224, position.Location.Y);
            }
            drawLocation = new Vector2(position.Location.X - 8, position.Location.Y - 8);
            drawTunnelLocationLeft = new Vector2(drawLocation.X + 224, drawLocation.Y);
            drawTunnelLocationRight = new Vector2(drawLocation.X - 224, drawLocation.Y);

            // Update animation
            animation.Update(gameTime);

            // Debugging
            if (game.DevMode)
                util.DevMsg("current tile is ", currentTile.Index);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 startDrawLocation)
        {
            // Draw debugging tiles
            if (game.DevMode)
            {
                for (int i = 0; i < surroundingTiles.Length; i++)
                    util.DrawRectangle(surroundingTiles[i].Bounds, surroundingTiles[i].DevColor, spriteBatch, game);
                util.DrawRectangle(currentTile.Bounds, Color.White, spriteBatch, game);
                if ((int)direction >= 0)
                    util.DrawRectangle(nextTile.Bounds, Color.Violet, spriteBatch, game);
                util.DrawRectangle(Bounds, Color.Brown, spriteBatch, game);
            }

            // Draw pacman
            animation.Draw(spriteBatch, startDrawLocation + drawLocation, (int)direction);
            animation.Draw(spriteBatch, startDrawLocation + drawTunnelLocationLeft, (int)direction);
            animation.Draw(spriteBatch, startDrawLocation + drawTunnelLocationRight, (int)direction);
        }

        

        /// <summary>
        /// Private methods only used by this class.
        /// </summary>
        private Point Move(Direction direction, int distance, Point location)
        {
            if (direction == Direction.Up)
                location = new Point(location.X, location.Y - distance);
            if (direction == Direction.Down)
                location = new Point(location.X, location.Y + distance);
            if (direction == Direction.Left)
                location = new Point(location.X - distance, location.Y);
            if (direction == Direction.Right)
                location = new Point(location.X + distance, location.Y);
            return location;
        }
    }
}

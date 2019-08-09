using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AntiPacman.Gameplay.GameObjects
{
    public class Ghost : GameObject
    {
        private Util util = new Util();

        private Texture2D colorTexture;

        private Direction direction = Direction.None;
        private Random random;
        private const int pixelsPerSecond = 1;
        private int distance;
        private Position position;

        private Tile[] tiles;
        private Tile startingTile;
        private Tile currentTile;
        private Tile nextTile;
        private Tile previousTile;
        private Tile[] surroundingTiles = new Tile[4];

        public bool AteCollectible { get; private set; }
        public bool IsChasing { get; set; }
        public bool IsAlive { get; set; }

        public Ghost(Game game, Tile[] tiles, Tile startingTile, Random random) : base(game)
        {
            this.tiles = tiles;
            this.startingTile = startingTile;
            this.random = random;
        }

        public void Load(string blueTexture, string textureName)
        {
            colorTexture = game.Content.Load<Texture2D>(@"Textures/Game/" + textureName);
            base.Load(blueTexture);
        }

        public void Initialize()
        {
            currentTile = startingTile;
            surroundingTiles[(int)Direction.Up] = tiles[currentTile.Index - 28];
            surroundingTiles[(int)Direction.Down] = tiles[currentTile.Index + 28];
            surroundingTiles[(int)Direction.Left] = tiles[currentTile.Index - 1];
            surroundingTiles[(int)Direction.Right] = tiles[currentTile.Index + 1];
            position = new Position(currentTile);
            nextTile = currentTile;
            drawLocation = new Vector2(position.Location.X - 7, position.Location.Y - 7);
            Bounds = new Rectangle((int)drawLocation.X, (int)drawLocation.Y, 14, 14);
            IsChasing = false;
            IsAlive = true;
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
            for (int i = 0; i < surroundingTiles.Length; i++)
            {
                if (surroundingTiles[i] != previousTile)
                {
                    if (direction == Direction.None)
                        previousTile = currentTile;
                    else
                        previousTile = surroundingTiles[(int)OppositeDirection(direction)];
                }
            }
            if (surroundingTiles[(int)Direction.Left].Line != currentTile.Line)
                surroundingTiles[(int)Direction.Left] = tiles[currentTile.Index + 27];
            if (surroundingTiles[(int)Direction.Right].Line != currentTile.Line)
                surroundingTiles[(int)Direction.Right] = tiles[currentTile.Index - 27];
            position.Tile = currentTile.Center.ToPoint();

            // Distance that the ghost can travel this frame.
            distance = (int)(pixelsPerSecond * gameTime.ElapsedGameTime.TotalSeconds * 60);

            // Manage AI.
            if (direction != Direction.None)
            {
                nextTile = surroundingTiles[(int)direction];
                if (!nextTile.IsWall || position.DeltaPixel > 0)
                    position.Location = Move(direction, distance, position.Location);
                
            }
            if (position.DeltaPixel == 0 || direction == Direction.None)
                CalculateNextMove();
            

            drawLocation = new Vector2(position.Location.X - 7, position.Location.Y - 7);
            Bounds = new Rectangle((int)drawLocation.X, (int)drawLocation.Y, 14, 14);

            // Go through tunnel.
            if (Bounds.Right <= 0)
            {
                position.Location = new Point(position.Location.X + 224, position.Location.Y);
            }
            else if (Bounds.Left >= 224)
            {
                position.Location = new Point(position.Location.X - 224, position.Location.Y);
            }
            drawLocation = new Vector2(position.Location.X - 7, position.Location.Y - 7);
            drawTunnelLocationLeft = new Vector2(drawLocation.X + 224, drawLocation.Y);
            drawTunnelLocationRight = new Vector2(drawLocation.X - 224, drawLocation.Y);

            // Collectibles.
            AteCollectible = false;
            if (currentTile.HasPacdot)
            {
                if (Bounds.Intersects(currentTile.Pacdot.Bounds))
                {
                    currentTile.HasPacdot = false;
                    AteCollectible = true;
                }
            }
            else if (currentTile.HasEnergizer)
            {
                if (Bounds.Intersects(currentTile.Pacdot.Bounds))
                {
                    currentTile.HasEnergizer = false;
                    AteCollectible = true;
                    IsChasing = true;
                }
            }
        }

        public void Respawn()
        {
            IsAlive = true;
            direction = Direction.None;
            Initialize();
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

            if (IsChasing)
            {
                spriteBatch.Draw(colorTexture, startDrawLocation + drawTunnelLocationLeft, Color.White);
                spriteBatch.Draw(colorTexture, startDrawLocation + drawTunnelLocationRight, Color.White);
                spriteBatch.Draw(colorTexture, startDrawLocation + drawLocation, Color.White);
            }
            else
            {
                spriteBatch.Draw(texture, startDrawLocation + drawTunnelLocationLeft, Color.White);
                spriteBatch.Draw(texture, startDrawLocation + drawTunnelLocationRight, Color.White);
                base.Draw(spriteBatch, startDrawLocation);
            }
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

        private void CalculateNextMove()
        {
            List<Direction> availableDirections = new List<Direction>();
            for (int i = 0; i < 4; i++)
            {
                if (!surroundingTiles[i].IsWall && surroundingTiles[i] != previousTile)
                {
                    availableDirections.Add((Direction)i);
                }
            }
            direction = availableDirections[random.Next(availableDirections.Count)];
        }

        private Direction OppositeDirection(Direction firstDirection)
        {
            if (firstDirection == Direction.Up)
                return Direction.Down;
            else if (firstDirection == Direction.Down)
                return Direction.Up;
            else if (firstDirection == Direction.Left)
                return Direction.Right;
            else if (firstDirection == Direction.Right)
                return Direction.Left;
            else
                return Direction.None;
        }

    }
}

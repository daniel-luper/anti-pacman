using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AntiPacman.Gameplay.GameObjects;
using AntiPacman.Engine;
using AntiPacman.Engine.States;
using Microsoft.Xna.Framework.Input;

namespace AntiPacman.Gameplay
{
    enum GhostName
    {
        Blinky,
        Pinky,
        Inky,
        Clyde
    }

    public class Map
    {
        // General
        private Game game;
        private StateManager stateManager;
        private Vector2 startDrawLocation;
        private Util util = new Util();

        // Game objects
        private Player pacman;
        private Ghost[] ghost = new Ghost[4];
        private Random ghostSeed = new Random();
        private const float chaseDuration = 4f;
        private float chaseTimeElapsed = 0;
        private bool chaseChecked = false;
        private int numGhosts;
        private int collectiblesEaten = 0;
        private int numCollectibles;

        // Map specific resources
        private Texture2D texture;
        private Texture2D propertiesTexture;
        private Tile[] tiles;

        public Map(Game game, StateManager stateManager)
        {
            this.game = game;
            this.stateManager = stateManager;
            startDrawLocation = game.CenterDrawLocation;
            tiles = new Tile[28 * 31];
            numGhosts = ghost.Length;
        }

        public void Load()
        {
            // Load map texture and properly initialize the starting draw location.
            texture = game.Content.Load<Texture2D>(@"Textures/Game/map");
            startDrawLocation.X -= texture.Width / 2;

            // Load tiles
            propertiesTexture = game.Content.Load<Texture2D>(@"Textures/Game/tiles");
            for (int j = 0; j < tiles.Length / 28; j++)
            {
                for (int i = 0; i < tiles.Length / 31; i++)
                {
                    int index = j * 28 + i;
                    tiles[index] = new Tile(game, i, j, propertiesTexture);
                    if (tiles[index].HasPacdot)
                    {
                        tiles[index].Pacdot.Load("pacdot");
                        numCollectibles++;
                    }
                    else if (tiles[index].HasEnergizer)
                    {
                        tiles[index].Energizer.Load("energizer");
                        numCollectibles++;
                    }
                }
            }

            // Load pacman
            pacman = new Player(game, tiles);
            pacman.Load("pacman-sheet");
            pacman.Initialize();

            // Load ghosts
            ghost[(int)GhostName.Blinky] = new Ghost(game, tiles, tiles[5 * 28 + 21], new Random(ghostSeed.Next()));
            ghost[(int)GhostName.Pinky] = new Ghost(game, tiles, tiles[5 * 28 + 6], new Random(ghostSeed.Next()));
            ghost[(int)GhostName.Inky] = new Ghost(game, tiles, tiles[23 * 28 + 21], new Random(ghostSeed.Next()));
            ghost[(int)GhostName.Clyde] = new Ghost(game, tiles, tiles[20 * 28 + 6], new Random(ghostSeed.Next()));
            for (int i = 0; i < ghost.Length; i++)
            {
                ghost[i].Load("ghost_00", "ghost_0" + (i+1));
                ghost[i].Initialize();
            }
        }

        public void Update(GameTime gameTime)
        {
            pacman.HandleInput();
            pacman.Update(gameTime);

            // Update ghosts
            chaseChecked = false;
            for (int i = 0; i < ghost.Length; i++)
            {
                if (ghost[i].IsAlive)
                {
                    ghost[i].Update(gameTime);
                    if (ghost[i].IsChasing && !chaseChecked)
                    {
                        for (int j = 0; j < ghost.Length; j++)
                            ghost[j].IsChasing = true;
                        chaseChecked = true;
                        chaseTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (chaseTimeElapsed >= chaseDuration)
                        {
                            chaseTimeElapsed = 0;
                            for (int j = 0; j < ghost.Length; j++)
                                ghost[j].IsChasing = false;
                        }
                        
                    }
                    if (ghost[i].AteCollectible)
                        collectiblesEaten++;
                    if (ghost[i].Bounds.Intersects(pacman.MiniBounds))
                    {
                        if (ghost[i].IsChasing)
                        {
                            collectiblesEaten = numCollectibles;
                        }
                        else
                        {
                            ghost[i].IsAlive = false;
                            numGhosts--;
                        }
                    }
                }         
            }

            // End game.
            if (collectiblesEaten >= numCollectibles)
            {
                Reset();
                stateManager.Set(new EndState(game, stateManager));
            }
            else if (numGhosts <= 0 || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Reset();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw map texture
            spriteBatch.Draw(texture, startDrawLocation, Color.White);

            // Draw collectibles
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].HasPacdot)
                {
                    tiles[i].Pacdot.Draw(spriteBatch, startDrawLocation);
                }
                    
                else if (tiles[i].HasEnergizer)
                {
                    tiles[i].Energizer.Draw(spriteBatch, startDrawLocation);
                }
            }

            // Draw ghosts
            for (int i = 0; i < ghost.Length; i++)
                if (ghost[i].IsAlive)
                    ghost[i].Draw(spriteBatch, startDrawLocation);

            // Draw Pacman
            pacman.Draw(spriteBatch, startDrawLocation);

            // Fill rest of map with black
            if (startDrawLocation.X != 0)
            {
                util.DrawRectangle(new Rectangle(0, 0, (int)startDrawLocation.X, game.ScreenHeight),
                    Color.Black, spriteBatch, game);
                util.DrawRectangle(new Rectangle((int)startDrawLocation.X + texture.Width, 0, (int)startDrawLocation.X * 2, game.ScreenHeight),
                    Color.Black, spriteBatch, game);
            }
        }

        public void Reset()
        {
            for (int i = 0; i < tiles.Length; i++)
                tiles[i].Reset();
            collectiblesEaten = 0;
            for (int i = 0; i < ghost.Length; i++)
                ghost[i].Respawn();
            numGhosts = ghost.Length;
            pacman.Initialize();
        }
    }
}
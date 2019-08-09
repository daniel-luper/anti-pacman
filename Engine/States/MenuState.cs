using AntiPacman.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace AntiPacman.Engine.States
{
    enum Selection
    {
        Play,
        Options,
        Scoreboard,
        Quit
    }

    public class MenuState : State
    {
        private Util util;
        private Clock timer;
        private const int wait = 150;
        private Vector2 startDrawLocation;
        private Texture2D titleScreen;
        private Texture2D arrow;
        private Vector2 arrowLocation;
        private Selection selection;

        // Background music.
        private Song radioStatic;
        private SoundEffect introMusic;
        private SoundEffectInstance introInstance;
        private bool audioPlaying;

        // UI sound effects.
        private SoundEffect sfxSelect;
        private SoundEffect sfxEnter;
        private SoundEffect sfxPlay;

        public MenuState(Game game, StateManager stateManager) : base(game, stateManager)
        {
            util = new Util();
            timer = new Clock();
            startDrawLocation = game.CenterDrawLocation;
            selection = Selection.Play;
        }

        public override void Initialize()
        {
            timer.Reset();
            base.Initialize();
        }

        public override void Load()
        {
            // Load title screen
            titleScreen = game.Content.Load<Texture2D>(@"Textures/Main Menu/main-menu");
            startDrawLocation.X -= titleScreen.Width / 2;

            // Load selection arrow
            arrow = game.Content.Load<Texture2D>(@"Textures/Main Menu/arrow");
            arrowLocation = new Vector2(startDrawLocation.X + 71, 110);

            // Load background music
            introMusic = game.Content.Load<SoundEffect>(@"Audio/intro");
            introInstance = introMusic.CreateInstance();
            introInstance.Play();
            radioStatic = game.Content.Load<Song>(@"Audio/static");
            MediaPlayer.Play(radioStatic);
            MediaPlayer.IsRepeating = true;
            audioPlaying = true;

            // Load sound effects
            sfxSelect = game.Content.Load<SoundEffect>(@"Audio/UI/select");
            sfxPlay = game.Content.Load<SoundEffect>(@"Audio/UI/start");
            sfxEnter = game.Content.Load<SoundEffect>(@"Audio/UI/enter");
        }

        public override void PostLoad()
        {
            introInstance.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            // Play audio
            if (!audioPlaying)
            {
                introInstance.Play();
                MediaPlayer.Play(radioStatic);
                MediaPlayer.IsRepeating = true;
                audioPlaying = true;
            }

            // Handle input.
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Enter) || (state.IsKeyDown(Keys.Space) && timer.ElapsedTime >= wait))
            {
                if (selection == Selection.Play)
                {
                    introInstance.Pause();
                    MediaPlayer.Stop();
                    audioPlaying = false;
                    sfxPlay.Play();
                    stateManager.Push(new IntroState(game, stateManager));
                }
                else if (selection == Selection.Options)
                {
                    sfxEnter.Play();
                    stateManager.Push(new OptionState(game, stateManager));
                } 
                else if (selection == Selection.Scoreboard)
                {
                    sfxEnter.Play();
                    stateManager.Push(new ScoreState(game, stateManager));
                }
                else if (selection == Selection.Quit)
                    game.Exit();
            }
                
            if (state.IsKeyDown(Keys.Down) && timer.ElapsedTime >= wait)
            {
                sfxSelect.Play();
                timer.Reset();
                if (selection == Selection.Quit)
                {
                    arrowLocation = new Vector2(arrowLocation.X, arrowLocation.Y - 33);
                    selection = selection - 3;
                }
                else
                {
                    arrowLocation = new Vector2(arrowLocation.X, arrowLocation.Y + 11);
                    selection = selection + 1;
                }
            }
            if (state.IsKeyDown(Keys.Up) && timer.ElapsedTime >= wait)
            {
                sfxSelect.Play();
                timer.Reset();
                if (selection == Selection.Play)
                {
                    arrowLocation = new Vector2(arrowLocation.X, arrowLocation.Y + 33);
                    selection = selection +3;
                }
                else
                {
                    arrowLocation = new Vector2(arrowLocation.X, arrowLocation.Y - 11);
                    selection = selection - 1;
                }
            }

            // Update timer.
            timer.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(titleScreen, startDrawLocation, Color.White);
            spriteBatch.Draw(arrow, arrowLocation, Color.White);
        }

        public override void Dispose()
        {
            introInstance.Stop();
            MediaPlayer.Stop();
            base.Dispose();
        }
    }
}

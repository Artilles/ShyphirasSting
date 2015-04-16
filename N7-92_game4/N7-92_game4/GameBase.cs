using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace N7_92_game4
{
    public enum Menustate
    {
        None = 0,
        Start = 1,
        NewGame = 2,
        Levelselect = 3,
        LevelDifficulty = 4,
        options = 5,
        highscore = 6,
        quit = 7,
        play = 8,
        playing = 9,
        score = 10,
        credits = 11
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameBase : Microsoft.Xna.Framework.Game
    {

        #region Variables
        public IList<Sprite> bg = new List<Sprite>();
        public static SpriteFont font;
        int back = 0;
        public static Camera camera;
        KeyboardState keyboardState, prevKeyboardState;
        GamePadState player1, prevPlayer1;
        MouseState mouseState, prevMouseState;
        public static Vector2 ScreenSize;
        public Boolean gamePlaying = false;
        public StartMenu start;
        public DiffMenu diff;
        public LevelMenu levmen;
        public OptionsMenu opt;
        public HighscoreScreen highscoreScreen;
        public static int difficulty = 1;
        public static int levelnumber = 1;
        public int levnum = 1;
        public int startBG = 1;
        public static Menustate state = Menustate.None;
        public static Menustate laststate = Menustate.None;
        static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Boolean kill = false;
        public Boolean bbutton = false;
        public static Level level;
        public static GraphicsDevice gr;
        public static int levelbeat = 0;
        public static int levelscore = 0;
        public int levelwait = 0;
        public static Boolean LevelDone = false;
        public SpriteFont sfont;
        public Sprite credits;

        public static AudioManager Audio;
        public static List<int> highscores;
        public static FileManager.GameSettings Settings;
        public static string saveFile = "save";
        public static string settingsFile = "settings";
        #endregion

        public GameBase()
        {
            graphics = new GraphicsDeviceManager(this);
            GameBase.gr = graphics.GraphicsDevice;
            Content.RootDirectory = "Content";

            ScreenSize.X = 800;
            ScreenSize.Y = 480;
            graphics.PreferredBackBufferWidth = (int)ScreenSize.X;
            graphics.PreferredBackBufferHeight = (int)ScreenSize.Y;
            GameBase.camera = new Camera();
            IsMouseVisible = false;

            SoundEffect.MasterVolume = 0.7f;
            MediaPlayer.Volume = 0.35f;

            start = new StartMenu();
            diff = new DiffMenu();
            levmen = new LevelMenu();
            level = new Level((int)ScreenSize.Y, (int)ScreenSize.X);

            Audio = new AudioManager(this);

            // Add Game Components
            this.Components.Add(Audio);
            this.Components.Add(new GamerServicesComponent(this));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            level = new Level();
            FileManager.Initialize();

            base.Initialize();

            BasicEffect effect = new BasicEffect(GraphicsDevice);
            GameServices.AddService<GraphicsDevice>(graphics.GraphicsDevice);
            GameServices.AddService<BasicEffect>(effect);

            highscores = FileManager.ReadHighscores();
            //Settings = FileManager.ReadGameSettings(settingsFile);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// Songs and sounds are also loaded here using Audio.LoadSong/Audio.LoadSound.
        /// </summary>
        protected override void LoadContent()
        {
            ContentClass.load(Content);
            start.Load();
            diff.Load();
            levmen.Load();
            opt = new OptionsMenu(this);
            highscoreScreen = new HighscoreScreen(this);
            credits = new Sprite();
            credits.LoadContent("CREDITS");
            sfont = ContentClass.Fonts["myFont"];

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            level.SetGraphicsManager(graphics);

            LoadBackgrounds();
        }

        public void LoadBackgrounds()
        {
            Sprite a = new Sprite();
            a.LoadContent("LOGOscreen");
            a.Scale = 1.0f;
            bg.Add(a);
            Sprite b = new Sprite();
            b.LoadContent("TITLEscreen");
            b.Scale = 1.0f;
            bg.Add(b);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Audio.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            laststate = state;
            keyboardState = Keyboard.GetState();
            player1 = GamePad.GetState(PlayerIndex.One);
            mouseState = Mouse.GetState();


            // Allows the game to exit
            if (state == Menustate.quit || player1.IsButtonDown(Buttons.Back))
                this.Exit();

            /* DEBUG & TEST */
            if (keyboardState.IsKeyUp(Keys.G) && prevKeyboardState.IsKeyDown(Keys.G))
            {
                graphics.ToggleFullScreen();
            }
            /* END DEBUG & TEST */


            if (kill)
            {
                LevelEnd();
            }
            if (GameBase.LevelDone)
            {
                LevelEnd();
                GameBase.LevelDone = false;
                state = Menustate.score;
            }
           
            if (state != Menustate.play && state != Menustate.playing)
            { 
                #region Menu Backbutton
                if ((keyboardState.IsKeyUp(Keys.Escape) && prevKeyboardState.IsKeyDown(Keys.Escape))
                    || (player1.IsButtonUp(Buttons.B) && prevPlayer1.IsButtonDown(Buttons.B)))
                {
                    if (state == Menustate.Start)
                    {
                        back = 0;
                        state = Menustate.None;
                    }
                    else if (state == Menustate.NewGame)
                    {
                        state = Menustate.Start;
                    }
                    else if (state == Menustate.Levelselect)
                    {
                        state = Menustate.Start;
                    }
                    else if (state == Menustate.LevelDifficulty)
                    {
                        state = Menustate.Levelselect;
                    }
                    else if (state == Menustate.options)
                    {
                        state = Menustate.Start;
                    }
                    else if (state == Menustate.highscore)
                    {
                        state = Menustate.Start;
                    }
                    Audio.PlaySound("menu_back");
                }
                #endregion

                if (state == Menustate.None)
                {
                    if ((keyboardState.IsKeyUp(Keys.Enter) && prevKeyboardState.IsKeyDown(Keys.Enter))
                        || (player1.IsButtonUp(Buttons.A) && prevPlayer1.IsButtonDown(Buttons.A)))
                    {
                        if (back != startBG)
                        {
                            back++;
                        }
                        if (back == startBG)
                        {
                            state = Menustate.Start;
                        }
                    }
                }
                else if (state == Menustate.Start)
                {
                    if (!Audio.IsSongActive)
                    {
                        Audio.PlaySong("main_menu", true);
                    }

                    start.lastKeyboardState = prevKeyboardState;
                    start.lastGamepadState = prevPlayer1;
                    start.Update(gameTime);
                }
                else if (state == Menustate.NewGame)
                {
                    diff.lastKeyboardState = prevKeyboardState;
                    diff.lastGamepadState = prevPlayer1;
                    diff.Update(gameTime);
                }
                else if (state == Menustate.Levelselect)
                {
                    if (bg.Count > startBG + 1)
                    {
                        back = startBG + 1;
                    }
                    levmen.lastKeyboardState = prevKeyboardState;
                    levmen.lastGamepadState = prevPlayer1;
                    levmen.Update(gameTime);
                }
                else if (state == Menustate.LevelDifficulty)
                {
                    diff.lastKeyboardState = prevKeyboardState;
                    diff.lastGamepadState = prevPlayer1;
                    diff.Update(gameTime);
                }
                else if (state == Menustate.options)
                {
                    opt.Update(gameTime);
                }
                else if (state == Menustate.highscore)
                {
                    if ((keyboardState.IsKeyUp(Keys.Escape) && prevKeyboardState.IsKeyDown(Keys.Escape))
                        || (player1.IsButtonUp(Buttons.B) && prevPlayer1.IsButtonDown(Buttons.B)))
                    {
                        state = Menustate.Start;
                    }
                    highscoreScreen.Update(gameTime);
                }
                else if (state == Menustate.quit)
                {
                    this.Exit();
                }
                else if (state == Menustate.score)
                {
                    levelwait++;
                    if (levelwait >= 120 && (keyboardState.IsKeyDown(Keys.Enter) || player1.IsButtonDown(Buttons.A) || player1.IsButtonDown(Buttons.Start)))
                    {
                        if (GameBase.levelnumber < 6)
                        {
                            GameBase.levelnumber++;
                            MakeLevel();                            
                        }
                        else
                        {
                            state = Menustate.credits;
                            levelwait = 0;
                        }

                        // Check highscore to see if it should be saved
                        if (levelscore > highscores[levelbeat - 1])
                        {
                            SaveLevel(levelbeat, levelscore);
                        }
                    }
                    
                }
                if (state == Menustate.credits)
                {
                    levelwait++;
                    if (levelwait >= 120 && ((keyboardState.IsKeyUp(Keys.Enter) && prevKeyboardState.IsKeyDown(Keys.Enter))
                        || (player1.IsButtonUp(Buttons.A) && prevPlayer1.IsButtonDown(Buttons.A))))
                    {
                        Audio.StopSong();
                        state = Menustate.Start;
                        levelwait = 0;                        
                    }
                }
                
                // Make Level
                if (state == Menustate.play)
                {
                    MakeLevel();
                }
            }
            if (state == Menustate.playing)
            {
                if(!Audio.IsSongActive)
                {
                    
                    switch (levelnumber)
                    {
                        default:
                            Audio.PlaySong("level1_music", true);
                            break;
                        case 1:
                            Audio.PlaySong("level1_music", true);
                            break;
                        case 2:
                            Audio.PlaySong("level1_music", true);
                            break;
                        case 3:
                            Audio.PlaySong("level1_music", true);
                            break;
                        case 4:
                            Audio.PlaySong("level1_music", true);
                            break;
                        case 5:
                            Audio.PlaySong("level5_music", true);
                            break;
                        case 6:
                            Audio.PlaySong("final_boss", true);
                            break;
                    }
                }
                level.Update(gameTime);
            }

            base.Update(gameTime);

            prevKeyboardState = keyboardState;
            prevPlayer1 = player1;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            if (state == Menustate.playing && laststate == Menustate.playing)
            {
                level.Draw(spriteBatch);
            }
            else
            {
                Camera.isLoadFinished = false;
                if (state == Menustate.Start && laststate == Menustate.Start)
                {
                    bg[back].Draw(spriteBatch);
                    start.Draw(spriteBatch);
                }
                else if ((state == Menustate.LevelDifficulty && laststate ==  Menustate.LevelDifficulty)|| (state == Menustate.NewGame && laststate == Menustate.NewGame))
                {
                    bg[back].Draw(spriteBatch);
                    diff.Draw(spriteBatch);
                }
                else if (state == Menustate.Levelselect && laststate == Menustate.Levelselect)
                {
                    bg[back].Draw(spriteBatch);
                    levmen.Draw(spriteBatch);
                }
                else if (state == Menustate.options && laststate == Menustate.options)
                {
                    bg[back].Draw(spriteBatch);
                    opt.Draw(spriteBatch, graphics);
                }
                else if (state == Menustate.highscore && laststate == Menustate.highscore)
                {
                    bg[back].Draw(spriteBatch);
                    highscoreScreen.Draw(spriteBatch, graphics);
                }
                else if (state == Menustate.score && laststate == Menustate.score)
                {
                   
                    int savedScore = 0;
                    switch (levelbeat)
                    {
                        case 1: savedScore = highscores[0]; break;
                        case 2: savedScore = highscores[1]; break;
                        case 3: savedScore = highscores[2]; break;
                        case 4: savedScore = highscores[3]; break;
                        case 5: savedScore = highscores[4]; break;
                        case 6: savedScore = highscores[5]; break;
                    }

                    int heightMod = 0;
                    spriteBatch.DrawString(ContentClass.Fonts["largeFont"], "Level " + levelbeat + " Complete",
                        new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - (ContentClass.Fonts["largeFont"].MeasureString("Level " + levelbeat + " Complete").X / 2),
                                150f), Color.Red);
                    heightMod += (int)ContentClass.Fonts["largeFont"].MeasureString("M").Y;
                    spriteBatch.DrawString(sfont, "Completed on " + diff.menuItems[difficulty],
                        new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - (sfont.MeasureString("Completed on " + diff.menuItems[difficulty]).X / 2),
                                150f + heightMod), Color.Black);
                    heightMod += sfont.LineSpacing;
                    if (levelscore > savedScore)
                    {
                        spriteBatch.DrawString(sfont, "NEW HIGH SCORE: " + levelscore,
                            new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - (sfont.MeasureString("NEW HIGHSCORE: " + levelscore).X / 2),
                                150f + heightMod),
                            Color.Red);
                    }
                    else
                    {
                        spriteBatch.DrawString(sfont, "Final Score: " + levelscore,
                            new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - (sfont.MeasureString("Final Score: " + levelscore).X / 2),
                                150f + heightMod),
                            Color.Black);
                    }
                    heightMod += sfont.LineSpacing;
                    spriteBatch.DrawString(sfont, "Previous High Score: " + savedScore, 
                        new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - (sfont.MeasureString("Previous High Score: " + savedScore).X / 2),
                            150f + heightMod),
                        Color.Black);
                }
                else if(state == Menustate.credits)
                {
                    credits.Draw(spriteBatch);
                }
                else
                {
                    if (state != Menustate.playing)
                    {
                        bg[back].Draw(spriteBatch);
                    }
                    //just in case
                }
            }
            spriteBatch.End();
            

            base.Draw(gameTime);
        }



        public static void QuitGame()
        {
            state = Menustate.quit;
        }

        public static void Diffset()
        {

        }

        public static void NewGame()
        {
            state = Menustate.NewGame;
        }

        public static void StartGame(int d)
        {
            difficulty = d;
            state = Menustate.play;
        }

        public static void SelectLevel(int l)
        {
            state = Menustate.LevelDifficulty;
            GameBase.levelnumber = l;

        }

        public static void LevelMenu()
        {
            state = Menustate.Levelselect;
        }

        public void MakeLevel()
        {
            levelwait = 0;
            level = new Level();
            level.SetGraphicsManager(graphics);
            Level.Pause = false;
            level.prevKeyboardState = keyboardState;
            level.prevPlayer1 = player1;
            level.LoadContent(GameBase.levelnumber, difficulty);
            state = Menustate.playing;
        }

        public static void Option()
        {
            state = Menustate.options;
        }

        public static void Highscores()
        {
            state = Menustate.highscore;
        }

        public static void EndLevel()
        {
            GameBase.kill = true;
        }

        public void LevelEnd()
        {
            GameBase.kill = false;
            state = Menustate.None;
            level = new Level();
            level.SetGraphicsManager(graphics);
            Level.Pause = false;
            back = 1;
        }

        public static void RestartLevel()
        {
            level = new Level();
            level.LoadContent(GameBase.levelnumber, difficulty);
            level.SetGraphicsManager(graphics);
            state = Menustate.playing;

        }

        public static void EndLevelProgress(int score, int levl)
        {
            levelscore = score;
            levelbeat = levl;           
            GameBase.LevelDone = true;
        }

        #region Save Game
        public void SaveLevel(int level, int highscore)
        {
            switch (level)
            {
                case 1:
                    highscores[0] = levelscore;
                    break;
                case 2:
                    highscores[1] = levelscore;
                    break;
                case 3:
                    highscores[2] = levelscore;
                    break;
                case 4:
                    highscores[3] = levelscore;
                    break;
                case 5:
                    highscores[4] = levelscore;
                    break;
                case 6:
                    highscores[5] = levelscore;
                    break;
            }
            FileManager.SaveHighscores(highscores);
        }
        #endregion

    }
}

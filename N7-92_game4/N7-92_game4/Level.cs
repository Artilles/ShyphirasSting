using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
namespace N7_92_game4
{
    /// <summary>
    /// add some shit here if you know what this class is for
    /// </summary>
    public class Level
    {
        #region Variables
        public int score = 0;
        float CUBEWIDTH = 115 * .8000f;
        GraphicsDevice graphics;
        GraphicsDeviceManager graphicsManager;
        public int width;
        Random r;
        public static int w, h;
        public ContentManager content;
        public List<Platform> platforms = new List<Platform>();
        public List<Enemy> enemies;
        public List<String> beginning = new List<String>();
        public List<String> triggers = new List<String>();
        public List<String> ending = new List<String>();
        Player player;
        private int x;
        public Text t;
        public static List<Text> text = new List<Text>();
        public static List<Text> textp = new List<Text>();
        public IList<Sprite> bg = new List<Sprite>();
        public List<Sprite> CutA = new List<Sprite>();
        public List<Sprite> CutM = new List<Sprite>();
        public List<Sprite> CutB = new List<Sprite>();
        public List<Sprite> spr = new List<Sprite>();
        public List<String> asset = new List<String>();
        Vector3 camera = new Vector3(0, 0, 0);
        Vector3 lastCam = new Vector3(0, 0, 0);
        Vector2 global = new Vector2(0, 0);
        Texture2D consoleR;
        public static bool Pause = false;
        public PauseMenu menu;
        public KeyboardState prevKeyboardState;
        public GamePadState prevPlayer1;
        public String backgroundData;
        public String cutsceneA;
        public String cutsceneB;
        public String cutsceneTr;
        public bool startBackground;
        public bool startcutbegin;
        public bool startcutend;
        public bool startcuttrigger;
        public DeadMenu deadmenu;
        public static float hardLeft;
        public static float hardRight;
        public static float skyCap;
        public static float floor;
        private Dictionary<int, Platform> tiles;
        public Boolean scenePlaying = true;
        public int scene = 0;
        public Boolean bosskilled = false;

        #region Ares 12.10
        public static bool isBossKilled;
        #endregion


        public Boolean middle = false;
        public List<Attack> shots;
        public Boolean final = false;
        public int levelnumber = 0;
        public static Boolean Debug = false;
        public static Boolean JustDied = false;
        public Boolean wasscene = false;
        public int diff = 0;
        public int time = 0;
        #endregion

        public Level()
        {
            GameBase.camera.Reset();
        }

        public Level(int height, int width)
        {
            h = height;
            w = width;
            GameBase.camera.Reset();
        }
        public void assetadd()
        {
           //asset.Add("Images//test1");
        }

        public void Load(List<String> assets)
        {
            asset = assets;
        }

        public void loadBg(ContentManager c)
        {
            int bgc = (asset.Count - 1) * (int)GameBase.ScreenSize.X;
            if (bgc == 0)
            {
                bgc = (int)GameBase.ScreenSize.X;
            }
            int totalw = width * (int)CUBEWIDTH;
            int w = (totalw/bgc) +1;
            content = c;

            for (int www = 0; www <= w; www++)
            {
                foreach (String a in asset)
                {
                    Sprite b = new Sprite();
                    b.LoadContent(a);
                    b.Scale = 1.0f;
                    bg.Add(b);
                }
            }

            for (int i = 0; i < bg.Count; i++)
            {
                if (i == 0)
                {
                    bg[i].Position = new Vector2((float)(bg[i].Size.Width) * -1, 0);
                }
                else
                {
                    #region Ares Modified 12.09
                    bg[i].Position = new Vector2(bg[i - 1].Position.X + bg[i - 1].Size.Width-1, 0);
                    #endregion
                }
            }

            if (beginning.Count != 0)
            {
                for (int i = 0; i < beginning.Count; i++)
                {
                    if (!beginning[i].Equals("null"))
                    {
                        Sprite b = new Sprite();
                        b.LoadContent(beginning[i]);
                        b.Scale = 1.0f;
                        CutA.Add(b);
                    }
                }
            }
            if (ending.Count != 0)
            {
                for (int i = 0; i < ending.Count; i++)
                {
                    if (!ending[i].Equals("null"))
                    {
                        Sprite b = new Sprite();
                        b.LoadContent(ending[i]);
                        b.Scale = 1.0f;
                        CutB.Add(b);
                    }
                }
            }
            if (triggers.Count != 0)
            {
                for (int i = 0; i < triggers.Count; i++)
                {
                    if (!triggers[i].Equals("null"))
                    {
                        Sprite b = new Sprite();
                        b.LoadContent(triggers[i]);
                        b.Scale = 1.0f;
                        CutM.Add(b);
                    }
                }
            }
        }

        public void PlatformAdd(List<Platform> P)
        {
            platforms = P;
        }

        public void SetGraphicsManager(GraphicsDeviceManager graphicsManager)
        {
            this.graphicsManager = graphicsManager;
        }

        //This holds everything that the Level has. dont forget to pass the c onto anything that needs to be loaded
        public void LoadContent(int levelNum, int dif)
        {
            diff = dif;
            r = new Random();
            player = new Player();
            assetadd();
            Model playerModel = ContentClass.models["dragon"];
            menu = new PauseMenu();
            menu.Load();
            player.Initialize(playerModel);
            platforms = new List<Platform>();
            enemies = new List<Enemy>(); 
            deadmenu = new DeadMenu();
            deadmenu.Load();
            GameBase.camera.Reset();
            GameBase.camera.removelist();
            levelnumber = levelNum;
            string s = "" + levelNum;
            string levelPath = "Content/Levels/" + s + ".txt";
            
            Stream fileStream = TitleContainer.OpenStream(levelPath);
            LoadTiles(fileStream);

            graphics = GameServices.GetService<GraphicsDevice>();

            foreach (Platform platform in platforms)
            {
                platform.LoadContent();
            }
            loadBg(content);

            consoleR = ContentClass.sprites["metal"];

            foreach (Enemy enemy in enemies)
            {
                enemy.LoadContent();
            }

            GameBase.camera.Reset();
        }

        private void LoadTiles(Stream fileStream)
        {

            // Load the level and ensure all of the lines are the same length.
            startBackground = false;
            startcutbegin = false;
            startcutend = false;
           
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                   if (line.Length != width && line[0].Equals("#"))
                         throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                    line = reader.ReadLine();
                }
            }

            // Allocate the tile grid.
            //tiles = new List<List<Platform>>();

            // Loop over every tile position,
            for (int y = 0; y < lines.Count; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    if (!startBackground && !startcutbegin && !startcuttrigger && !startcutend)
                    {
                        char tileType = lines[y][x];
                        LoadTile(tileType, x, y);
                    }
                    else if(startBackground)
                    {
                        if (lines[y][x] == '?')
                        {
                            startBackground = false;
                            startcutbegin = true;
                            break;
                        }
                        else
                        {
                            backgroundData += lines[y][x].ToString();
                        }
                    }
                    else if (startcutbegin)
                    {
                        if (lines[y][x] == '?')
                        {
                            startcutbegin = false;
                            startcuttrigger = true;
                            break;
                        }
                        else
                        {
                            cutsceneA += lines[y][x].ToString();
                        }
                    }
                    else if (startcuttrigger)
                    {
                        if (lines[y][x] == '?')
                        {
                            startcuttrigger = false;
                            startcutend = true;
                            break;
                            //startBackground = false;
                            break;
                        }
                        else
                        {
                            cutsceneTr += lines[y][x].ToString();
                        }
                    }
                    else if (startcutend)
                    {
                        if (lines[y][x] == '?')
                        {
                            startcutbegin = false;
                            startcutend = false;
                            startcuttrigger = false;
                            startBackground = false;
                            break;
                        }
                        else
                        {
                            cutsceneB += lines[y][x].ToString();
                        }
                    }
                }
            }
            handleBegin(cutsceneA);
            handleTrig(cutsceneTr);
            handleEnd(cutsceneB);
            handleBack(backgroundData);
            fliptiles(lines.Count - 1);
        }
        public void handleBegin(String s)
        {
            int b = 0;

            for (int x = 0; x < s.Length; x++)
            {
                if (s[x].Equals(','))
                {
                    beginning.Add(s.Substring(b, x - b));
                    b = x + 1;
                }
                else if (x == s.Length - 1)
                {
                    beginning.Add(s.Substring(b, x - b + 1));
                }
            }
        }
        public void handleTrig(String s)
        {
            int b = 0;

            for (int x = 0; x < s.Length; x++)
            {
                if (s[x].Equals(','))
                {
                    triggers.Add(s.Substring(b, x - b));
                    b = x + 1;
                }
                else if (x == s.Length - 1)
                {
                    triggers.Add(s.Substring(b, x - b + 1));
                }
            }

        }
        public void handleEnd(String s)
        {
            int b = 0;
            for (int x = 0; x < s.Length; x++)
            {
                if (s[x].Equals(','))
                {
                    ending.Add(s.Substring(b, x - b));
                    b = x + 1;
                }
                else if (x == s.Length - 1)
                {
                    ending.Add(s.Substring(b, x - b + 1));
                }
            }
        }
        public void handleBack(String s)
        {
            int b = 0;
            
            for (int x = 0; x < s.Length; x++)
            {
                if (s[x].Equals(','))
                {
                    asset.Add(s.Substring(b, x-b));
                    b = x+1;
                }
                else if (x == s.Length - 1)
                {
                    asset.Add(s.Substring(b, x - b+1));
                }
            }
        }
        public void fliptiles(int line)
        {
            int x = 0;
            float lastY = 0.00f;
            foreach (Platform p in platforms)
            {
                if (p.position.Y != lastY)
                {
                    x++;
                    lastY = p.position.Y;
                }

            }

            foreach (Platform p in platforms)
            {
                p.position.Y = (p.position.Y - MathHelper.ToRadians((line * CUBEWIDTH))) * -1;
            }
            foreach (Enemy p in enemies)
            {
                p.position.Y = (p.position.Y - MathHelper.ToRadians((line * CUBEWIDTH))) * -1;
            }
            player.position.Y = (player.position.Y - MathHelper.ToRadians((line * CUBEWIDTH))) * -1;
            GameBase.camera.Reset();
        }

        /// <summary>
        /// Loads an individual tile's appearance and behavior.
        /// </summary>
        /// <param name="tileType">
        /// The character loaded from the structure file which
        /// indicates what should be loaded.
        /// </param>
        /// <param name="x">
        /// The X location of this tile in tile space.
        /// </param>
        /// <param name="y">
        /// The Y location of this tile in tile space.
        /// </param>
        /// <returns>The loaded tile.</returns>
        private void LoadTile(char tileType, float x, float y)
        {
            Enemy enemA;
            Enemy enemB;
            Enemy enemC;
            Enemy enemD;
            Platform plat;
            string pl = "";
            switch (tileType)
            {
                // Blank space
                case '.':
                    //plat = new Platform(null, TileCollision.Passable);
                    break;

                case '-':
                    pl = "grass";
                    pl = pl + "" + r.Next(1, 4);
                    plat = new Platform(pl, TileCollision.Platform);
                    // plat.setPosition(x *-50, y *-50);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case '+':
                    pl = "jungle";
                    pl = pl + "" + r.Next(1, 4);
                    plat = new Platform(pl, TileCollision.Platform);
                    // plat.setPosition(x *-50, y *-50);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case '~':
                    pl = "path";
                    pl = pl + "" + r.Next(1, 3);
                    plat = new Platform(pl, TileCollision.Platform);
                    // plat.setPosition(x *-50, y *-50);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case 'R':
                    pl = "rock";
                    pl = pl + "" + r.Next(1, 5);
                    plat = new Platform(pl, TileCollision.Platform);
                    // plat.setPosition(x *-50, y *-50);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case '(':
                    plat = new Platform("stalag", TileCollision.Spikes);
                    // plat.setPosition(x *-50, y *-50);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case ')':
                    plat = new Platform("stalac", TileCollision.Spikes);
                    // plat.setPosition(x *-50, y *-50);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case 'L':
                    plat = new Platform("lava", TileCollision.Spikes);
                    // plat.setPosition(x *-50, y *-50);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case 'n':
                    pl = "wood";
                    pl = pl + "" + r.Next(1, 4);
                    plat = new Platform(pl, TileCollision.Platform);
                    // plat.setPosition(x *-50, y *-50);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case '"':
                    plat = new Platform("castleruins", TileCollision.Platform);
                    // plat.setPosition(x *-50, y *-50);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case '!':
                    plat = new Platform("regal", TileCollision.Platform);
                    // plat.setPosition(x *-50, y *-50);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case '@':
                    plat = new Platform("regalstairs", TileCollision.Platform);
                    // plat.setPosition(x *-50, y *-50);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case '$':
                    plat = new Platform("regalstairscarpet", TileCollision.Platform);
                    // plat.setPosition(x *-50, y *-50);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case '%':
                    plat = new Platform("regalcar", TileCollision.Platform);
                    // plat.setPosition(x *-50, y *-50);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case ',':
                    plat = new Platform("castleruinspass", TileCollision.Passable);
                    // plat.setPosition(x *-50, y *-50);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    plat = new Platform("castleruinspass", TileCollision.Fall);
                    // plat.setPosition(x *-50, y *-50);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);            
                    break;
                case '\'':
                    plat = new Platform("castleruinspass", TileCollision.Passable);
                    // plat.setPosition(x *-50, y *-50);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    plat = new Platform("castleruinshrz", TileCollision.Fall);
                    // plat.setPosition(x *-50, y *-50);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case'?':
                    plat = new Platform("castleruinspass", TileCollision.Passable);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case ':':
                    plat = new Platform("spike", TileCollision.Spikes);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case '|':
                    plat = new Platform("spikebase", TileCollision.Platform);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case '[':
                    plat = new Platform("woodstairs", TileCollision.Passable);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    plat.rotate(180);
                    platforms.Add(plat);
                    break;
                case ']':
                    plat = new Platform("woodstairs", TileCollision.Passable);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;

                case '^':
                    plat = new Platform("bridge", TileCollision.Platform);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case '>':
                    plat = new Platform("bars", TileCollision.Passable);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case ';':
                    plat = new Platform("spike", TileCollision.Spikes);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    plat = new Platform("castleruinspass", TileCollision.Passable);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case '_':
                    plat = new Platform("castleruinshrz", TileCollision.Platform);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case 'c':
                    plat = new Platform("water", TileCollision.Boss);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case 't':
                    plat = new Platform("treebase", TileCollision.Break);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case 'g':
                    plat = new Platform("tree", TileCollision.Break);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
               /* case 'b':
                    plat = new Platform("treetop", TileCollision.Break);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;*/
                case 'a':
                    plat = new Platform("guardbase", TileCollision.Column);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    enemB = new Enemy("archer");
                    enemB.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    enemies.Add(enemB);
                    break;
                case 'z':
                    plat = new Platform("guard", TileCollision.Passable);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case 'q':
                    plat = new Platform("guardtop", TileCollision.Platform);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case 'm':
                    pl = "mud";
                    pl = pl + "" + r.Next(1, 2);
                    plat = new Platform(pl, TileCollision.Spikes);
                    // plat.setPosition(x *-50, y *-50);
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    platforms.Add(plat);
                    break;
                case 'S':
                    player.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    GameBase.camera.Reset();
                    break;
                case 'x':                    
                    plat = new Platform("eightycube", TileCollision.Left);                    
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    hardLeft = plat.position.X;
                    hardLeft = plat.position.X;
                    floor = plat.position.Y + 8.6f;                  
                    floor = plat.position.Y + 8.6f;
                    Camera.begin = plat.position.X ;
                    break;
                case 'y':
                    plat = new Platform("eightycube", TileCollision.Right);                   
                    plat.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    hardRight = plat.position.X;
                    skyCap = plat.position.Y + 8.6f;
                    Camera.end = plat.position.X - 6f;
                    Camera.end = plat.position.X;
                    break;
                case '#':
                    startBackground = true;
                    break;
                case 'A':
                    enemA = new Enemy("spear");
                    enemA.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    enemies.Add(enemA);
                    break;
                case 'B':
                    enemB = new Enemy("archer");
                    enemB.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    enemies.Add(enemB);
                    break;
                case 'C':
                    enemC = new Enemy("turtle");
                    enemC.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    enemies.Add(enemC);
                    break;
                case 'D':
                    enemD = new Enemy("dude");
                    enemD.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    enemies.Add(enemD);
                    break;
                case 'T':
                    enemD = new Enemy("troll");
                    enemD.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    enemies.Add(enemD);
                    break;
                case 'H':
                    enemD = new Enemy("crossbow");
                    enemD.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    enemies.Add(enemD);
                    break;
                case 'K':
                    enemD = new Enemy("lessknight");
                    enemD.setAbsolutePosition(x * CUBEWIDTH, y * CUBEWIDTH);
                    enemies.Add(enemD);
                    break;
                default:
                    break;
            }
        }
        public void updateBackGround(GameTime t)
        {
            Vector2 aDirection = new Vector2(-1, 0);
            Vector2 aSpeed = new Vector2(50, 0);/*
            for (int i = 0; i < bg.Count; i++)
            {
                if (i == 0)
                {
                    if (bg[i].Position.X < 0- bg[i].Size.Width)
                    {
                        bg[i].Position.X = bg[bg.Count - 1].Position.Y + bg[bg.Count - 1].Size.Width;
                    }
                }
                else
                {
                    if (bg[i].Position.X < 0-bg[i].Size.Width)
                    {
                        bg[i].Position.X = bg[i - 1].Position.Y + bg[i - 1].Size.Width;
                    }
                }
            }*/
            for (int x = 0; x < bg.Count; x++)
            {
                bg[x].update(0);
            }
        }

        private Platform LoadPlatform(String name, TileCollision collision)
        {
            return new Platform(name, collision);
        }        

        private void DrawTiles(SpriteBatch spriteBatch)
        {
            graphics.BlendState = BlendState.Opaque;

            graphics.DepthStencilState = DepthStencilState.Default;
            foreach (Platform p in platforms)
            {
                p.Draw(spriteBatch);
            }
            /*
            for (int i = platforms.Count - 1; i >= 0; i--)
            {
                platforms[i].Draw(spriteBatch);
            }*/
            
            foreach (Enemy e in enemies)
            {
                e.Draw(spriteBatch);
            }
        }


        public void Update(GameTime gametime)
        {
            #region Ares 12.10

            isBossKilled = bosskilled;


            #endregion


            wasscene = scenePlaying;
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState player1 = GamePad.GetState(PlayerIndex.One);

            if (scenePlaying)
            {
                x++;
                if ((keyboardState.IsKeyDown(Keys.Enter) && !prevKeyboardState.IsKeyDown(Keys.Enter))
                    || (player1.IsButtonDown(Buttons.A) && !prevPlayer1.IsButtonDown(Buttons.A))
                    || x == 7 * 60)
                {
                    GameBase.Audio.PlaySound("menu_select");
                    scene++;
                    x = 0;
                }
                if (!middle && !bosskilled && !final)
                {
                    if (scene >= CutA.Count)
                    {
                        middle = true;
                        scenePlaying = false;
                        prevKeyboardState = keyboardState;
                        prevPlayer1 = player1;
                        scene = 0;
                        x = 0;
                    }
                }
                if (bosskilled)
                {
                    middle = false;
                    bosskilled = false;
                    final = true;
                    scene = 0;
                    prevKeyboardState = keyboardState;
                    prevPlayer1 = player1;
                    x = 0;
                }
                if (final && scene == CutB.Count)
                {
                    switch (GameBase.difficulty)
                    {
                        // Blank space
                        case 0:
                            score += (time / 4) + (player.health/2);
                            break;
                        case 1:
                            score += (time / 3) + (player.health);
                            break;
                        case 2:
                            score += time / 2 + player.health*2;
                            break;
                        case 3: ;
                            score += time + player.health *4;
                            break;
                    }
                    GameBase.EndLevelProgress(score, levelnumber);
                }
            }
            else if (!Level.Pause && !player.dead && !scenePlaying)
            {
                if ((keyboardState.IsKeyDown(Keys.Escape) && !prevKeyboardState.IsKeyDown(Keys.Escape))
                    || (player1.IsButtonDown(Buttons.Start) && !prevPlayer1.IsButtonDown(Buttons.Start)))
                {
                    GameBase.Audio.Enabled = false;
                    GameBase.Audio.ChangeEnabled();
                    Level.Pause = true;
                }

                Vector2 p = player.GetCoordsRadCentre();
                Vector2 v = player.GetSpeed();
                
                if (Level.JustDied)
                {
                    GameBase.camera.Reset();
                    Level.JustDied = false;
                }
                player.Update(gametime);
                time++;
                float frameRate = 1 / (float)gametime.ElapsedGameTime.TotalSeconds;

                GameBase.camera.Update(p, v);
                checkDistance();

                foreach (Platform platform in platforms)
                {
                    platform.Update(gametime, x, camera);
                }

                foreach (Enemy e in enemies)
                {
                    e.Update(gametime, x, camera);              
                }

                if (keyboardState.IsKeyDown(Keys.D))
                {
                    debug();
                }
                else
                {
                    Debug = false;
                }

                updateBackGround(gametime);
                foreach (Sprite s in spr)
                {
                    s.update();
                } 
                CheckCollisions();
            }
            else if (player.dead)
            {
                if (GameBase.Audio.IsSongActive)
                {
                    GameBase.Audio.PauseSong();
                }
                deadmenu.Update(gametime);
            }
            else
            {
                if ((keyboardState.IsKeyDown(Keys.Escape) && !prevKeyboardState.IsKeyDown(Keys.Escape))
                    || (player1.IsButtonDown(Buttons.Start) && !prevPlayer1.IsButtonDown(Buttons.Start)))
                {
                    Level.Pause = false;
                    GameBase.Audio.Enabled = true;
                    GameBase.Audio.ChangeEnabled();
                }
                menu.Update(gametime);                
            }

            prevKeyboardState = keyboardState;
            prevPlayer1 = player1;
        }

        public void drawBG(SpriteBatch b)
        {
            foreach (Sprite s in bg)
            {
                b.Draw(s.mSpriteTexture, s.Position, null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 1.0f);
            }
        }

        public void debug()
        {
            Level.Debug = true;
            //write("" +score, 50f, 50f);
            int y = 1;
            int d = 1;
            int x = 25;
            Debug = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            graphics.BlendState = BlendState.Opaque;
            graphics.DepthStencilState = DepthStencilState.Default;

            if (!scenePlaying && !wasscene)
            {
                drawBG(spriteBatch);
                spriteBatch.End();
                DrawTiles(spriteBatch);
                player.Draw(spriteBatch);
                spriteBatch.Begin();

                drawText(spriteBatch);
                DrawScore(spriteBatch);
            }
            else
            {
                if (!middle && !bosskilled && !final && CutA.Count > 0)
                {
                    CutA[scene].Draw(spriteBatch);
                }
                else if (middle && CutM.Count > 0 && !bosskilled &&!final && !wasscene)
                {
                    CutM[scene].Draw(spriteBatch);
                }
                else if ((bosskilled || final) && CutB.Count>0 && scene < CutB.Count)
                {
                    CutB[scene].Draw(spriteBatch);
                }
            }
            if (Level.Pause)
            {
                menu.Draw(spriteBatch, graphicsManager);
            }
            if (player.dead)
            {
                deadmenu.Draw(spriteBatch, graphicsManager);
            }
        }

        private void DrawScore(SpriteBatch spriteBatch)
        {
            SpriteFont font = ContentClass.Fonts["myFont"];
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(graphics.Viewport.Width - 139f, 30f), Color.Black);
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(graphics.Viewport.Width - 141f, 30f), Color.Black);
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(graphics.Viewport.Width - 140f, 29f), Color.Black);
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(graphics.Viewport.Width - 140f, 31f), Color.Black);
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(graphics.Viewport.Width - 140f, 30f), Color.White);
        }
        
        public void CheckCollisions()
        {
            player.collisionBottom = false;
            player.collisionLeft = false;
            player.collisionRight = false;
            player.collisionTop = false;            

            if (player.bounds.Min.Y < floor)
            {
                player.health = 0;
            }
            if (player.bounds.Max.Y > skyCap)
            {
                player.collisionTop = true;
                player.position.Y -= player.mSpeed.Y;
                player.mSpeed.Y = 0;
            }

            foreach (Platform platform in platforms)
            {
                if (platform.bounds.Intersects(player.bufferedBounds))
                {
                    platform.Visible = true;
                }
                else
                {
                    platform.Visible = false;
                }
                if (platform.Visible)
                {

                        #region enemies
                        foreach (Enemy e in enemies)
                        {
                            if (e.bounds.Intersects(player.bufferedBounds))
                            {
                                e.Visible = true;
                                e.collisionBottom = false;
                            }
                            else
                            {
                                e.Visible = false;
                            }
                            if (e.bounds.Intersects(platform.bounds) && platform.Collision != TileCollision.Passable)
                            {
                                if (e.bounds.Min.Y < platform.bounds.Max.Y && platform.Collision == TileCollision.Spikes)
                                {
                                    if (!e.boss)
                                    {
                                        e.health = 0;
                                    }
                                }
                                if (e.bounds.Min.Y < platform.bounds.Max.Y && platform.Collision == TileCollision.Fall)
                                {
                                    e.collisionBottom = true;
                                    platform.mSpeed.Y = .098f;
                                }                               
                                else if (e.bounds.Min.Y < platform.bounds.Max.Y)
                                {
                                    e.collisionBottom = true;
                                    e.position.Y -= e.mSpeed.Y;
                                    e.mSpeed.Y = 0;
                                }
                                if (e.bounds.Max.Y > platform.bounds.Min.Y && (e.bounds.Min.Y - platform.bounds.Max.Y) < -.3)
                                {
                                    e.collisionTop = true;
                                    e.position.Y -= e.mSpeed.Y;
                                    e.mSpeed.Y = 0;
                                }
                                if (e.bounds.Min.X < platform.bounds.Max.X && (e.bounds.Min.Y - platform.bounds.Max.Y) < -(.5 + e.mSpeed.Y))
                                {
                                    e.collisionLeft = true;
                                    e.position.X -= e.mSpeed.X + 65f;
                                    e.mSpeed.X = 0;
                                }
                                if (e.bounds.Max.X > platform.bounds.Min.X && (e.bounds.Min.Y - platform.bounds.Max.Y) < -(.5 + e.mSpeed.Y))
                                {
                                    e.collisionRight = true;
                                    e.position.X -= e.mSpeed.X - 65f;
                                    e.mSpeed.X = 0;
                                }
                            }
                            #region enemy projectiles
                            foreach (Attack a in e.attacks)
                            {
                              if (a.bounds.Intersects(player.bufferedBounds))
                                {
                                    a.Visible = true;
                                }
                                else
                                {
                                    a.Visible = false;
                                }
                                if (a.Visible)
                                {
                                    if (a.bounds.Intersects(platform.bounds) && a.model != null)
                                    {
                                        a.deeps = 0;
                                        a.Visible = false;
                                    }
                                }
                            }
                            #endregion
                        }
                
                        #endregion

                        if (player.bounds.Intersects(platform.bounds) && platform.Collision != TileCollision.Passable)
                        {
                            if (player.bounds.Min.Y < platform.bounds.Max.Y && platform.Collision == TileCollision.Spikes)
                            {
                                player.health = 0;
                            }
                            if (player.bounds.Min.Y < platform.bounds.Max.Y && platform.Collision == TileCollision.Fall)
                            {
                                player.collisionBottom = true;
                                platform.mSpeed.Y = .098f;
                            }
                            else if (player.bounds.Min.Y < platform.bounds.Max.Y && platform.Collision == TileCollision.Spikes)
                            {
                                player.collisionBottom = true;
                                player.mSpeed.Y = 0;
                                player.position.Y -= 0.05f;
                            }
                            else if (player.bounds.Min.Y < platform.bounds.Max.Y && player.mSpeed.Y <= 0)
                            {
                                player.collisionBottom = true;
                                //player.position.Y -= player.mSpeed.Y;
                                player.mSpeed.Y = 0;                                
                            }
                            if (player.bounds.Max.Y > platform.bounds.Min.Y && (player.bounds.Min.Y - platform.bounds.Max.Y) < -.3)
                            {
                                player.collisionTop = true;
                                player.position.Y -= player.mSpeed.Y;
                                player.mSpeed.Y = 0;
                            }
                            if (player.bounds.Min.X < platform.bounds.Max.X && (player.bounds.Min.Y - platform.bounds.Max.Y) < -(.5 + player.mSpeed.Y))
                            {
                                player.collisionLeft = true;
                                player.position.X -= player.mSpeed.X + 65f;
                                player.mSpeed.X = 0;
                            }
                            if (player.bounds.Max.X > platform.bounds.Min.X && (player.bounds.Min.Y - platform.bounds.Max.Y) < -(.5 + player.mSpeed.Y))
                            {
                                player.collisionRight = true;
                                player.position.X -= player.mSpeed.X - 65f;
                                player.mSpeed.X = 0;
                            }
                        }

                        #region player attacks
                        foreach (Attack a in player.attacks)
                        {
                            if (a.bounds.Intersects(player.bufferedBounds))
                            {
                              a.Visible = true;
                            }
                            else
                            {
                             a.Visible = false;
                            }
                            if (a.Visible)
                            {
                                if (a.model == null)
                                {
                                    if (a.bounds.Intersects(platform.bounds) && platform.Collision == TileCollision.Boss)
                                    {
                                            platform.Visible = false;
                                            bosskilled = true; 
                                            scenePlaying = true;    
                                    }
                                }
                                if (a.bounds.Intersects(platform.bounds) && platform.Collision != TileCollision.Column && a.model != null)
                                {
                                    a.deeps = 0;
                                    a.Visible = false;                                   
                                }
                            }
                        }
                        #endregion
                    }
                }
            
            combat();
        }
        public void write(String a)
        {
            t = new Text(a);
            Text.font = ContentClass.Fonts["myFont"];
            text.Add(t);

        }
        public void write(String a, Vector2 xy)
        {
            t = new Text(a, xy);
            t.Lo(content);
            text.Add(t);

        }
        public void write(String a, float x, float y)
        {
            Text t = new Text(a, x, y);
            t.Lo(content);
            text.Add(t);

        }
        public void writep(String a, float x, float y)
        {
            Text t = new Text(a, x, y);
            t.Lo(content);
            textp.Add(t);
        }
        public void drawText(SpriteBatch s)
        {
            foreach (Text i in text)
            {
                i.draw(s);
            }
            foreach (Text i in textp)
            {
                i.draw(s);

            }
            text.Clear();

        }
        public static void Unpause()
        {
            Level.Pause = false;
        }
        public void combat()
        {
            for(int e = 0; e < enemies.Count; e++)
            {
                if (enemies[e].Dead)
                {
                    if (enemies[e].boss)
                    {
                        bosskilled = true;
                        
                        scenePlaying = true;
                    }
                    score += enemies[e].score;
                    enemies.Remove(enemies[e]);
                    e--;
                }
                else
                {
                    foreach (Attack a in enemies[e].attacks)
                    {
                        if (player.bounds.Intersects(a.bounds))
                        {
                            player.hit(a.deeps);
                           // a.deeps = 0;
                            a.Visible = false;
                            player.invinc(120);
                        }
                        foreach (Attack aa in player.attacks)
                        {
                            if (a.bounds.Intersects(aa.bounds))
                            {
                                if (aa.modelstr.Equals("fireball"))
                                {
                                    a.deeps = 0;
                                    a.Visible = false;
                                }
                            }

                        }
                    }
                    foreach (Attack fireball in player.attacks)
                    {
                        if (fireball.bounds.Intersects(enemies[e].bounds))
                        {
                            if (enemies[e].defence == 0)
                            {
                                enemies[e].defence = 1;
                            }
                            enemies[e].hit(fireball.deeps/enemies[e].defence);
                            fireball.deeps = 0;
                            fireball.Visible = false;
                            //enemies[e].invinc(15);
                        }
                    }
                    if (player.bounds.Intersects(enemies[e].bounds))
                    {
                      //  player.hit(5);
                    //    player.invinc(60);
                    }
                }
            }
        }
        public void checkDistance()
        {
            //Console.WriteLine(MathHelper.ToDegrees(player.position.Y) - MathHelper.ToDegrees(GameBase.camera.cameraPosition.Y));
            if(MathHelper.ToDegrees(player.position.X) - MathHelper.ToDegrees(GameBase.camera.cameraPosition.X) > 200
                ||MathHelper.ToDegrees(player.position.X) - MathHelper.ToDegrees(GameBase.camera.cameraPosition.X) < -200
                ||MathHelper.ToDegrees(player.position.Y) - MathHelper.ToDegrees(GameBase.camera.cameraPosition.Y) > 200
                  || MathHelper.ToDegrees(player.position.Y) - MathHelper.ToDegrees(GameBase.camera.cameraPosition.Y) < -200)
            {
                if (player.health > 0)
                {
                    GameBase.camera.Reset();
                }
           
            }
        }     
    }
}

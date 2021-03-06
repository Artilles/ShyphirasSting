﻿using System;
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
    public class ContentClass
    {
        /// <summary>
        /// Load a whatever before the game starts, then instead of loading from content make = this
        /// access like fonts["myFont"] to load "myFont"
        /// instead of in the loadcontent of the object
        /// </summary>
        public static Dictionary<String, SpriteFont> Fonts;
        public static Dictionary<String, Model> models;
        public static GraphicsDevice graphics;
        public static Dictionary<String, Texture2D> sprites;

        public static void load(ContentManager c)
        {
            int x = 0;
            Fonts = new Dictionary<String, SpriteFont>();
            models = new Dictionary<String, Model>();
            sprites = new Dictionary<String, Texture2D>();
            Console.WriteLine(x++ + " Initializing");
            Fonts.Add("myFont", c.Load<SpriteFont>("Fonts\\myFont"));
            Console.WriteLine(x++ + " loading myfont");
            Fonts.Add("largeFont", c.Load<SpriteFont>("Fonts\\largeFont"));
            Console.WriteLine(x++ + " loading largefont");

            models.Add("dragon", c.Load<Model>("Models\\dragon"));
            Console.WriteLine(x++ + " loading dragon");
            models.Add("cube", c.Load<Model>("Models\\cube"));
            Console.WriteLine(x++ + " loading cube");
            models.Add("fireball", c.Load<Model>("Models\\fireball"));
            Console.WriteLine(x++ + " loading fireball");
            models.Add("eightycube", c.Load<Model>("Models\\eightycube"));
            Console.WriteLine(x++ + " loading eightycube");
            models.Add("bullet", c.Load<Model>("Models\\bullet"));
            Console.WriteLine(x++ + " loading bullet");
            models.Add("platform", c.Load<Model>("Models\\platform"));
            Console.WriteLine(x++ + " loading platform");
            models.Add("platform2", c.Load<Model>("Models\\platform2"));
            Console.WriteLine(x++ + " loading platform2");
            models.Add("halfcube", c.Load<Model>("Models\\halfcube"));
            Console.WriteLine(x++ + " loading halfcube");
            models.Add("archer", c.Load<Model>("Models\\archer"));
            Console.WriteLine(x++ + " loading archer");
            models.Add("spear", c.Load<Model>("Models\\spear"));
            Console.WriteLine(x++ + " loading spear");
            models.Add("castleruins", c.Load<Model>("Models\\castleruins"));
            Console.WriteLine(x++ + " loading castle ruins");
            models.Add("castleruinspass", c.Load<Model>("Models\\castleruinspass"));
            Console.WriteLine(x++ + " loading castleruinspass");
            models.Add("castleruinshrz", c.Load<Model>("Models\\castleruinshrz"));
            Console.WriteLine(x++ + " loading castleruinbh");
            models.Add("spike", c.Load<Model>("Models\\spike"));
            Console.WriteLine(x++ + " loading spike");
            models.Add("arrow", c.Load<Model>("Models\\arrow"));
            Console.WriteLine(x++ + " loading arrow");
            models.Add("guard", c.Load<Model>("Models\\guard"));
            Console.WriteLine(x++ + " loading gaurd");
            models.Add("guardbase", c.Load<Model>("Models\\guardbase"));
            Console.WriteLine(x++ + " loading guardbase");
            models.Add("guardtop", c.Load<Model>("Models\\guardtop"));
            Console.WriteLine(x++ + " loading guardtop");
            models.Add("mud1", c.Load<Model>("Models\\mud1"));
            Console.WriteLine(x++ + " loading mud1");
            models.Add("mud2", c.Load<Model>("Models\\mud2"));
            Console.WriteLine(x++ + " loading mud2");
            models.Add("path1", c.Load<Model>("Models\\path1"));
            Console.WriteLine(x++ + " loading path1");
            models.Add("path2", c.Load<Model>("Models\\path2"));
            Console.WriteLine(x++ + " loading path2");
            models.Add("rock1", c.Load<Model>("Models\\rock1"));
            Console.WriteLine(x++ + " loading rock1");
            models.Add("rock2", c.Load<Model>("Models\\rock2"));
            Console.WriteLine(x++ + " loading rock2");
            models.Add("rock3", c.Load<Model>("Models\\rock3"));
            Console.WriteLine(x++ + " loading rock1");
            models.Add("rock4", c.Load<Model>("Models\\rock4"));
            Console.WriteLine(x++ + " loading rock4");
            models.Add("wood1", c.Load<Model>("Models\\wood1"));
            Console.WriteLine(x++ + " loading wood1");
            models.Add("wood2", c.Load<Model>("Models\\wood2"));
            Console.WriteLine(x++ + " loading wood2");
            models.Add("wood3", c.Load<Model>("Models\\wood3"));
            Console.WriteLine(x++ + " loading wood1");
            models.Add("stalag", c.Load<Model>("Models\\stalag"));
            Console.WriteLine(x++ + " loading rock1");
            models.Add("stalac", c.Load<Model>("Models\\stalac"));
            Console.WriteLine(x++ + " loading rock2");
            models.Add("water", c.Load<Model>("Models\\water"));
            Console.WriteLine(x++ + " loading water");
            models.Add("lava", c.Load<Model>("Models\\lava"));
            Console.WriteLine(x++ + " loading lava");
            models.Add("turtle", c.Load<Model>("Models\\turtle"));
            Console.WriteLine(x++ + " loading turtle");
            models.Add("grass1", c.Load<Model>("Models\\grass1"));
            Console.WriteLine(x++ + " loading grassblock1");
            models.Add("grass2", c.Load<Model>("Models\\grass2"));
            Console.WriteLine(x++ + " loading grassblock2");
            models.Add("grass3", c.Load<Model>("Models\\grass3"));
            Console.WriteLine(x++ + " loading grassblock3");
            models.Add("jungle1", c.Load<Model>("Models\\jungle1"));
            Console.WriteLine(x++ + " loading jungleblock1");
            models.Add("jungle2", c.Load<Model>("Models\\jungle2"));
            Console.WriteLine(x++ + " loading jungleblock2");
            models.Add("jungle3", c.Load<Model>("Models\\jungle3"));
            Console.WriteLine(x++ + " loading jungleblock3");
            models.Add("spikebase", c.Load<Model>("Models\\spikebase"));
            Console.WriteLine(x++ + " loading spikebase");
            models.Add("tree", c.Load<Model>("Models\\tree"));
            Console.WriteLine(x++ + " loading tree");
            models.Add("treebase", c.Load<Model>("Models\\treebase"));
            Console.WriteLine(x++ + " loading treebase");
            models.Add("treetop", c.Load<Model>("Models\\treetop"));
            Console.WriteLine(x++ + " loading treetop");
            models.Add("bridge", c.Load<Model>("Models\\bridge"));
            Console.WriteLine(x++ + " loading bridge");
            models.Add("woodstairs", c.Load<Model>("Models\\woodstairs"));
            Console.WriteLine(x++ + " loading woodstairs");
            models.Add("bars", c.Load<Model>("Models\\bars"));
            Console.WriteLine(x++ + " loading bars");

            models.Add("regal", c.Load<Model>("Models\\regal"));
            Console.WriteLine(x++ + " loading regal");
            models.Add("regalcar", c.Load<Model>("Models\\regalcar"));
            Console.WriteLine(x++ + " loading regalcar");
            models.Add("regalstairs", c.Load<Model>("Models\\regalstairs"));
            Console.WriteLine(x++ + " loading regalstairs");
            models.Add("regalstairscarpet", c.Load<Model>("Models\\regalstairscarpet"));
            Console.WriteLine(x++ + " loading regalstairscarpet");
            models.Add("dude", c.Load<Model>("Models\\dude"));
            Console.WriteLine(x++ + " loading dude");
            models.Add("crossbow", c.Load<Model>("Models\\crosbow"));
            Console.WriteLine(x++ + " loading crossbow");
            models.Add("lessknight", c.Load<Model>("Models\\lessknight"));
            Console.WriteLine(x++ + " loading lessknight");
            models.Add("troll", c.Load<Model>("Models\\troll"));
            Console.WriteLine(x++ + " loading troll");

            
            sprites.Add("fire", c.Load<Texture2D>("Sprites\\fire"));
            Console.WriteLine(x++ + " loading fire");
            sprites.Add("health", c.Load<Texture2D>("Sprites\\health"));
            Console.WriteLine(x++ + " loading health");
            sprites.Add("LOGOscreen", c.Load<Texture2D>("Sprites\\LOGOscreen"));
            Console.WriteLine(x++ + " loading logo");
            sprites.Add("test", c.Load<Texture2D>("Sprites\\test"));
            Console.WriteLine(x++ + " loading test");
            sprites.Add("TITLEscreen", c.Load<Texture2D>("Sprites\\TITLEscreen"));
            Console.WriteLine(x++ + " loading title");
            sprites.Add("metal", c.Load<Texture2D>("Sprites\\metal"));
            Console.WriteLine(x++ + " loading metal");
            sprites.Add("firemetre", c.Load<Texture2D>("Sprites\\firemetre"));
            Console.WriteLine(x++ + " loading firemeter");
            sprites.Add("gone", c.Load<Texture2D>("Sprites\\gone"));
            Console.WriteLine(x++ + " loading gone");
            sprites.Add("gtwo", c.Load<Texture2D>("Sprites\\gtwo"));
            Console.WriteLine(x++ + " loading gtwo");
            sprites.Add("gthree", c.Load<Texture2D>("Sprites\\gthree"));
            Console.WriteLine(x++ + " loading gthree");
            sprites.Add("c1a", c.Load<Texture2D>("Sprites\\c1a"));
            Console.WriteLine(x++ + " loading c1a");
            sprites.Add("c1b", c.Load<Texture2D>("Sprites\\c1b"));
            Console.WriteLine(x++ + " loading c1b");
            sprites.Add("c1c", c.Load<Texture2D>("Sprites\\c1c"));
            Console.WriteLine(x++ + " loading c1c");
            sprites.Add("c1d", c.Load<Texture2D>("Sprites\\c1d"));
            Console.WriteLine(x++ + " loading c1d");
            sprites.Add("c2a", c.Load<Texture2D>("Sprites\\c2a"));
            Console.WriteLine(x++ + " loading c2a");
            sprites.Add("c2b", c.Load<Texture2D>("Sprites\\c2b"));
            Console.WriteLine(x++ + " loading c2b");
            sprites.Add("c2c", c.Load<Texture2D>("Sprites\\c2c"));
            Console.WriteLine(x++ + " loading c2c");
            sprites.Add("c2d", c.Load<Texture2D>("Sprites\\c2d"));
            Console.WriteLine(x++ + " loading c2d");
            sprites.Add("c3a", c.Load<Texture2D>("Sprites\\c3a"));
            Console.WriteLine(x++ + " loading c3a");
            sprites.Add("c3b", c.Load<Texture2D>("Sprites\\c3b"));
            Console.WriteLine(x++ + " loading c3b");
            sprites.Add("c3c", c.Load<Texture2D>("Sprites\\c3c"));
            Console.WriteLine(x++ + " loading c3c");
            sprites.Add("c4a", c.Load<Texture2D>("Sprites\\c4a"));
            Console.WriteLine(x++ + " loading c4a");
            sprites.Add("c4b", c.Load<Texture2D>("Sprites\\c4b"));
            Console.WriteLine(x++ + " loading c4b");
            sprites.Add("c5a", c.Load<Texture2D>("Sprites\\c5a"));
            Console.WriteLine(x++ + " loading c5a");
            sprites.Add("c5b", c.Load<Texture2D>("Sprites\\c5b"));
            Console.WriteLine(x++ + " loading c5b");
            sprites.Add("c6a", c.Load<Texture2D>("Sprites\\c6a"));
            Console.WriteLine(x++ + " loading c6a");
            sprites.Add("c6b", c.Load<Texture2D>("Sprites\\c6b"));
            Console.WriteLine(x++ + " loading c6b");
            sprites.Add("c6c", c.Load<Texture2D>("Sprites\\c6c"));
            Console.WriteLine(x++ + " loading c6c");
            sprites.Add("demo", c.Load<Texture2D>("Sprites\\demo"));
            Console.WriteLine(x++ + " loading demo");
            sprites.Add("demo1", c.Load<Texture2D>("Sprites\\demo1"));
            Console.WriteLine(x++ + " loading demo1");
            sprites.Add("demo2", c.Load<Texture2D>("Sprites\\demo2"));
            Console.WriteLine(x++ + " loading demo2");
            sprites.Add("demo3", c.Load<Texture2D>("Sprites\\demo3"));
            Console.WriteLine(x++ + " loading demo3");
            sprites.Add("demo4", c.Load<Texture2D>("Sprites\\demo4"));
            Console.WriteLine(x++ + " loading demo4");
            sprites.Add("demo5", c.Load<Texture2D>("Sprites\\demo5"));
            Console.WriteLine(x++ + " loading demo5");
            sprites.Add("CREDITS", c.Load<Texture2D>("Sprites\\CREDITS"));
            Console.WriteLine("Content Loaded");

            GameBase.Audio.LoadSong("main_menu", ".\\Audio\\main_menu");
            GameBase.Audio.LoadSound("menu_select", ".\\Audio\\menu_select");
            GameBase.Audio.LoadSound("menu_click", ".\\Audio\\menu_click");
            GameBase.Audio.LoadSound("menu_back", ".\\Audio\\menu_back");
            GameBase.Audio.LoadSound("dead_music", ".\\Audio\\dead_music");
            GameBase.Audio.LoadSong("level1_music", ".\\Audio\\level1_music");
            GameBase.Audio.LoadSong("level5_music", ".\\Audio\\level5_music");
            GameBase.Audio.LoadSong("final_boss", ".\\Audio\\final_boss");
        }
    }
}

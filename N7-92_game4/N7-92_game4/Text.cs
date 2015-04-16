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

namespace N7_92_game4
{

    public class Text
    {
        private Vector2 loc = new Vector2(0, 0);
        String s = "";

        Color c = Color.Black;
        public static SpriteFont font;

        public Text(String a)
        {
            s = a;

        }
        public Text(String a, Vector2 xy)
        {
            s = a;
            loc = xy;

        }
        public Text(String a, float x, float y)
        {
            s = a;
            loc.X = x;
            loc.Y = y;

        }
        public void Lo(ContentManager Content)
        {

            font = ContentClass.Fonts["myFont"];
        }
        public void Lo(ContentManager Content, String f)
        {
            font = Content.Load<SpriteFont>(f);
        }
        public void draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.DrawString(font, s, loc, c);
        }



    }

}

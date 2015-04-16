using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace N7_92_game4
{
    public class DeadMenu : Menu
    {
        SpriteFont titleFont;

        public DeadMenu()
        {

        }
        public void Load()
        {
            titleFont = ContentClass.Fonts["largeFont"];
            menuFont = ContentClass.Fonts["myFont"];
            menuItems = new List<string> { "Restart", "Main Menu" };
            MeasureMenu();
        }
        public override void Select()
        {
            switch (selectedIndex)
            {
                default:
                    break;
                case 0:
                    GameBase.RestartLevel();
                    GameBase.camera.Reset();
                    GameBase.Audio.RestartSong();
                    GameBase.Audio.Enabled = true;
                    GameBase.Audio.ChangeEnabled();
                    Level.JustDied = true;
                    break;
                case 1:
                    GameBase.Audio.StopSong();
                    GameBase.Audio.Enabled = true;
                    GameBase.EndLevel();
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            Byte transparency_amount = 100; //0 transparent; 255 opaque
            Texture2D texture = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] c = new Color[1];
            c[0] = Color.FromNonPremultiplied(255, 255, 255, transparency_amount);
            texture.SetData<Color>(c);

            Rectangle rectangle = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            spriteBatch.Draw(texture, rectangle, Color.Black);

            spriteBatch.DrawString(titleFont, "YOU ARE DEAD",
                new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - (titleFont.MeasureString("YOU ARE DEAD").X / 2), 100),
                Color.Red);

            base.Draw(spriteBatch);
        }
    }
}

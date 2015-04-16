using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace N7_92_game4
{
    public class OptionsMenu
    {
        #region Variables
        Game game;

        SpriteBatch spriteBatch;
        SpriteFont smallFont, largeFont;
        string title;

        bool soundStatus;
        int selectedIndex = 0;

        KeyboardState keyboard, lastKeyboard;
        GamePadState gamepad, lastGamepad;
        #endregion

        public OptionsMenu(Game game)
        {
            this.game = game;
            smallFont = ContentClass.Fonts["myFont"];
            largeFont = ContentClass.Fonts["largeFont"];
            title = "Options";
            soundStatus = true;
        }

        public void Update(GameTime gameTime)
        {
            keyboard = Keyboard.GetState();
            gamepad = GamePad.GetState(PlayerIndex.One);

            if ((keyboard.IsKeyUp(Keys.Down) && lastKeyboard.IsKeyDown(Keys.Down))
                || (gamepad.IsButtonUp(Buttons.DPadDown) && lastGamepad.IsButtonDown(Buttons.DPadDown))
                || (gamepad.IsButtonUp(Buttons.LeftThumbstickDown) && lastGamepad.IsButtonDown(Buttons.LeftThumbstickDown)))
            {
                selectedIndex++;
                GameBase.Audio.PlaySound("menu_click");
                if (selectedIndex == 3)
                    selectedIndex = 0;
            }
            if ((keyboard.IsKeyUp(Keys.Up) && lastKeyboard.IsKeyDown(Keys.Up))
                || (gamepad.IsButtonUp(Buttons.DPadUp) && lastGamepad.IsButtonDown(Buttons.DPadUp))
                || (gamepad.IsButtonUp(Buttons.LeftThumbstickUp) && lastGamepad.IsButtonDown(Buttons.LeftThumbstickUp)))
            {
                selectedIndex--;
                GameBase.Audio.PlaySound("menu_click");
                if (selectedIndex < 0)
                    selectedIndex = 2;
            }

            // Enable/Disable volume
            if (((keyboard.IsKeyDown(Keys.Right) && !lastKeyboard.IsKeyDown(Keys.Right))
                || (keyboard.IsKeyDown(Keys.Left) && !lastKeyboard.IsKeyDown(Keys.Left))
                || (gamepad.IsButtonUp(Buttons.DPadLeft) && lastGamepad.IsButtonDown(Buttons.DPadLeft))
                || (gamepad.IsButtonUp(Buttons.DPadRight) && lastGamepad.IsButtonDown(Buttons.DPadRight)))
                && selectedIndex == 0)
            {
                GameBase.Audio.DisableSound = !GameBase.Audio.DisableSound;
                if (!GameBase.Audio.Enabled)
                {
                    GameBase.Audio.Enabled = true;
                    GameBase.Audio.ChangeEnabled();
                }
            }

            // Adjust Music Volumes
            if ((keyboard.IsKeyDown(Keys.Right) || gamepad.IsButtonDown(Buttons.LeftThumbstickRight)
                || gamepad.IsButtonDown(Buttons.DPadRight)) && selectedIndex == 1)
            {
                if (MediaPlayer.Volume < 1f)
                    MediaPlayer.Volume += 0.003f;
            }
            if ((keyboard.IsKeyDown(Keys.Left) || gamepad.IsButtonDown(Buttons.LeftThumbstickLeft)
                || gamepad.IsButtonDown(Buttons.DPadLeft)) && selectedIndex == 1)
            {
                if (MediaPlayer.Volume > 0f)
                    MediaPlayer.Volume -= 0.003f;
            }

            // Adjust Sound Volumes
            if ((keyboard.IsKeyDown(Keys.Right) || gamepad.IsButtonDown(Buttons.LeftThumbstickRight)
                || gamepad.IsButtonDown(Buttons.DPadRight)) && selectedIndex == 2)
            {
                if (SoundEffect.MasterVolume >= 0.997f)
                    SoundEffect.MasterVolume = 1f;
                else
                    SoundEffect.MasterVolume += 0.003f;
            }
            if ((keyboard.IsKeyDown(Keys.Left) || gamepad.IsButtonDown(Buttons.LeftThumbstickLeft)
                || gamepad.IsButtonDown(Buttons.DPadLeft)) && selectedIndex == 2)
            {
                if (SoundEffect.MasterVolume <= 0.003f)
                    SoundEffect.MasterVolume = 0f;
                else
                    SoundEffect.MasterVolume -= 0.003f;
            }

            lastKeyboard = keyboard;
            lastGamepad = gamepad;
        }

        public void Draw(SpriteBatch sBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch = sBatch;
            int height = 150;
            Texture2D pixel = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });

            DrawString(largeFont, title,
                    new Vector2(400 - (largeFont.MeasureString("Options").X / 2), height),
                    Color.Red);

            // ENABLED SOUND
            height += (int)largeFont.MeasureString("M").Y;
            if (selectedIndex == 0)
            {
                Rectangle highlight = new Rectangle(275, height - 1, (int)smallFont.MeasureString("Enable Sound").X + 10, (int)smallFont.MeasureString("M").Y);
                spriteBatch.Draw(pixel, highlight, Color.IndianRed);
            }
            DrawString(smallFont, "Enable Sound", new Vector2(280, height), Color.White);
            if (!GameBase.Audio.DisableSound)
                DrawString(smallFont, "Yes", new Vector2(430, height), Color.Green);
            else
                DrawString(smallFont, "No", new Vector2(430, height), Color.Red);

            height += (int)smallFont.MeasureString("M").Y;
            DrawString(smallFont, "Volume", new Vector2(280, height), Color.White);

            // MUSIC VOL
            height += (int)smallFont.MeasureString("M").Y;
            if (selectedIndex == 1)
            {
                Rectangle highlight = new Rectangle(295, height - 1, (int)smallFont.MeasureString("Music").X + 10, (int)smallFont.MeasureString("M").Y);
                spriteBatch.Draw(pixel, highlight, Color.IndianRed);
            }
            DrawString(smallFont, "Music", new Vector2(300, height), Color.White);
            Rectangle r_musicVol_border = new Rectangle(429, height + 2, 102, 22);
            DrawRectangleBorder(spriteBatch, graphics, r_musicVol_border, 1, Color.Black);
            Rectangle r_musicVol = new Rectangle(430, height + 3, (int)(MediaPlayer.Volume * 100), 20);
            spriteBatch.Draw(pixel, r_musicVol, Color.IndianRed);
            DrawString(smallFont, "" + (int)(MediaPlayer.Volume * 100), new Vector2(545, height), Color.White);

            // SOUND VOL
            height += (int)smallFont.MeasureString("M").Y;
            if (selectedIndex == 2)
            {
                Rectangle highlight = new Rectangle(295, height - 1, (int)smallFont.MeasureString("Sound").X + 10, (int)smallFont.MeasureString("M").Y);
                spriteBatch.Draw(pixel, highlight, Color.IndianRed);
            }
            DrawString(smallFont, "Sound", new Vector2(300, height), Color.White);
            Rectangle r_soundVol_border = new Rectangle(429, height + 2, 102, 22);
            DrawRectangleBorder(spriteBatch, graphics, r_soundVol_border, 1, Color.Black);
            Rectangle r_soundVol = new Rectangle(430, height + 3, (int)(SoundEffect.MasterVolume * 100), 20);
            spriteBatch.Draw(pixel, r_soundVol, Color.IndianRed);
            DrawString(smallFont, "" + (int)(SoundEffect.MasterVolume * 100), new Vector2(545, height), Color.White);
        }

        private void DrawRectangleBorder(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Rectangle rectangle, int thickness, Color color)
        {
            Texture2D pixel = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });

            // top line
            spriteBatch.Draw(pixel, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);

            // left line
            spriteBatch.Draw(pixel, new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);

            // right line
            spriteBatch.Draw(pixel, new Rectangle((rectangle.X + rectangle.Width - thickness),
                                            rectangle.Y, thickness, rectangle.Height), color);
            // bottom line
            spriteBatch.Draw(pixel, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - thickness,
                                            rectangle.Width, thickness), color);
        }

        protected void DrawString(SpriteFont font, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, text, new Vector2(position.X - 1, position.Y), Color.Black);
            spriteBatch.DrawString(font, text, new Vector2(position.X + 1, position.Y), Color.Black);
            spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y - 1), Color.Black);
            spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y + 1), Color.Black);
            spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y), color);
        }
    }
}

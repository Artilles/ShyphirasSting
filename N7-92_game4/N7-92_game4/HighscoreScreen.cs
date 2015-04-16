using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace N7_92_game4
{
    public class HighscoreScreen
    {
        #region Variables
        Game game;
        SpriteBatch spriteBatch;
        SpriteFont smallFont, largeFont;
        string title;

        KeyboardState keyboard, lastKeyboard;
        GamePadState gamepad, lastGamepad;
        #endregion

        public HighscoreScreen(Game game)
        {
            this.game = game;
            smallFont = ContentClass.Fonts["myFont"];
            largeFont = ContentClass.Fonts["largeFont"];
            title = "Highscores";
        }

        public void Update(GameTime gameTime)
        {
            keyboard = Keyboard.GetState();
            gamepad = GamePad.GetState(PlayerIndex.One);

            lastKeyboard = keyboard;
            lastGamepad = gamepad;
        }

        public void Draw(SpriteBatch sBatch, GraphicsDeviceManager graphics)
        {
            this.spriteBatch = sBatch;

            DrawString(largeFont, title,
                    new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - (largeFont.MeasureString("Highscores").X / 2), 150),
                    Color.Red);

            int heightMod = 150 + (int)largeFont.MeasureString("M").Y;
            int level = 1;
            foreach (int highscore in GameBase.highscores)
            {
                DrawString(smallFont,
                    "Level " + level + ": " + highscore,
                    new Vector2(
                        (graphics.GraphicsDevice.Viewport.Width / 2) - (smallFont.MeasureString("Level " + level + ": " + highscore).X / 2),
                        heightMod), Color.White);
                heightMod += (int)(smallFont.LineSpacing);
                level++;
            }
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

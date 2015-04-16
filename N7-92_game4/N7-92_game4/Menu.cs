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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Menu
    {
        public List<string> menuItems;
        public int selectedIndex;

        public KeyboardState keyboardState, lastKeyboardState;
        public GamePadState gamepadState, lastGamepadState;

        public SpriteBatch spriteBatch;
        public SpriteFont menuFont;

        public Vector2 position;
        public float width = 0f;
        public float height = 0f;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                if (selectedIndex < 0)
                    selectedIndex = 0;
                if (selectedIndex > menuItems.Count - 1)
                    selectedIndex = menuItems.Count - 1;
            }
        }

        public Menu()
        {
        }

        public void MeasureMenu()
        {
            menuFont = ContentClass.Fonts["myFont"];
            height = 0;
            width = 0;

            foreach (string item in menuItems)
            {
                Vector2 size = menuFont.MeasureString(item);
                if (size.X > width)
                    width = size.X;
                height += menuFont.LineSpacing + 5;
            }

            position = new Vector2(
                (GameBase.ScreenSize.X) / 2,
                (GameBase.ScreenSize.X/2 - height) / 2);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            gamepadState = GamePad.GetState(PlayerIndex.One);
            keyboardState = Keyboard.GetState();

            if ((keyboardState.IsKeyUp(Keys.Down) && lastKeyboardState.IsKeyDown(Keys.Down))
                || (gamepadState.IsButtonUp(Buttons.DPadDown) && lastGamepadState.IsButtonDown(Buttons.DPadDown))
                || (gamepadState.IsButtonUp(Buttons.LeftThumbstickDown) && lastGamepadState.IsButtonDown(Buttons.LeftThumbstickDown)))
            {
                selectedIndex++;
                if (selectedIndex == menuItems.Count)
                    selectedIndex = 0;
                GameBase.Audio.PlaySound("menu_click");
            }
            if ((keyboardState.IsKeyUp(Keys.Up) && lastKeyboardState.IsKeyDown(Keys.Up)) 
                || (gamepadState.IsButtonUp(Buttons.DPadUp) && lastGamepadState.IsButtonDown(Buttons.DPadUp))
                || (gamepadState.IsButtonUp(Buttons.LeftThumbstickUp) && lastGamepadState.IsButtonDown(Buttons.LeftThumbstickUp)))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                    selectedIndex = menuItems.Count - 1;
                GameBase.Audio.PlaySound("menu_click");
            }

            if ((keyboardState.IsKeyDown(Keys.Enter) && !lastKeyboardState.IsKeyDown(Keys.Enter))
                || (gamepadState.IsButtonDown(Buttons.A) && !lastGamepadState.IsButtonDown(Buttons.A)))
            {
                Select();
                GameBase.Audio.PlaySound("menu_select");
            }

            lastGamepadState = gamepadState;
            lastKeyboardState = keyboardState;            
        }
            
        public virtual void Select()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 location = position;
            location.Y += 50;
            Color tint;

            for (int i = 0; i < menuItems.Count; i++)
            {
                if (i == selectedIndex)
                    tint = Color.Red;
                else
                    tint = Color.White;

                Vector2 stringSize = menuFont.MeasureString(menuItems[i]);
                location.X = location.X - (stringSize.X / 2);

                // Draw 5 strings, 4 to add the black outline
                spriteBatch.DrawString(
                    menuFont,
                    menuItems[i],
                    new Vector2(location.X - 1, location.Y),
                    Color.Black);
                spriteBatch.DrawString(
                    menuFont,
                    menuItems[i],
                    new Vector2(location.X + 1, location.Y),
                    Color.Black);
                spriteBatch.DrawString(
                    menuFont,
                    menuItems[i],
                    new Vector2(location.X, location.Y - 1),
                    Color.Black);
                spriteBatch.DrawString(
                    menuFont,
                    menuItems[i],
                    new Vector2(location.X, location.Y + 1),
                    Color.Black);
                spriteBatch.DrawString(
                    menuFont,
                    menuItems[i],
                    location,
                    tint);
                    
                location.Y += menuFont.LineSpacing + 5;
                location.X = position.X;                
            }
        }
    }
}

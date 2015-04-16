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
    public class Sprite
    {
        
        public bool Visible = true;
        public Matrix transform;
        public Rectangle rectangle;
        public Vector2 centre;
        public int height, width;
        public Random r = new Random();
        public Random b = new Random();
    
        //The current position of the Sprite
        public const int TWO = 2;
        public Vector2 Position = new Vector2(0, 0);
        public Vector2 Store = new Vector2(0, 0);
        public float rotation = 0;

        //The texture object used when drawing the sprite
        public Texture2D mSpriteTexture;

        //The asset name for the Sprite's Texture
        public string AssetName;

        //The Size of the Sprite (with scale applied)
        public Rectangle Size;

        //The Rectangular area from the original image that 
        //defines the Sprite. 
        Rectangle mSource;
        Color[] colour;

        public Rectangle Source
        {
            get { return mSource; }
            set
            {
                mSource = value;
                Size = new Rectangle(0, 0, (int)(mSource.Width * Scale), (int)(mSource.Height * Scale));
            }
        }


        //The amount to increase/decrease the size of the original sprite. When
        //modified throught he property, the Size of the sprite is recalculated
        //with the new scale applied.
        private float mScale = 1.0f;
        public float Scale
        {
            get { return mScale; }
            set
            {
                mScale = value;
                //Recalculate the Size of the Sprite with the new scale
                Size = new Rectangle(0, 0, (int)(Source.Width * Scale), (int)(Source.Height * Scale));
            }
        }
        //Constructor to add position
        public Sprite(Double X, Double Y)
        {
            //0, 0
            //lowx, lowy
            //10, 10
            //low
            Position.X = (float)X - GameBase.ScreenSize.X / 2;
            Position.Y = (float) Y +GameBase.ScreenSize.Y/2;
            Store = Position;
            
        }

        public Sprite()
        {

        }


        

        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(string theAssetName)
        {
            mSpriteTexture = ContentClass.sprites[theAssetName];
            AssetName = theAssetName;
            Source = new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height);
            Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));
            height = mSpriteTexture.Height;
            width = mSpriteTexture.Width;
            centre = new Vector2(width / TWO, height / TWO);
            colour = new Color[width * height];
            mSpriteTexture.GetData(colour);
            setAbsolutePosition();
        }
        public void update(Vector3 previous, Vector3 now)
        {
            Position.X += MathHelper.ToDegrees(now.X) - MathHelper.ToDegrees(previous.X);// MathHelper.ToDegrees(now.X - previous.X);
            //Position.Y -= MathHelper.ToDegrees(now.Y - previous.Y);
        }
        public void update()
        {
            Position.X -= MathHelper.ToDegrees(GameBase.camera.change.X);
            Position.Y += MathHelper.ToDegrees(GameBase.camera.change.Y);
        }
        public void update(int x)
        {
            if (x == 0)
            {
                Position.X -= MathHelper.ToDegrees(GameBase.camera.change.X);
            }
            else
            {
                Position.Y += MathHelper.ToDegrees(GameBase.camera.change.Y);
            }
        }

        //Update the Sprite and change it's position based on the passed in speed, direction and elapsed time.
        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            if (Visible)
            {
                Position += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        //Draw the sprite to the screen
        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            if (Visible)
            {

                  theSpriteBatch.Draw(mSpriteTexture, Position, Source,
                    Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            }
        }

        public void setAbsolutePosition()
        {
            Position.X += GameBase.ScreenSize.X / 2;
            Position.X -= width / 2;
            Position.Y += GameBase.ScreenSize.Y / 2;
            Position.Y -= height / 2;
        }
        public Vector2 getAbsolutePosition()
        {
            Vector2 v = new Vector2(Position.X + (GameBase.ScreenSize.X / 2) - (width / 2), Position.Y + (GameBase.ScreenSize.Y / 2) - (height / 2));
            return v;
        }


    }
    
}

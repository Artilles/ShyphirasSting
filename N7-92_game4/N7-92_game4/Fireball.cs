using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace N7_92_game4
{
    class Fireball : Entity
    {
        const int MAX_DISTANCE = 1280;
        

        public Vector2 mStartPosition;
        public Vector2 mSpeed = new Vector2(20, 20);
        public Vector2 iSpeed = new Vector2(20, 20);
        public Vector2 mDirection;
        public Vector2 mSpeedDefault = new Vector2(200, 200);
        public Fireball()
        {
            size = new Vector3(3, 3, 1);
        }
        public Fireball(Vector2 p, Vector2 s, Vector2 d)
        {
        }
        public void LoadContent()
        {

            model = ContentClass.models["fireball"];
        }

        public void Update(GameTime gametime, Vector3 n)
        {
            if (true)
            {
            view = Matrix.CreateLookAt(GameBase.camera.current, GameBase.camera.target, Vector3.UnitY);
            position.X += MathHelper.ToRadians(mSpeed.X);
            angle.Y = MathHelper.ToRadians(90);
            world = Matrix.CreateScale(size.X, size.Y, 2) * Matrix.CreateRotationX(angle.X) * Matrix.CreateRotationY(angle.Y) * Matrix.CreateRotationZ(angle.Z) * Matrix.CreateTranslation(position);
            base.Update(world);// world *= Matrix.CreateScale(3, 6, 0);
            }
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            if (Visible == true)
            {
                base.Draw(theSpriteBatch);
            }
        }

        public void Fire(Vector2 theStartPosition, Vector2 theSpeed, Vector2 theDirection, Vector2 startSpeed)
        {
            
            position = new Vector3(theStartPosition.X,theStartPosition.Y, 0) ;
            mStartPosition = new Vector2(theStartPosition.X, theStartPosition.Y);         
            mDirection = theDirection;

            //face = -1
            if(theDirection.X != -1)
            {
                mSpeed = startSpeed + theSpeed;
            }
            else
            {
                mSpeed = startSpeed + (theSpeed*-1);
            }
            //setAbsolutePosition();
            Visible = true;
        }
    }
    
}

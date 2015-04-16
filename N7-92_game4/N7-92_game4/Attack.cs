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
    public class Attack : Entity
    {

        public Boolean modd = false;
        int range = 0;
        float dir = 0;
        int ro;
        int count;
        public int deeps;
        Vector2 hitbox;
        Vector3 start;
        Rectangle r;
        public string modelstr;
        public Attack(Vector3 v, Vector2 si, string _modelName, int _range, float speed, Boolean g, float direction, int rotate, int damage)
        {
            
            position.X = v.X;
            position.Y = v.Y;
            start = position; 
            mSpeed.X =speed;
            deeps = damage;
            if (speed > 0)
            {
                angle.Z = direction;
                float soh = speed * (float)Math.Sin((double)direction);
                float cah = speed * (float)Math.Cos((double)direction);
                mSpeed.X = cah;
                mSpeed.Y = soh;
            }
            dir = direction;
             range = _range;
            size = new Vector3(1, 1, 1);
           

            ro = rotate;
            try
            {
                modelstr = _modelName;
                model = ContentClass.models[_modelName];
                modd = true;
            }
            catch (Exception C)
            {
                modd = false;
            }
            grav = g;
            horizontalSpeed = speed;
            hitbox = si;
          //  mSpeed.Y = .001f;
        }
        public void Update()
        {

          
            //add movment
            view = Matrix.CreateLookAt(GameBase.camera.current, GameBase.camera.target, Vector3.UnitY);
            position.X += mSpeed.X;
            
            if (grav)
            {
                mSpeed.X -= .00098f;
                if (angle.Z > 0)
                {
                    angle.Z-=.0098f;
                }
                else if (angle.Z < 0)
                {
                    angle.Z += .0098f;
                }
            }
            position.Y += mSpeed.Y;
          
            angle.Y = dir;// MathHelper.ToRadians(ro);
           
            count++;
            if (count >= range)
            {
                Visible = false;
            }
            world = Matrix.CreateScale(size.X, size.Y, 2) * Matrix.CreateRotationX(angle.X) * Matrix.CreateRotationY(angle.Y) * Matrix.CreateRotationZ(angle.Z) * Matrix.CreateTranslation(position);
            if(modd)
            {
                base.Update(world);
            }
            else
            {
                boundbox();
                base.UpdateBoundless(world);
            }
        }
        public void boundbox()
        {
            int minx = (int)MathHelper.ToDegrees(position.X);
            int maxx = (int)MathHelper.ToDegrees(position.X) + (int)hitbox.X; ;
            

            
            int miny = (int)MathHelper.ToDegrees(position.Y) - (int)hitbox.Y / 2;
            int maxy = (int)MathHelper.ToDegrees(position.Y) + (int)hitbox.Y / 2;
            int maxz = 1;
            int minz = -1;

           
                if (dir < MathHelper.ToRadians(270) && dir > MathHelper.ToRadians(90))
                {
                    int wid = maxx - minx;
                    maxx = minx;
                    
                    minx -= wid;
                   // maxx = (int)MathHelper.ToDegrees(position.X);
                }
            
            bounds = new BoundingBox(new Vector3(MathHelper.ToRadians(minx), MathHelper.ToRadians(miny), MathHelper.ToRadians(minz)),
                new Vector3(MathHelper.ToRadians(maxx), MathHelper.ToRadians(maxy), MathHelper.ToRadians(maxz)));


        }
        public void Draw(SpriteBatch s)
        {
            if (modd)
            {
                base.Draw(s);
            }
            else
            {
                base.DrawBound(s);
            }
        }



    }
        
}

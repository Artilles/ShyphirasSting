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
    class Player : Entity
    {
        #region Variables
        public float aim = 0f;
        public Sprite ret;
        Boolean smooth = true;
        private float jumpstart;
        private float flystart;
        private float jumppos;
        private float momentum;
        private int jumpCut;
        private float maxHSpeed;
        private Boolean isJumping = false;
        private Boolean isFalling = false;
        private Boolean isFlying = false;
        private Boolean flap = false;
        public int totalfire;
        Random r = new Random();
        int debug = 0;
        int flygrav;
        int maxFlaps;
        int flapCount;
        int time = 0;
        int face = 0;
        
        public Vector2 direction;
        public Boolean grounded = true;
        private Boolean jumped = false;

        public float fall = 0f;

        public List<Fireball> mFireball = new List<Fireball>();
        public List<Attack> attacks;

        Texture2D mHealthBar;
        Texture2D mFireBar;
        int count = 0;
        public Boolean dead = false;
        public int fireballDamage;
        public static Vector3 playerPosition;
        public static Vector2 playerSpeed;
        public int fire;
        public int flamecost;
        Vector3 head;
        #endregion

        public void Initialize(Model model)
        {
            type = "dragon";
            face = 1;
            attacks = new List<Attack>();
            gravity = 0.0098f;
            jumpstart = 0.20f;
            flystart = 0.120f;
            horizontalSpeed = 0.15f;
            hAccel = .013f;
            vAccel = .04f;
            jumppos = 0f;
            momentum = 0f;
            jumpCut = 2;
            flygrav = 2;
            maxFlaps = 500;
            flapCount = 0;
            direction = Vector2.Zero;
            Height = 90/2;
            Width = 115/2;
            health = 100 ;
            maxHSpeed = 0.15f;
            fall = 10f;
            fire = 0;
            totalfire = 360;
            rectangle = new Rectangle((int)GetCoords().X, (int)GetCoords().Y, Height, Width);
            ret = new Sprite();
            angle.Y = MathHelper.ToRadians(-90);
            angle.Z = MathHelper.ToRadians(0);
            //position.Y = MathHelper.ToRadians(100);
            ret.LoadContent("fire");
            mHealthBar = ContentClass.sprites["health"];
            mFireBar = ContentClass.sprites["firemetre"];
           // setAbsolutePosition();
           //
            //Player.playerPosition = new Vector3(0, 0, 0);
            Player.playerSpeed = new Vector2(0, 0);
            fireballDamage = 10;
            fire = 0;
            flamecost = 0;
            switch (GameBase.difficulty)
            {
                // Blank space
                case 0:
                    flamecost = 1;
                    maxFlaps = 20;
                    break;
                case 1:
                    flamecost = 2;
                    maxFlaps = 15;
                    break;
                case 2:
                    flamecost = 3;
                    maxFlaps = 10;
                    break;
                case 3: ;
                    flamecost = 5;
                    maxFlaps = 5;
                    break;
            }
            GameBase.camera.Reset();
            base.Initialize(model);
        }
        
        
        public void myMove()
        {
            jumpCut = 1;
            #region Ares Modified 12.10
           /* if (position.Y <= 16.44)
            {
                position.Y = 16.45f;
            }*/
            if (!collisionBottom)
            {
            #endregion
                mSpeed.Y -= gravity;

                if (fall < mSpeed.Y)
                {
                    mSpeed.Y = 0;
                }
                if (!flap)
                {
                    jumpCut = 2;
                }
            }
            else
            {
                jumped = false;
                flapCount = 0;
            }
            
            #region horizontal
            if (keyboardState.IsKeyDown(Keys.Right)
                || player1.IsButtonDown(Buttons.LeftThumbstickRight))
            {
                face = 1;
                if (!collisionRight)
                {
                    if (mSpeed.X < maxHSpeed) //not moving max speed, accelerate
                    {
                        mSpeed.X += hAccel / jumpCut; //if you're in the air, acceleration is cut
                    }
                    else
                    {
                        mSpeed.X = maxHSpeed; //moving max speed
                    }
                }
            }
            else if (keyboardState.IsKeyDown(Keys.Left)
                || player1.IsButtonDown(Buttons.LeftThumbstickLeft))
            {
                face = -1;
                if (!collisionLeft)
                {
                    if (mSpeed.X > -maxHSpeed) //not moving max speed, accelerate
                    {
                        mSpeed.X -= hAccel / jumpCut; //if you're in the air, accleration is cut
                    }
                    else
                    {
                        mSpeed.X = -maxHSpeed; //moving max speed
                    }
                }
            }
            else
            {
                if (collisionBottom)
                {
                    if (mSpeed.X > -threas && mSpeed.X < threas)
                    {
                        mSpeed.X = 0;
                    }
                    else if (mSpeed.X > 0)
                    {
                        if (!collisionRight)
                        {
                            mSpeed.X -= hAccel / jumpCut;
                        }
                        else
                        {
                            mSpeed.X = 0;
                        }
                    }
                    else if (mSpeed.X < 0)
                    {
                        if (!collisionLeft)
                        {
                            mSpeed.X += hAccel / jumpCut;
                        }
                        else
                        {
                            mSpeed.X = 0;
                        }
                    }
                }
            }
            if (mSpeed.X > 0 && collisionRight)
            {
                mSpeed.X = 0;
            }
            if (mSpeed.X < 0 && collisionLeft)
            {
                mSpeed.X = 0;
            }
#endregion
            flap = false;
#region Ares Modified 12.09

       

            if ((keyboardState.IsKeyDown(Keys.Space) && !prevKeyboardState.IsKeyDown(Keys.Space))

#endregion
 || (player1.IsButtonDown(Buttons.RightShoulder) && !prevPlayer1.IsButtonDown(Buttons.RightShoulder))
                 || (player1.IsButtonDown(Buttons.RightStick ) && !prevPlayer1.IsButtonDown(Buttons.RightStick))
                 || (player1.IsButtonDown(Buttons.A) && !prevPlayer1.IsButtonDown(Buttons.A)))
            {
                if (!collisionTop && !jumped)
                {
                    jumped = true;
                    mSpeed.Y = jumpstart;
                }
                else if (flapCount < maxFlaps)
                {
                    flapCount++;
                    mSpeed.Y = flystart;
                    flap = true;
                }
            }
            

            if (position.X + mSpeed.X > Level.hardLeft && position.X + mSpeed.X < Level.hardRight)
                position.X += mSpeed.X;
            //if (position.Y + mSpeed.Y < Level.skyCap)
                position.Y += mSpeed.Y;
           // if (position.Y < Level.floor)
            //    health = 0;

            // set some static info
            Player.playerPosition = position;            
            Player.playerSpeed.X = mSpeed.X;
            Player.playerSpeed.Y = mSpeed.Y;
        }
        public Vector3 Update(GameTime gametime)
        {
            if (health <= 0)
            {
                if (!dead)
                {
                    GameBase.Audio.PlaySound("dead_music");
                }
                dead = true;    
            }
            if (!dead)
            {
                keyboardState = Keyboard.GetState();
                //mSpeed = Vector3.Zero;
                player1 = GamePad.GetState(PlayerIndex.One);
                //mouseState = Mouse.GetState();
                smooth = true;
                getDir();

                if (keyboardState.IsKeyDown(Keys.W))
                {
                    health--;
                }
                if (keyboardState.IsKeyDown(Keys.E))
                {
                    health++;
                }

                if ((keyboardState.IsKeyDown(Keys.S) && !prevKeyboardState.IsKeyDown(Keys.S) && !keyboardState.IsKeyDown(Keys.A))
                    || (player1.IsButtonDown(Buttons.RightTrigger) && !prevPlayer1.IsButtonDown(Buttons.RightTrigger) && !player1.IsButtonDown(Buttons.LeftTrigger))
                    || (player1.IsButtonDown(Buttons.B) && !prevPlayer1.IsButtonDown(Buttons.B) && !player1.IsButtonDown(Buttons.Y)))
                {
                    SpitFire();
                }
                if ((keyboardState.IsKeyDown(Keys.A))
                    || (player1.IsButtonDown(Buttons.LeftTrigger)) || player1.IsButtonDown(Buttons.Y))
                {
                    SpitFire2(totalfire);
                    totalfire -= flamecost;
                }
                else
                {
                    totalfire++;
                }
                if (keyboardState.IsKeyDown(Keys.Q) && !prevKeyboardState.IsKeyDown(Keys.Q)
                    || (player1.IsButtonDown(Buttons.X)) || player1.IsButtonDown(Buttons.LeftStick))
                {
                    melee();
                }

                if (keyboardState.IsKeyDown(Keys.D1) && !prevKeyboardState.IsKeyDown(Keys.D1)
              || (player1.IsButtonDown(Buttons.DPadUp)))
                {
                    position.Y += 0.4f;
                }
                if (keyboardState.IsKeyDown(Keys.D2) && !prevKeyboardState.IsKeyDown(Keys.D2)
              || (player1.IsButtonDown(Buttons.DPadLeft)))
                {
                    position.X -= 0.4f;
                }
              /*  if ((!keyboardState.IsKeyDown(Keys.A) || !player1.IsButtonDown(Buttons.LeftTrigger)) && fire != 0)
                {
                    fire = 0;
                }*/

                for (int i = 0; i < attacks.Count; i++)
                {
                    attacks[i].Update();
                    if (!attacks[i].Visible)
                    {
                        attacks.Remove(attacks[i]);
                        i--;
                    }
                }
               /* if (smooth)
                {
                    newMove();
                }
                else
                {
                    oldMove();
                }*/
                myMove();

                totalfire = (int)MathHelper.Clamp(totalfire, 0, 360);
                health = (int)MathHelper.Clamp(health, 0, 100);

                prevKeyboardState = keyboardState;
                prevPlayer1 = player1;

                if (face == 1)
                {
                    angle.Y += MathHelper.ToRadians(-10);
                }
                else if (face == -1)
                {
                    angle.Y += MathHelper.ToRadians(10);
                }

                int aaa = 10;
                if(keyboardState.IsKeyDown(Keys.LeftControl))
                {
                    aaa = -10;
                }
                if (keyboardState.IsKeyDown(Keys.Z))
                {
                    angle.X += MathHelper.ToRadians(aaa);
                }
                if (keyboardState.IsKeyDown(Keys.X))
                {
                    angle.Y += MathHelper.ToRadians(aaa);
                }
                if (keyboardState.IsKeyDown(Keys.C))
                {
                    angle.Z += MathHelper.ToRadians(aaa);
                }

                // AIMING
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    aim += 4;
                }
                if (player1.ThumbSticks.Right.Y > 0)
                {
                    aim += player1.ThumbSticks.Right.Y * 5f;                   
                }
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    aim -= 4;
                }
                if (player1.ThumbSticks.Right.Y < 0)
                {
                    aim += player1.ThumbSticks.Right.Y * 5f;
                }
                // END AIMING

                aim = MathHelper.Clamp(aim, -70, 70);
                int power = 200;
                //aim = 70;
                float xang = (float)Math.Cos(MathHelper.ToRadians(aim)) * power;
                float yang = (float)Math.Sin(MathHelper.ToRadians(aim)) * power;
                if (face < 0)
                {
                    xang = (float)Math.Cos(MathHelper.ToRadians(180) - MathHelper.ToRadians(aim)) * power;
                }
                head = new Vector3((position.X + 0.8f * face), position.Y, position.Z);
                ret.Position.X = MathHelper.ToDegrees(position.X - GameBase.camera.cameraPosition.X) + GameBase.ScreenSize.X/2 + (xang) + (head.X * face);
                ret.Position.X = MathHelper.Clamp(ret.Position.X, 0, GameBase.ScreenSize.X);
                ret.Position.Y = ((-GameBase.ScreenSize.Y / 2 + MathHelper.ToDegrees(position.Y - GameBase.camera.cameraPosition.Y)) * -1) +(-yang) + head.Y;
               // ret.Position.Y = MathHelper.Clamp(ret.Position.Y, -GameBase.ScreenSize.Y, 0);
                angle.Y = MathHelper.Clamp(angle.Y, MathHelper.ToRadians(-90), MathHelper.ToRadians(90));
                world = Matrix.CreateRotationX(angle.X) * Matrix.CreateRotationY(angle.Y) * Matrix.CreateRotationZ(angle.Z) * Matrix.CreateTranslation(position);
                //Player.playerPosition = position;

                base.Update(world);
            }
            return position;
        }

        public void melee()
        {
            float shoot = 0f;
            if (face == 1)
                shoot = 0;
            else
                shoot = 3.14f;
                //shoot = MathHelper.ToRadians(aim);


            Vector3 head = new Vector3((position.X + 0.9f * face), position.Y, position.Z);
            attacks.Add(new Attack(new Vector3(position.X+.5f, position.Y, position.Z), new Vector2(65, 90), "", 4, 0, false, shoot, 0, 100));
        }
        public Vector2 GetSpeed()
        {
            return new Vector2(mSpeed.X, mSpeed.Y);
        }
        private Vector2 getDir()
        {
            int x, y = 0;
            if (actual.Y > 0)
            {
                y = 1;
            }
            else if (actual.Y < 0)
            {
                y = -1;
            }
            else
            {
                y = 0;
            }
            if (actual.X > 0)
            {
                x = 1;
            }
            else if (actual.X < 0)
            {
                x = -1;
            }
            else
            {
                x = 0;
            }
                
            return new Vector2(x, y);
        }

        private void SpitFire()
        {
            float shoot = 0f;
            if (face >= 0)
            {
                shoot = MathHelper.ToRadians(aim);
            }
            if (face == -1)
            {
                shoot = MathHelper.ToRadians(180)-MathHelper.ToRadians(aim);
            }
           // attacks.Add(new Attack(position, new Vector2(0, 0), "fireball", 120, .9f, false, shoot, 110, fireballDamage));
                
            
            attacks.Add(new Attack(head, new Vector2(0, 0), "fireball", 30, .3f + (mSpeed.X * face), false, shoot, 110, 50 * fireballDamage));            
        }
               
        private void SpitFire2(int count)
        {
            float shoot = 0f;
            if (face >= 0)
            {
                shoot = MathHelper.ToRadians(aim);
            }
            if (face == -1)
            {
                shoot = MathHelper.ToRadians(180) - MathHelper.ToRadians(aim);
            }
            
            Vector3 head = new Vector3((position.X + 0.9f * face), position.Y, position.Z);

            attacks.Add(new Attack(head, new Vector2(0, 0), "fireball", count / 24, .3f + (mSpeed.X * face), false, shoot + ((r.Next(0, 240) - 120)/1000f), 110, fireballDamage));
            attacks.Add(new Attack(head, new Vector2(0, 0), "fireball", count / 24, .27f + (mSpeed.X * face), false, (shoot + ((r.Next(0, 240) - 120)/1000f)), 110, fireballDamage));
            attacks.Add(new Attack(head, new Vector2(0, 0), "fireball", count / 24, .27f + (mSpeed.X * face), false, (shoot + ((r.Next(0, 240) - 120)/1000f)), 110, fireballDamage));
            attacks.Add(new Attack(head, new Vector2(0, 0), "fireball", count / 24, .31f + (mSpeed.X * face), false, (shoot + ((r.Next(0, 240) - 120)/1000f)), 110, fireballDamage));
            attacks.Add(new Attack(head, new Vector2(0, 0), "fireball", count / 24, .28f + (mSpeed.X * face), false, (shoot + ((r.Next(0, 240) - 120)/1000f)), 110, fireballDamage));
            attacks.Add(new Attack(head, new Vector2(0, 0), "fireball", count / 24, .26f + (mSpeed.X * face), false, (shoot + ((r.Next(0, 240) - 120)/1000f)), 110, fireballDamage));

            
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (Attack a in attacks)
            {
                a.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);

            spriteBatch.Begin();
    //        ret.Draw(spriteBatch);
            spriteBatch.Draw(mHealthBar, new Rectangle(Level.w / 2 - mHealthBar.Width / 2,
30, mHealthBar.Width, 44), new Rectangle(0, 45, mHealthBar.Width, 44), Color.Black);

            //Draw the current health level based on the current Health
            spriteBatch.Draw(mHealthBar, new Rectangle(Level.w / 2 - mHealthBar.Width / 2,
                30, (int)(mHealthBar.Width * ((double)health / 100)),
                44), new Rectangle(0, 45, mHealthBar.Width, 44), Color.Blue);

            //Draw the box around the health bar
            spriteBatch.Draw(mHealthBar, new Rectangle(Level.w / 2 - mHealthBar.Width / 2,
                30, mHealthBar.Width, 44), new Rectangle(0, 0, mHealthBar.Width, 44), Color.White);

            //spriteBatch.Draw(mHealthBar, new Rectangle(30, Level.h / 2 - mHealthBar.Width / 2
            //    , 22, mHealthBar.Width), new Rectangle(0, 45, mHealthBar.Width, 44), Color.Black);

            //Draw the current health level based on the current Health
            spriteBatch.Draw(mFireBar, new Rectangle(30,
                Level.h / 1 - mFireBar.Height / 2, 22,
                (int)(mHealthBar.Height * (((double)(totalfire) / 360))-12)), new Rectangle(45, 0,44 , mFireBar.Height), Color.Red);

            //Draw the box around the health bar
            spriteBatch.Draw(mFireBar, new Rectangle(30,
                (Level.h / 1 - mFireBar.Height / 2), 22, 76), new Rectangle(0, 0, 44, mFireBar.Height), Color.Black);
            

            spriteBatch.End();
            base.Draw(spriteBatch);
           // spriteBatch.Begin();
        }


        public void SetGround()
        {
            grounded = true;
        }

        public void SetGround(Boolean a)
        {
            grounded = a;
        }

        public void meleeDamage(int damage)
        {
            health -= damage;
        }
    }
}

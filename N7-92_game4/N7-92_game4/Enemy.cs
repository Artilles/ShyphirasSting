using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using N7_92_game4;

namespace N7_92_game4
{

    public class Enemy : Entity
    {
        #region Variables
        public float speed;
        public Boolean Dead = false;
        public static int id;
        public int defence;
        public Vector4 immunity;
        public Random e = new Random(id);
        //public TileCollision Collision;
        Vector2 moveSpeed = new Vector2(0.05f, 0.1f);
        Vector2 firstPosition = new Vector2(0, 0);
        bool autoRotateOpen = false;
        public int rotSpeed = 6;
        public List<Attack> attacks;
        public Boolean fireball = false;
        public Boolean sword = false;
        public Boolean arrow = false;
        public Vector2 difference;
        public float playerangle;
        public int score;
        public int idt;
        public Boolean boss = false;
        public float chase = 0f;
        #endregion

        public Enemy(String _model)
        {
            type = _model;
            attacks = new List<Attack>();
            model = ContentClass.models[_model];
            immunity = new Vector4(0, 0, 0, 0);
            idt = Enemy.id;

            switch (_model)
            {
                case "archer":
                        rotSpeed = 12;
                        score = 100;
                        defence = 1;
                        arrow = true;
                        autoRotateOpen = true;
                        break;
                case "crossbow":
                        rotSpeed = 22;
                        score = 5000;
                        defence = 4;
                        arrow = true;
                        chase = 5f;
                        hAccel = .10f;
                        maxHSpeed = .10f;
                        speed = .1f;
                        /*   chase = 20f;
                            hAccel = .10f;
                            maxHSpeed = .10f;
                            speed = .05f;
                         */
                        autoRotateOpen = true;
                        boss = true;
                        break;
                case "dude":
                        rotSpeed = 30;
                        score = 10000;
                        defence = 10;
                        arrow = true;
                        chase = 60f;
                        hAccel = .10f;
                        maxHSpeed = .2f;
                        speed = .02f;
                        /*   chase = 20f;
                            hAccel = .10f;
                            maxHSpeed = .10f;
                            speed = .05f;
                         */
                        autoRotateOpen = true;
                        fireball = true;
                        sword = true;
                        boss = true;
                        break;
                case "lessknight":
                        rotSpeed = 20;
                        score = 3000;
                        defence = 5;
                        arrow = false;
                        chase = 10f;
                        hAccel = .5f;
                        maxHSpeed = .30f;
                        speed = .05f;
                        /*   chase = 20f;
                            hAccel = .10f;
                            maxHSpeed = .10f;
                            speed = .05f;
                         */
                        autoRotateOpen = true;
                        sword = true;
                        boss = true;
                        break;
                case "troll":
                        rotSpeed = 5;
                        score = 3000;
                        defence = 15;
                        chase = 5f;
                        hAccel = .1f;
                        maxHSpeed = .10f;
                        speed = .05f;
                    /*   chase = 20f;
                        hAccel = .10f;
                        maxHSpeed = .10f;
                        speed = .05f;
                     */
                        autoRotateOpen = true;
                        sword = true;
                        boss = true;
                        break;
                case "spear":
                        rotSpeed = 12;
                        score = 100;
                        defence = 2;
                        fireball = false;
                        autoRotateOpen = true;
                        sword = true;
                        chase = 10f;
                        hAccel = .02f;
                        maxHSpeed = .10f;
                        break;

                case "turtle":
                        rotSpeed = 2;
                        score = 1000;
                        defence = 5;
                        fireball = true;
                        autoRotateOpen = true;
                        sword = true;
                        immunity.X = 1;
                        boss = true;
                        chase = 20f;
                        hAccel = .10f;
                        maxHSpeed = .10f;
                        speed = .05f;
                       // angle.Y += MathHelper.ToRadians(90);
                        break;
                case "ship":
                    rotSpeed = 2;
                    autoRotateOpen = true;
                    boss = true;
                    break;
                default:
                    break;
            }

            switch (GameBase.difficulty)
            {
                // Blank space
                case 0:
                    chase = chase / 2;
                    defence = (int)(defence * 0.5);
                    // score = score
                    maxHSpeed = maxHSpeed / 2f;
                    break;
                case 1:
                    chase = chase / 1.5f;
                    // defence = defence
                    score = (int)(score * 1.5);
                    maxHSpeed = maxHSpeed / 1.5f;
                    break;
                case 2:
                    // chase = chase
                    defence = defence * 3;
                    score = score * 2;
                    maxHSpeed = maxHSpeed / 1.2f;
                    break;
                case 3:
                    defence = defence * 8;
                    chase = chase * 1.5f;
                    score = (int)(score * 3);
                    maxHSpeed = maxHSpeed * 1.1f;                    
                    break;
            }
            health = 100;
            Enemy.id++;
        }

        public void LoadContent()
        {

            firstPosition.X = position.X;
            firstPosition.Y = position.Y;
        }

        public void Update(GameTime gametime, int time, Vector3 n)
        {
            /*difference.X = Player.playerPosition.X - position.X;
            difference.Y = Player.playerPosition.Y - position.Y;
            playerangle =MathHelper.ToDegrees((float)Math.Atan((double)difference.X /(double)difference.Y));
            if (difference.X < 0 && difference.Y > 0)
            {
                playerangle = 180 + playerangle;
            }
            else if (difference.X < 0 && difference.Y < 0)
            {
                playerangle = 180 + playerangle;
            }
            else if (difference.X > 0 && difference.Y < 0)
            {
                playerangle = 360 + playerangle;
            }*/

            


            if (health <= 0)
            {
                Dead = true;
                Visible = false;
            }
            if (Visible)
            {
                if (fireball)
                {
                    updateFire();
                }
                if (arrow)
                {
                    updateArrow();
                }
                if (sword)
                {
                    updateStrike();
                }
                    UpdateMove();
                if (autoRotateOpen)
                {
                    autoRotate();
                    angle.Y = MathHelper.Clamp(angle.Y, MathHelper.ToRadians(0), MathHelper.ToRadians(180));
                }
                for(int i = 0; i < attacks.Count; i++)
                {
                    attacks[i].Update();
                    if(!attacks[i].Visible)
                    {
                        attacks.Remove(attacks[i]);
                        i--;
                    }
                }
                position.X -= MathHelper.ToRadians(time) * speed;
                world = Matrix.CreateRotationX(angle.X) * Matrix.CreateRotationY(angle.Y) * Matrix.CreateRotationZ(angle.Z) * Matrix.CreateTranslation(position);

                base.Update(world);

            }
        }
        public void UpdateMove()
        {
            float speedf = speed;
            
            difference.X = Player.playerPosition.X - position.X;

            /*if (difference.X > -chase && difference.X < chase)
            {
                if (difference.X < -threas)
                {
                    speedf = -speed;
                }
                if (difference.X < threas && difference.X > -threas)
                {
                    speedf = 0f;
                }
                mSpeed.X += speedf;
            }
            mSpeed.X = MathHelper.Clamp(mSpeed.X, -maxHSpeed, maxHSpeed);
            if (!collisionBottom)
            {
                mSpeed.Y -= .0981f;
            }
            else
            {
            }
            position += mSpeed;*/
            if (!collisionBottom)
            {

                mSpeed.Y -= gravity;
               
            }
            if (difference.X > -chase && difference.X < chase)
            {
                if (difference.X > threas)
                {
                    if (!collisionRight)
                    {
                        if (mSpeed.X < maxHSpeed) //not moving max speed, accelerate
                        {
                            mSpeed.X += hAccel;//if you're in the air, acceleration is cut
                        }
                        else
                        {
                            mSpeed.X = maxHSpeed; //moving max speed
                        }
                    }
                }
                else if (difference.X < -threas)
                {
                    if (!collisionLeft)
                    {
                        if (mSpeed.X > -maxHSpeed) //not moving max speed, accelerate
                        {
                            mSpeed.X -= hAccel;//if you're in the air, accleration is cut
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
                                mSpeed.X -= hAccel; 
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
                                mSpeed.X += hAccel;
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
            }
            if (difference.X < threas && difference.X > -threas)
            {
                speedf = 0f;
            }
            position.X += mSpeed.X;
            position.Y += mSpeed.Y;
        }
        public void Draw(SpriteBatch s)
        {
            foreach (Attack a in attacks)
            {
                a.Draw(s);
            }
            base.Draw(s);
        }
        private void SpitFire()
        {
            float speed = 0.55f;
            
            
            difference.X = Player.playerPosition.X - position.X;
            difference.Y = Player.playerPosition.Y - position.Y;
            double v = speed * speed;
            double x = (double)difference.X;
            double y = (double)difference.Y;
            double g = .0981;
            switch (GameBase.difficulty)
            {
                // Blank space
                case 0:
                    speed = speed / 3;
                    break;
                case 1:
                    speed = speed / 2;
                    break;
                case 2:
                    speed = speed / 1.5f;
                    break;
                case 3:;
                    break;
            }

            if (Math.Sqrt(difference.X * difference.X + difference.Y * difference.Y) < 100)
            {
                //playerangle = (float)Math.Atan2(difference.Y, difference.X);
                playerangle = (float)Math.Atan2(difference.Y + Player.playerSpeed.X, difference.X + Player.playerSpeed.Y);
                //playerangle = (float)Math.Atan((v + Math.Sqrt(v * v - g * (g * x * x + 2 * y * v))) / (g * x));
                int damage = 5;
                if (boss)
                {
                    damage *= 3;
                    speed *= 3;
                }
                attacks.Add(new Attack(position, new Vector2(0, 0), "fireball", 20, speed, false, playerangle, 0, damage));
            }
            
        }
        public void shootArrow()
        {
            float speed = 0.35f;
            
            
            difference.X = Player.playerPosition.X - position.X;
            difference.Y = Player.playerPosition.Y - position.Y;
            double v = speed * speed;
            double x = (double)difference.X;
            double y = (double)difference.Y;
            double g = .0981;
            int damage = 0;
            if (Math.Sqrt(difference.X * difference.X + difference.Y * difference.Y) < 100)
            {
                switch (GameBase.difficulty)
                {
                    // Blank space
                    case 0:
                        damage = 10;
                        speed = speed / 3;
                        break;
                    case 1:
                        damage = 20;
                        speed = speed / 2;
                        break;
                    case 2:
                        damage = 30;
                        speed = speed / 1.5f;
                        break;
                    case 3: ;
                        damage = 40;
                        break;
                }
                //playerangle = (float)Math.Atan2(difference.Y, difference.X);
                playerangle = (float)Math.Atan2(difference.Y + Player.playerSpeed.X, difference.X + Player.playerSpeed.Y);
                //playerangle = (float)Math.Atan((v + Math.Sqrt(v * v - g * (g * x * x + 2 * y * v))) / (g * x));
                if (boss)
                {
                    damage *= 3;
                    speed *= 3;
                }
                attacks.Add(new Attack(new Vector3(position.X, position.Y + 1f, position.Z), new Vector2(0, 0), "arrow", 70, speed, true, playerangle, 0, damage));
            }

        }
        public void strike()
        {
            difference.X = Player.playerPosition.X - position.X;
            difference.Y = Player.playerPosition.Y - position.Y;
            float cxx = (difference.X * difference.X) + (difference.Y * difference.Y);
            float cyy = (float)Math.Sqrt(cxx);
            int damage = 0;
            if (Math.Sqrt(difference.X * difference.X + difference.Y * difference.Y) < 5)
            {
                switch (GameBase.difficulty)
                {
                    // Blank space
                    case 0:
                        damage = 15;
                        //speed = speed / 3;
                        break;
                    case 1:
                        damage = 25;
                       // speed = speed / 2;
                        break;
                    case 2:
                        damage = 35;
                        //speed = speed / 1.5f;
                        break;
                    case 3: ;
                        damage = 60;
                        break;
                }
                if (boss)
                {
                    damage *= 3;
                    speed *= 3;
                }
                attacks.Add(new Attack(position, new Vector2(30, 30), "", 20, 0, false, playerangle, 0, damage));
            }
        }
        public void updateArrow()
        {
            int x = e.Next(-60, 60);
            int blahg = 0;
            switch (GameBase.difficulty)
            {
                // Blank space
                case 0:
                    blahg = 59;
                    break;
                case 1:
                    blahg = 58;
                    break;
                case 2:
                    blahg = 56;
                    break;
                case 3: ;
                     blahg = 55;
                    break;
            }
            if (x >= blahg)
            {
                shootArrow();
            }
        }
        private void updateFire()
        {
            int x = e.Next(0, 120)-59;
            if (x >= 50)
            {
                SpitFire();
            }
        }
        public void updateStrike()
        {            
            int x = e.Next(0, 40);
            if (x % 40 ==0 && attacks.Count == 0)
            {
                strike();
            }
        }


        public void autoRotate()
        {
            if (Player.playerPosition.X < position.X)
            {
                angle.Y += MathHelper.ToRadians(rotSpeed);
            }
            else if (Player.playerPosition.X > position.X)
            {
                angle.Y += MathHelper.ToRadians(-rotSpeed);
            }
        }



    }
}

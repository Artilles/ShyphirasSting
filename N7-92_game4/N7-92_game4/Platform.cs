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
    public enum TileCollision
    {
        /// <summary>
        /// A passable tile is one which does not hinder player motion at all.
        /// </summary>
        Passable = 0,

        /// <summary>
        /// An impassable tile is one which does not allow the player to move through
        /// it at all. It is completely solid.
        /// </summary>
        Impassable = 1,

        /// <summary>
        /// A platform tile is one which behaves like a passable tile except when the
        /// player is above it. A player can jump up through a platform as well as move
        /// past it to the left and right, but can not fall down through the top of it.
        /// </summary>
        Platform = 2,
        Sink = 3,
        //falls
        Fall = 4,
        //floor and hardleft
        Left = 5,
        //skycap and hardright
        Right = 6,
        //spikes
        Spikes = 7,
        
        Break = 8,
        Boss = 9,
        Column = 10,
    }

    public class Platform : Entity
    {
        Vector2 speed;
        Vector2 hard;
        public TileCollision Collision;
        public List<Vector2> waypoints = new List<Vector2>();
        public Vector2 nextWaypoint; //The target waypoint
        int waypointCounter = 0;//Counts which waypoint the platform is heading to
        Vector2 point = new Vector2();
        Boolean hitWaypointX = false;
        Boolean hitWaypointY = false;

        public Platform(int x, int y, int h, int w, float s)
        {
            hard = new Vector2();
            hard.X = x;
            hard.Y = y;

            size.Y = h;
            size.X = w;
            size.Z = 1;
            setAbsolutePosition(hard);
        }
        public Platform(String _model, TileCollision collision)
        {
            mSpeed.X = 0.0f;
            mSpeed.Y = 0.0f;
            model = ContentClass.models[_model];
            Collision = collision;
            Height = 115;
            Width = 115;
            size.Y = 1;
            size.X = 1;
            size.Z = 1;
            rectangle = new Rectangle((int)GetCoords().X, (int)GetCoords().Y, Height, Width);
        }

        public void LoadContent()
        {

            if (model == null)
            {
                model = ContentClass.models["Platform2"];
            }
            // texture = c.Load<Texture2D>("cube1_auv");
        }
        public void LoadContent(String _model)
        {

            model = ContentClass.models[_model];
            // texture = c.Load<Texture2D>("cube1_auv");
        }

        public void setTexture(String texturePath)
        {
            //texture = case.LoadContent<Texture2D>(texturePath);
        }

        public void addWaypoint(float x, float y)
        {
            point.X = x;
            point.Y = y;
            waypoints.Add(point);
            if (waypoints.Count == 1)
            {
                nextWaypoint = point;
            }
        }
        public void rotate(int angl)
        {
            angle.Y = MathHelper.ToRadians(angl);
        }

        public void Update(GameTime gametime, int time, Vector3 n)
        {
            //view = Matrix.CreateLookAt(GameBase.camera.current, GameBase.camera.target, Vector3.UnitY);
            // view = GameBase.camera.view;
         /*   if (Player.playerPosition.X - position.X > MathHelper.ToRadians(40) && Player.playerPosition.X - position.X < MathHelper.ToRadians(-40))
            {
                Visible = false;
            }
            else
            {
                Visible = true;
            }*/
            foreach (Vector2 point in waypoints)
            {
                if (nextWaypoint == point)
                {
                    if (position.X > point.X)
                    {
                        position.X -= mSpeed.X;
                        hitWaypointX = false;
                        if (position.X <= point.X)
                        {
                            position.X = point.X;
                            hitWaypointX = true;
                        }
                    }
                    else if (position.X < point.X)
                    {
                        position.X += mSpeed.X;
                        hitWaypointX = false;
                        if (position.X > point.X)
                        {
                            position.X = point.X;
                            hitWaypointX = true;
                        }
                    }

                    if (position.Y > point.Y)
                    {
                        position.Y -= mSpeed.Y;
                        hitWaypointY = false;
                        if (position.Y < point.Y)
                        {
                            position.Y = point.Y;
                            hitWaypointY = true;
                        }
                    }
                    else if (position.Y < point.Y)
                    {
                        position.Y += mSpeed.Y;
                        hitWaypointY = false;
                        if (position.Y > point.Y)
                        {
                            position.Y = point.Y;
                            hitWaypointY = true;
                        }
                    }
                    if (hitWaypointX && hitWaypointY)
                    {
                        waypointCounter++;
                        if (waypointCounter > waypoints.Count)
                        {
                            waypointCounter = 1;
                        }
                        nextWaypoint = waypoints[waypointCounter-1];
                    }
                }
            }
            position.Y -= mSpeed.Y;
            world = Matrix.CreateScale(size.X, size.Y, size.Z) * Matrix.CreateRotationX(angle.X) * Matrix.CreateRotationY(angle.Y) * Matrix.CreateRotationZ(angle.Z) * Matrix.CreateTranslation(position);
            base.Update(world);// world *= Matrix.CreateScale(3, 6, 0);
            if (!Collision.Equals(TileCollision.Fall))
                firstLoad = false;
        }
    }
}

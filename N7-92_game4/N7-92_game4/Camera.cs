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
    public class Camera : Entity
    {
        public Vector3 start;
        public Vector3 current;//position
        public Vector3 target;
        public Matrix view { get; protected set; }
        public Matrix projection;
        public Vector3 cameraPosition;
        public Vector3 currentCameraTarget;
        public float lowX;
        public float highX;
        public float lowY;
        public float highY;
        public Vector2 change = new Vector2(0, 0);
        public Vector2 topLEFT;
        public static float end;
        public static float begin;
        float cameraBoundsLeft;
        float cameraBoundsRight;
        public static float min;

        public BoundingFrustum Frustum;

        public Vector2 position;

        public List<Vector2> zoomOutList = new List<Vector2>();
        public static bool isLoadFinished=false;
        public bool isStopLoad;
        public List<Vector2> zoomInList = new List<Vector2>();

        public Camera()
        {
            
            target = new Vector3(0, 0, 0);
            start = new Vector3(0, 0, 10);

            current = start;
            position = new Vector2(0, 0);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), 800 / 460f, 1.0f, 10.0f);
            
            #region Ares 12.09

            zoomOutList = new List<Vector2>();

            //zoomInList.Add(new Vector2(-100, -100));

            addZoomOutPoint(33, 40);

            #endregion

        }


        public void loadZoomOutPoints() {/*
            if (GameBase.levelnumber==1&&isLoadFinished==false)
            {
                zoomOutList.Clear();
                addZoomOutPoint(55, 58);
                addZoomOutPoint(131, 173);
                isLoadFinished = true;
            }

            if (GameBase.levelnumber==2&&isLoadFinished==false)
            {
                zoomOutList.Clear();
                addZoomOutPoint(-100, -100);
                addZoomOutPoint(28, 10000);
               // addZoomOutPoint(55, 60);
                isLoadFinished = true;
            }

            if (GameBase.levelnumber == 3 && isLoadFinished == false)
            {
                zoomOutList.Clear();

                addZoomOutPoint(20, 86);
                addZoomOutPoint(366, 700);
                isLoadFinished = true;
            }

            if (GameBase.levelnumber == 4 && isLoadFinished == false)
            {
                zoomOutList.Clear();
                addZoomOutPoint(33, 40);
                addZoomOutPoint(55, 60);
                isLoadFinished = true;
            }

            if (GameBase.levelnumber == 5 && isLoadFinished == false)
            {
                zoomOutList.Clear();
                addZoomOutPoint(33, 40);
                addZoomOutPoint(55, 60);
                isLoadFinished = true;
            }

            if (GameBase.levelnumber == 6 && isLoadFinished == false)
            {
                zoomOutList.Clear();
                addZoomOutPoint(33, 40);
                addZoomOutPoint(55, 60);
                isLoadFinished = true;
            }*/

        }

        //position, and speed
        public void Update(Vector2 _playerPosition, Vector2 _playerSpeed)
        {

                

            loadZoomOutPoints();

         
            position = _playerPosition;
            cameraPosition = target;

            cameraBoundsRight = cameraPosition.X + MathHelper.ToRadians(GameBase.ScreenSize.X / 2) - MathHelper.ToRadians(230);
            cameraBoundsLeft = cameraPosition.X - MathHelper.ToRadians(GameBase.ScreenSize.X / 2) + MathHelper.ToRadians(230);

            #region opposite
            if (_playerSpeed.X > 0f && cameraPosition.X < end)
            {
                if (_playerSpeed.X > 0 && _playerPosition.X > cameraBoundsLeft)
                {
                    change.X = _playerSpeed.X + 0.04f;
                }
                else
                {
                    change.X = _playerSpeed.X;
                }
            }
            else if (_playerSpeed.X < 0f && cameraPosition.X > begin)
            {
                if (_playerSpeed.X < 0 && _playerPosition.X < cameraBoundsRight)
                {
                    change.X = _playerSpeed.X - 0.04f;
                }
                else
                {
                    change.X = _playerSpeed.X;
                }
            }
            else
            {
                change.X = 0;
            }
            if (_playerSpeed.Y > 0f )
            {
                if (_playerSpeed.Y > 0 && (_playerPosition.Y - cameraPosition.Y) > -1)
                {
                    change.Y = _playerSpeed.Y + 0.03f;                             
                }
                else
                {
                    change.Y = _playerSpeed.Y;           
                }
            }
            else if (_playerSpeed.Y < 0f && cameraPosition.Y > min)
            {
                if (_playerSpeed.Y < 0 && (_playerPosition.Y - cameraPosition.Y) < 0.5)
                {
                    change.Y = _playerSpeed.Y - 0.05f;
                }
                else
                {
                    change.Y = _playerSpeed.Y;
                }
            }
            else
            {
                change.Y = 0;
            }
            #endregion
            
            cameraPosition.X += change.X;
            cameraPosition.Y += change.Y;

            target = cameraPosition;
          
            current.X = target.X;
            current.Y = target.Y;
            topLEFT.X = target.X + GameBase.ScreenSize.X / 2;
            topLEFT.Y = target.Y + GameBase.ScreenSize.Y / 2;
            

            Frustum = new BoundingFrustum(view * projection);
            autoZoomOut();

            
        }

        public Vector3 Get()
        {
            return current;
        }
        public void removelist()
        {
            zoomOutList = new List<Vector2>();
            
        }
        public Vector4 GetScreen()
        {
            return new Vector4(current.X + GameBase.ScreenSize.X, current.X - GameBase.ScreenSize.X,
                current.Y + GameBase.ScreenSize.Y, current.Y - GameBase.ScreenSize.Y);
        }

        public void UpdateCameraPosition()
        {
            view = Matrix.CreateLookAt(new Vector3(0, 0, 10), target, Vector3.Up);
        }

        public void Reset()
        {
            //Console.WriteLine(Player.playerPosition.X);
            cameraPosition.X = Player.playerPosition.X;
            cameraPosition.Y = Player.playerPosition.Y;
            change.X = 0;
            change.Y = 0;
            target = cameraPosition;
            min = target.Y + 1;
            //  target.Y = cameraPosition.Y + .15f;
            current.X = target.X;
            current.Y = target.Y;
            topLEFT.X = target.X + GameBase.ScreenSize.X / 2;
            topLEFT.Y = target.Y + GameBase.ScreenSize.Y / 2;
            ZoomGoBack();

        }
        public void autoZoomOut()
        {
            
            for (int i = 0; i < zoomOutList.Count; i++)
            {
                if ((Player.playerPosition.X > zoomOutList[i].X && Player.playerPosition.X < zoomOutList[i].Y))
                {
                    if (current.Z < 20)
                    {
                        current.Z += 1f;
                    }
                }
                if (i != 0)
                    if ((Player.playerPosition.X > zoomOutList[i - 1].Y && Player.playerPosition.X < zoomOutList[i].X) || Player.playerPosition.X > zoomOutList[zoomOutList.Count - 1].Y || Player.playerPosition.X < zoomOutList[0].X)
                    {
                        ZoomGoBack();
                    }
            }
        }
       
        public void ZoomGoBack()
        {

            if (current.Z > 10)
            {
                current.Z -= 1f;
            }

            if (current.Z < 10)
            {
                current.Z += 1f;
            }
        }

      
        public void addZoomOutPoint(float x, float y)
        {
            zoomOutList.Add(new Vector2(x, y));
        }
        public void addZoomInPoint(float x, float y)
        {
            zoomOutList.Add(new Vector2(x, y));
        }
    }
}

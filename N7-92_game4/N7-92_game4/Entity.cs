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
    public class Entity
    {
        public BasicEffect boxEffect;
        public float maxHSpeed;
        public Model model;
        public string type = "";
        public int Height;
        public int Width;
        public float hAccel;
        public float vAccel;
        public Matrix world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        public Matrix mod = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        public Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), Vector3.UnitY);
        public Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), GameBase.ScreenSize.X / GameBase.ScreenSize.Y, 0.1f, 100f);
        public Vector3 size;
        public Vector3 position;
        public Vector3 positionOld;
        public Vector3 angle;
        public Vector3 actual;
        public float gravity = .0098f;
        public Boolean grav;
        public float horizontalSpeed;
        public float verticalSpeed;
        public Vector3 mSpeed;
        public Vector3 mSpeedOld;
        public KeyboardState prevKeyboardState;
        public GamePadState prevPlayer1;
        public int debug = 0;
        public Boolean Visible = true;
        int mCurrentHealth = 100;
        public int health;
        public KeyboardState keyboardState;
        public GamePadState player1;
        public Rectangle rectangle;
        public float threas = .007001f;
        
        
        public BoundingBox bounds;
        public List<BoundingBox> boundingBoxes;
        
        public Boolean collisionBottom = false;
        public Boolean collisionTop = false;
        public Boolean collisionLeft = false;
        public Boolean collisionRight = false;
        public BoundingBox bufferedBounds;
        public Vector3 bufferedPosition;

        public Sprite test;
        public Boolean invincible = false;
        public int inv = 0;

        public bool firstLoad = true;
  
        
  
        //DEBUG
        short[] bbIndices = {
            0, 1, 1, 2, 2, 3, 3, 0, // front
            4, 5, 5, 6, 6, 7, 7, 4, // back
            0, 4, 1, 5, 2, 6, 3, 7 // side
        };
        //END DEBUG

        public Entity()
        {
            boxEffect = GameServices.GetService<BasicEffect>();
        }

        public void Initialize(Model m)
        {
            model = m;            
        }

        public void Update(Matrix w)
        {
            if (Visible)
            {
                if (inv <= 0)
                {
                    invincible = false;
                }

                view = Matrix.CreateLookAt(GameBase.camera.current, GameBase.camera.target, Vector3.UnitY);
                world = w;
                if (firstLoad)
                {
                    bounds = UpdateBoundingBox(model, w);
                    if (type.Equals("dragon"))
                    {
                        bufferedBounds = UpdateBufferedBox();

                    }
                }
                Height = (int)MathHelper.ToDegrees(bounds.Max.Y - bounds.Min.Y);
                Width = (int)MathHelper.ToDegrees(bounds.Max.X - bounds.Min.Y);
            }
        }

        public void UpdateBoundless(Matrix w)
        {
            if (Visible)
            {
                if (inv <= 0)
                {
                    invincible = false;
                }
                view = Matrix.CreateLookAt(GameBase.camera.current, GameBase.camera.target, Vector3.UnitY);
                world = w;
            }
        }
        public void DrawBound(SpriteBatch spriteBatch)
        {
            if (Level.Debug)
            {
              //  foreach (BoundingBox b in boundingBoxes)
              //  {
                    Vector3[] corners = bounds.GetCorners();

                    VertexPositionColor[] primitiveList = new VertexPositionColor[corners.Length];

                    for (int i = 0; i < corners.Length; i++)
                        primitiveList[i] = new VertexPositionColor(corners[i], Color.Blue);


                    boxEffect.World = Matrix.Identity;
                    boxEffect.View = view;
                    boxEffect.Projection = projection;
                    boxEffect.TextureEnabled = false;

                    GraphicsDevice device = GameServices.GetService<GraphicsDevice>();
                    foreach (EffectPass pass in boxEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        device.DrawUserIndexedPrimitives(
                            PrimitiveType.LineList, primitiveList, 0, 8,
                            bbIndices, 0, 12);
                    }
              //  }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                inv--;
                DrawModel(model, world, view, projection);

                DrawBound(spriteBatch);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D t)
        {
            if (Visible)
            {
                inv--;
                DrawModel(model, world, view, projection, t);
            }
        }

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
     
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
        }
        

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection, Texture2D t)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), 800/460f, 1.0f, 10.0f);
                    //effect.Projection = projection;
                    effect.Texture = t;
                }
                mesh.Draw();
            }

        }


        public Vector2 GetCoords()
        {
            Vector2 c = new Vector2(MathHelper.ToDegrees(position.X) + (GameBase.ScreenSize.X / 2), MathHelper.ToDegrees(position.Y) + (GameBase.ScreenSize.Y / 2));
            return c;
        }

        public Vector2 GetCoordsRadCentre()
        {
            Vector2 c = new Vector2(position.X, position.Y);
            return c;
        }

        public float getGravity()
        {
            return gravity;

        }

        public Vector2 GetSize()
        {
            //model.Meshes.

            Vector2 c = new Vector2(size.X*10, size.Y*10);
            //model.Meshes.
            return c;
        }

        public Vector2 GetSizeRad()
        {
            Vector2 c = new Vector2(MathHelper.ToRadians(size.X) + (GameBase.ScreenSize.X / 2), MathHelper.ToRadians(size.Y) + (GameBase.ScreenSize.Y / 2));
            return c;
        }

        public void setAbsolutePosition()
        {
            position.X -= MathHelper.ToRadians(GameBase.ScreenSize.X/ 2);
            position.Y -= MathHelper.ToRadians(GameBase.ScreenSize.Y / 2);
        }

        public void setPosition(float x, float y)
        {
            position.X = MathHelper.ToRadians(x);
            position.Y = MathHelper.ToRadians(y);
        }

        public void setAbsolutePosition(Vector2 z)
        {
            Vector2 send = new Vector2();
            send.X += z.X - (GameBase.ScreenSize.X / 2);
            send.Y += z.Y - (GameBase.ScreenSize.Y / 2);
            position.X = MathHelper.ToRadians(send.X);
            position.Y = MathHelper.ToRadians(send.Y);
        }

        public void setAbsolutePosition(float x, float y)
        {
            Vector2 send = new Vector2();
            send.X += x - (GameBase.ScreenSize.X / 2);
            send.Y += y - (GameBase.ScreenSize.Y / 2);
            position.X = MathHelper.ToRadians(send.X);
            position.Y = MathHelper.ToRadians(send.Y);
        }

        public Rectangle getRect()
        {

            return rectangle;
        }

        public void fireballHit(int damage)
        {
            mCurrentHealth = mCurrentHealth - damage;
        }


        protected BoundingBox UpdateBoundingBox(Model model, Matrix worldTransform)
        {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), worldTransform);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }

            // Create and return bounding box

            BoundingBox returnBox = new BoundingBox(min, max);


            return returnBox;
        }

        protected BoundingBox UpdateBufferedBox()
        {
            Vector3 temp = Player.playerPosition;
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(temp.X - 15f, temp.Y - 7f, temp.Z - 0.5f);
            Vector3 max = new Vector3(temp.X + 15f, temp.Y + 7f, temp.Z + 0.5f);

            BoundingBox returnBox = new BoundingBox(min, max);


            return returnBox;
        }

        protected List<BoundingBox> UpdateBoundingBoxList(Model model, Matrix worldTransform)
        {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            List<BoundingBox> boxes = new List<BoundingBox>();

            // For each mesh of the model
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), worldTransform);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                    
                }
                BoundingBox returnBox = new BoundingBox(min, max);
                boxes.Add(returnBox);
            }

            return boxes;
        }

        public void hit(int damage)
        {
            if (!invincible)
            {
                health -= damage;
            }
        }

        public void invinc(int frames)
        {
            if (inv <= 0)
            {
                inv = frames;
                invincible = true;
            }
        }

    }
}

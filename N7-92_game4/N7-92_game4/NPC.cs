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
    public class NPC
    {
        int height;
        int width;
        String mo;
        public float speed;
        private Model model;
        private Matrix world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        private Matrix mod = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        private Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 30), new Vector3(0, 0, 0), Vector3.UnitY);
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);
        private Vector3 position;
        private Vector3 angle;

        public NPC() { } //Avoids error "parent class does not contain contructor that contains 0 arguments" for inheriting classes

        public NPC(int x, int y)
        {
            position.X = MathHelper.ToRadians(x);
            position.Y = MathHelper.ToRadians(y);
        }

        public NPC(int x, int y, String s)
        {
            mo = s;
            position.X = MathHelper.ToRadians(x);
            position.Y = MathHelper.ToRadians(y);
        }

        public void LoadContent(ContentManager c, String who)
        {
            mo = who;
            model = c.Load<Model>(mo);   
        }

        public void Update(GameTime gametime)
        {
            angle.Y+= (float)gametime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.1f);                       
            
            world = Matrix.CreateRotationX(angle.X) * Matrix.CreateRotationY(angle.Y) * Matrix.CreateRotationZ(angle.Z) * Matrix.CreateTranslation(position);
        }

        public void Draw(SpriteBatch s)
        {
            DrawModel(model, world, view, projection);
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
    }
}
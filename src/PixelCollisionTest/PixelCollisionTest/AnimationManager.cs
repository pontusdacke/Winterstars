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

namespace PixelCollisionTest
{
    class AnimationManager
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Rectangle Rectangle;
        public Color Color = Color.White;
        public Vector2 Origin;
        public float Rotation = 0f;
        public float Scale = 1f;
        public SpriteEffects SpriteEffect;
        protected Rectangle[] Rectangles;
        protected int frameIndex = 0;

        public AnimationManager(Texture2D Texture, int Frames, Vector2 Position)
        {
            this.Position = Position;
            this.Texture = Texture;
            int width = Texture.Width / Frames;
            Rectangles = new Rectangle[Frames];
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width / Frames, Texture.Height);
            for (int i = 0; i < Frames; i++)
            {
                Rectangles[i] = new Rectangle(i * width, 0, width, Texture.Height);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Rectangles[frameIndex],
                Color, Rotation, Origin, SpriteEffect, 0f); 
        }

        public void ResetAnimation()
        {
            frameIndex = 0;
        }
    }
}

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
    class SpriteAnimation : AnimationManager
    {
        private float timeElapsed;
        public bool IsLooping = false;

        private float timeToUpdate = 0.05f;

        public SpriteAnimation( Vector2 Position, Texture2D Texture, int frames)
            : base(Texture, frames, Position)
        {

        }

        public int FramesPerSecond
        {
            set { timeToUpdate = (1f / value); }
        }

        public void Update(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeElapsed > timeToUpdate)
            {
                timeElapsed -= timeToUpdate;

                if (frameIndex < Rectangles.Length - 1)
                    frameIndex++;
                else if (IsLooping)
                    frameIndex = 0;
            }
        }
    }
}

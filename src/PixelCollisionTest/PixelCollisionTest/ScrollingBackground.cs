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
    class ScrollingBackground
    {
        Texture2D background;
        Vector2 position;

        public ScrollingBackground(Texture2D background)
        {
            this.background = background;
            position = Vector2.Zero;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, position, Color.White);
            spriteBatch.Draw(background, new Vector2(0, -background.Height) + position, Color.White);
        }

        public void Update(float speed)
        {
            position.Y += speed;
            if (position.Y >= background.Height) position.Y = 0;
        }
    }
}

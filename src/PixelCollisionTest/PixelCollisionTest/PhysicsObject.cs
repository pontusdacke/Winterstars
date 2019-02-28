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
    class PhysicsObject : SpriteAnimation
    {
        public Vector2 Velocity;
        float acceleration;
        float heightModifier;

        public enum State { Falling, Colliding };
        public State state;

        bool canJump;

        public PhysicsObject(Vector2 StartPosition, Texture2D Sprite, int frames)
            : base(StartPosition,Sprite, frames)
        {
            base.IsLooping = true;
            base.FramesPerSecond = 25;
            canJump = true;
            state = State.Falling;
            Velocity = new Vector2(0f, 0f);
            heightModifier = 3.5f; // Higher value = shorter length of jump
            acceleration = 0.9f / heightModifier; // Higher value = faster
        }

        public float HeightModifier
        {
            get { return heightModifier; }
            set { heightModifier = value; }
        }
        public float Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }

        public void IntersectAndStick(Rectangle rectangle)
        {
            if (base.Rectangle.Intersects(rectangle))
            {
                state = State.Colliding;
                Rectangle tempRect = base.Rectangle;
                tempRect.Y = rectangle.Y - base.Rectangle.Height;
                Rectangle = tempRect; 
                Velocity.X = 0f;
                canJump = true;
            }
        }

        public void JumpVelocity(int height)
        {
            if (canJump)
            {
                Velocity.Y = -(height / heightModifier);
                state = State.Falling;
                canJump = false;
            }
        }

        public void EnableJump()
        {
            canJump = true;
        }

        public void Move(GameTime gameTime)
        {
            if (Mouse.GetState().X > Rectangle.X)
            {
                base.SpriteEffect = SpriteEffects.None;
                Rectangle.X += (int)((Mouse.GetState().X - Rectangle.X) / 15 + 0.1f);
                Update(gameTime);
            }
            else if (Mouse.GetState().X < Rectangle.X)
            {
                base.SpriteEffect = SpriteEffects.FlipHorizontally;
                Rectangle.X -= (int)((Rectangle.X - Mouse.GetState().X) / 15 + 0.1f);
                Update(gameTime);
            }
            else
                ResetAnimation();

        }

        public void UpdateVelocity(float XSpeed, int MaxFallSpeed)
        {
            if (Velocity.X > 0) Velocity.X -= XSpeed;
            if (Velocity.X < 0) Velocity.X += XSpeed;
            Rectangle rTemp = base.Rectangle;
            rTemp.X += (int)Velocity.X;
            base.Rectangle = rTemp;

            if (state == State.Falling)
            {
                if (Velocity.Y <= MaxFallSpeed)
                    Velocity.Y += acceleration;

                rTemp = base.Rectangle;
                rTemp.Y += (int)Velocity.Y;
                base.Rectangle = rTemp;
            }
        }

    }
}

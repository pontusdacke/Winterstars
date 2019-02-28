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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Winterstars : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public static Random rand = new Random();
        SpriteBatch spriteBatch;
        Texture2D bunny, star, bg;
        ScrollingBackground sbg;

        SpriteFont scoreFont;

        Rectangle ground;

        List<SpriteAnimation> starList = new List<SpriteAnimation>();

        PhysicsObject character;


        int counter, score;
        int indexToRemove;

        bool gameWon = false;
        bool upAndJumping = false;
        bool splash = false;
        float fader = 1.0f;

        bool removeIndex = false;
        private bool canJump = true;

        public Winterstars()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.IsFullScreen = true;
            IsMouseVisible = true;
            graphics.ApplyChanges();
            ground = new Rectangle(0, 768, 1024, 50);

            counter = 0;
            score = 0;

            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            bunny = Content.Load<Texture2D>("bunny");
            star = Content.Load<Texture2D>("star");
            scoreFont = Content.Load<SpriteFont>("sf");
            bg = Content.Load<Texture2D>("background");

            sbg = new ScrollingBackground(bg);
            character = new PhysicsObject(new Vector2(1024/2, 768-bunny.Height), bunny, 5);
            
            
            AddPlatform(9);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) this.Exit();

            character.Move(gameTime);
            character.UpdateVelocity(0.7f, 10);


            if (Mouse.GetState().LeftButton == ButtonState.Pressed && canJump)
            {
                canJump = false;
                character.JumpVelocity(30);
            }
            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                canJump = true;
            }

            foreach (SpriteAnimation go in starList)
            {
                if (character.Rectangle.Intersects(go.Rectangle))
                {
                    indexToRemove = starList.IndexOf(go);
                    character.EnableJump();
                    character.JumpVelocity(30);
                    removeIndex = true;
                }
            }
            if (removeIndex)
            {
                counter += 1;
                score += counter * 10;
                splash = true;
                fader = 1.0f;

                starList.RemoveAt(indexToRemove);
                AddPlatform();
                removeIndex = false;
            }

            if (character.Rectangle.Y <= 500 || score > 0)
            {
                upAndJumping = true;
            }


            for (int i = 0; i < starList.Count; i++)
            {
                starList[i].Rectangle.Y += 2;
                if (!upAndJumping)
                {
                    if (starList[i].Rectangle.Y >= 650)
                    {
                        starList.RemoveAt(i);
                        AddPlatform();
                    }
                }
                else
                {
                    if (starList[i].Rectangle.Y >= 768)
                    {
                        starList.RemoveAt(i);
                        AddPlatform();
                    }
                }
            }

            if (character.Rectangle.Y <= 500)
            {
                for (int i = 0; i < starList.Count; i++)
                {
                    starList[i].Rectangle.Y += 5 - (character.Rectangle.Y / 100);
                }
                sbg.Update(5 - (character.Rectangle.Y / 100));
            }

            if(!upAndJumping)
                character.IntersectAndStick(ground); // check collision with the ground
            if (character.Rectangle.Y >= 800)
            {
                gameWon = true;
            }

            if (gameWon)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    gameWon = false;
                    score = 0;
                    counter = 0;
                    starList.Clear();
                    AddPlatform(9);
                    character.Rectangle.Y = 768 - character.Rectangle.Height;
                    character.Rectangle.X = 1024/2;
                    character.Velocity = Vector2.Zero;
                    upAndJumping = false;

                }
            }
            
            if (fader >= 0)
                fader -= 0.02f;
            else
            {
                splash = false;
            }


            for (int i = 0; i < starList.Count; i++)
            {
                starList[i].Origin = new Vector2(starList[i].Rectangle.Width / 2 - 5, starList[i].Rectangle.Height / 2 - 5);
                starList[i].Rotation += 0.05f;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            sbg.Draw(spriteBatch);//draw scrolling background
            character.Draw(spriteBatch); // Draw teh bunny

            if (!gameWon)
                spriteBatch.DrawString(scoreFont, "Score: " + score, new Vector2(1024 / 2 - 50, 20), Color.White); // Draw teh Score
            else
            {
                spriteBatch.DrawString(scoreFont, "End Score: " + score, new Vector2(1024 / 2 - 100, 768 / 2), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
                spriteBatch.DrawString(scoreFont, "Press Enter to Play Again", new Vector2(1024 / 2 - 80, 768 / 2 + 50), Color.White);
            }
                if (splash)
            {
                spriteBatch.DrawString(scoreFont, (counter * 10).ToString(), new Vector2(character.Rectangle.X, character.Rectangle.Y - 20), Color.Yellow * fader);
            }
           
            foreach (SpriteAnimation go in starList) // Draw teh stars
            {
                //spriteBatch.DrawString(scoreFont, "X: " + go.Rectangle.X + " Y: " + go.Rectangle.Y, new Vector2(20, starList.IndexOf(go) * 20), Color.Blue);
                //spriteBatch.Draw(star, go.Rectangle, Color.White);
                go.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void AddPlatform()
        {
            if (starList.Count > 0)
            {
                int randX = rand.Next(starList[starList.Count - 1].Rectangle.X - 400, starList[starList.Count - 1].Rectangle.X + 400);
                randX = Math.Abs(randX % 1024); // No negative numbers, and only numbers inside 0 and 1024
                int randY = starList[starList.Count - 1].Rectangle.Y - 170;
                starList.Add(new SpriteAnimation(new Vector2(randX, randY), star, 1));
            }
            else
            {
                starList.Add(new SpriteAnimation(new Vector2(1024/2, 768/2*2), star, 1)); // first starlocation
            }
        }
        private void AddPlatform(int n)
        {
            if (n > 0)
                for (int i = 0; i < n; i++)
                    AddPlatform();
        }
    }
}

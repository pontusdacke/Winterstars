using System;

namespace PixelCollisionTest
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Winterstars game = new Winterstars())
            {
                game.Run();
            }
        }
    }
#endif
}


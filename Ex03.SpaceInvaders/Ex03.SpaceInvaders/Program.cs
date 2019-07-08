using System;

namespace Ex03.SpaceInvaders
{
#if WINDOWS || XBOX
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            using (SpaceInvadersGame game = new SpaceInvadersGame())
            {
                game.Run();
            }
        }
    }
#endif
}

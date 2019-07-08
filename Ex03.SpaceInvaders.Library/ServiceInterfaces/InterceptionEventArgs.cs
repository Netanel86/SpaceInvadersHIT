namespace Ex03.SpaceInvaders.Library.ServiceInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public class InterceptionEventArgs : EventArgs
    {
        public InterceptionEventArgs(PlayerIndex i_BulletSource)
        {
            BulletSource = i_BulletSource;
        }

        public PlayerIndex BulletSource { get; private set; }
    }
}

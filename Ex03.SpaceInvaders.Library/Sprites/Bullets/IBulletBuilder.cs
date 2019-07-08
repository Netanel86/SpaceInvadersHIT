namespace Ex03.SpaceInvaders.Library.Sprites.Bullets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Ex03.Infrastracture;
    using Ex03.Infrastracture.Direction;
    
    public interface IBulletBuilder
    {
        Bullet Build();
    }
}

namespace Ex03.SpaceInvaders.Library.Sprites.Bullets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Ex03.Infrastracture.Direction;

    public class InvaderBulletBuilder : IBulletBuilder
    {
        public InvaderBulletBuilder(Game i_Game)
        {
            m_Game = i_Game;
        }

        private Game m_Game;
        private Bullet m_Bullet;

        public Bullet Build()
        {
            m_Bullet = new InvaderBullet(m_Game, @"Sprites\Bullet")
            {
                MovementDirection = Direction2D.Down,
                TintColor = Color.Aqua
            };
            return m_Bullet;
        }
    }
}

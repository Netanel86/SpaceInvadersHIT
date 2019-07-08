namespace Ex03.SpaceInvaders.Library.Sprites.Bullets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Ex03.Infrastracture.Direction;

    public class SpaceCraftBulletBuilder : IBulletBuilder
    {
        public SpaceCraftBulletBuilder(Game i_Game, PlayerIndex i_BulletSource)
        {
            m_Game = i_Game;
            m_BulletSource = i_BulletSource;
        }

        private PlayerIndex m_BulletSource;
        private Game m_Game;
        private SpaceCraftBullet m_Bullet;

        public Bullet Build()
        {
            m_Bullet = new SpaceCraftBullet(m_Game, @"Sprites\Bullet")
                {
                    MovementDirection = Direction2D.Up,
                    TintColor = Color.Red,
                    BulletSource = m_BulletSource
                };
            
            return m_Bullet;
        }
    }
}

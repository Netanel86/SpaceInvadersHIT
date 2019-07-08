namespace Ex03.SpaceInvaders.Library.Sprites.Bullets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Ex03.Infrastracture.ObjectModel;
using Ex03.Infrastracture.ServiceInterfaces;
    
    public class BulletSpawner : CompositeDrawableComponent<Bullet>
    {
        private IBulletBuilder m_BulletBuilder;
        private int m_MaxSpawnedBullets;

        public BulletSpawner(Game i_Game, int i_MaxSpawnedBullets, IBulletBuilder i_BulletBuilder)
            : base(i_Game)
        {
            m_BulletBuilder = i_BulletBuilder;
            m_MaxSpawnedBullets = i_MaxSpawnedBullets;
        }

        public override void Initialize()
        {
            base.Initialize();
            initiateBulletArray();
        }
        
        public bool TrySpawnBullet(Rectangle i_SourceBounds)
        {
            bool fired = false;

            foreach (Bullet bullet in this)
            {
                if (!bullet.Visible)
                {
                    bullet.SourceBounds = i_SourceBounds;
                    bullet.InitBounds();
                    bullet.Visible = true;
                    bullet.Enabled = true;
                    fired = true;
                    break;
                }
            }

            return fired;
        }
        
        public void Reset()
        {
            foreach (Bullet bullet in this)
            {
                bullet.Visible = bullet.Enabled = false;
            }
        }
        
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.Clear();
        }
        
        private void initiateBulletArray()
        {
            for (int i = 0; i < m_MaxSpawnedBullets; i++)
            {
                Bullet bullet = m_BulletBuilder.Build();
                this.Add(bullet);
                bullet.Visible = false;
                bullet.Enabled = false;
            }
        }
    }
}
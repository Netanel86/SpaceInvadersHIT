namespace Ex03.SpaceInvaders.Library.Sprites.Bullets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.GamerServices;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;
    using Ex03.Infrastracture;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ServiceInterfaces;
    using Ex03.SpaceInvaders.Library.Sprites.Entities;

    public class SpaceCraftBullet : Bullet
    {
        public SpaceCraftBullet(Game i_Game, string i_Asset)
            : base(i_Game, i_Asset)
        {
        }

        public PlayerIndex BulletSource { get; set; }

        public override void Collided(ICollidable i_CollidedComponent)
        {
            base.Collided(i_CollidedComponent);
            if (i_CollidedComponent is Enemy || ((i_CollidedComponent is Bullet) && !(i_CollidedComponent is SpaceCraftBullet)))
            {
                this.Enabled = false;
                this.Visible = false;
            }
        }
    }
}

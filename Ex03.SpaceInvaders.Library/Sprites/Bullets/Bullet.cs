namespace Ex03.SpaceInvaders.Library.Sprites.Bullets
{
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
    using Ex03.Infrastracture;
    using Ex03.Infrastracture.Direction;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ServiceInterfaces;
    using Ex03.SpaceInvaders.Library.Sprites.Entities;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public class Bullet : Sprite, ICollidablePerPixel
    {
        public Vector2? PointOfImpact
        {
            get { return m_PointOfImpact; }
            set { m_PointOfImpact = value; }
        }

        public Rectangle SourceBounds { get; set; }

        private Vector2? m_PointOfImpact = null;
        
        private float m_PenetrationDepth = 0.7f;
        
        public Bullet(Game i_Game, string i_Asset)
            : base(i_Game, i_Asset)
        {
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            if (!this.Bounds.Intersects(this.Game.GraphicsDevice.Viewport.Bounds))
            {
                this.Visible = false;
                this.Enabled = false;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            this.m_Velocity.Y = 125f;
        }

        public override void InitBounds()
        {
            base.InitBounds();
            m_Position.X = this.SourceBounds.X + (this.SourceBounds.Width / 2) - (this.Width / 2);
            m_Position.Y = this.SourceBounds.Y + (this.Height * (int)m_MovementDirection.YAxis);
        }

        public virtual void Collided(ICollidable i_CollidedComponent)
        {
            Barrier barrier = i_CollidedComponent as Barrier;
            if (barrier != null)
            {
                if (this.MovementDirection.YAxis == eDirectionY.Up)
                {
                    if (m_PointOfImpact != null && m_PointOfImpact.Value.Y - (this.Height * m_PenetrationDepth) >= (int)this.Position.Y)
                    {
                        m_PointOfImpact = null;
                        this.Enabled = false;
                        this.Visible = false;
                    }
                }
                else if (this.MovementDirection.YAxis == eDirectionY.Down)
                {
                    if (m_PointOfImpact != null && m_PointOfImpact.Value.Y + (this.Height * m_PenetrationDepth) <= this.Position.Y + this.Height)
                    {
                        m_PointOfImpact = null;
                        this.Enabled = false;
                        this.Visible = false;
                    }
                }
            }
        }
    }
}
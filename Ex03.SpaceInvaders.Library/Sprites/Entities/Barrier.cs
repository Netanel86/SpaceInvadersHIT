namespace Ex03.SpaceInvaders.Library.Sprites.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Ex03.Infrastracture;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ServiceInterfaces;
    using Ex03.Infrastracture.Direction;
    using Ex03.SpaceInvaders.Library.Sprites.Bullets;
    using Ex03.SpaceInvaders.Library.GameServices;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public class Barrier : Sprite, ICollidablePerPixel
    {
        private IAudioManager m_AudioManager;

        public Vector2? PointOfImpact
        {
            get { return m_PointOfImpact; }
            set { m_PointOfImpact = value; }
        }

        private Vector2? m_PointOfImpact = null;

        public Barrier(Game i_Game, string i_AssetName)
            : base(i_Game, i_AssetName)
        {
        }

        public Barrier(Game i_Game, string i_AssetName, bool i_SharedTexture)
            : base(i_Game, i_AssetName, i_SharedTexture)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            m_MovementDirection = new Direction2D(eDirectionX.Left, eDirectionY.NoMovement);

            m_AudioManager = Game.Services.GetService(typeof(IAudioManager)) as IAudioManager;
        }

        public void Collided(ICollidable i_CollidedComponent)
        {
            if (i_CollidedComponent is Bullet || i_CollidedComponent is Invader)
            {
                ICollidablePerPixel source = i_CollidedComponent as ICollidablePerPixel;

                if (this.CheckCollisionPerPixel(source))
                {
                    m_AudioManager.Play("BarrierHit");

                    IntersectRectangle intersectionRect = this.GetIntersectionBounds(source.Bounds, this.Bounds);
                    for (int y = intersectionRect.YMin; y < intersectionRect.YMax; y++)
                    {
                        for (int x = intersectionRect.XMin; x < intersectionRect.XMax; x++)
                        {
                            int sourcePixelMapIdx = (int)(source.Bounds.Width * (y - source.Bounds.Y)) + (x - (int)source.Bounds.X);
                            int thisPixelMapIdx = (int)(this.Width * (y - this.Position.Y)) + (x - (int)this.Position.X);

                            Color colorSource = source.PixelMap[sourcePixelMapIdx];
                            Color colorThis = this.PixelMap[thisPixelMapIdx];

                            if (colorSource.A != 0 && colorThis.A != 0)
                            {
                                this.PixelMap[thisPixelMapIdx] = sr_TransparentPixel;
                            }
                        }
                    }

                    if (this.CheckThisTextureTransparency())
                    {
                        this.Dispose();
                    }
                    else
                    {
                        this.Texture.SetData<Color>(0, this.SourceRectangle, this.PixelMap, 0, (int)this.WidthBeforeScale * (int)this.HeightBeforeScale);
                    }
                }
            }
        }
    }
}

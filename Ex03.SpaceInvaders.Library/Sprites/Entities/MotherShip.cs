namespace Ex03.SpaceInvaders.Library.Sprites.Entities
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
    using Ex03.Infrastracture.ObjectModel.Animators;
    using Ex03.Infrastracture.ObjectModel.Animators.ConcreteAnimators;
    using Ex03.Infrastracture.Managers;
    using Ex03.Infrastracture.Direction;
    using Ex03.Infrastracture.ServiceInterfaces;
    using Ex03.SpaceInvaders.Library.Sprites.Bullets;
    using Ex03.SpaceInvaders.Library.GameServices;
    using Ex03.SpaceInvaders.Library.ServiceInterfaces;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public class MotherShip : Enemy, ICollidable2D
    {
        public float SpawnRate { get; set; }

        private const int k_BlinksPerSecond = 4;
        private OpacityAnimator m_OpacityAnimator;
        private BlinkAnimator m_BlinkAnimator;
        private ScaleAnimator m_ScaleAnimator;
        private bool m_Dying = false;

        public MotherShip(Game i_Game, string i_Asset, int i_ScoreHit)
            : base(i_Game, i_Asset, i_ScoreHit)
        {
        }

        public MotherShip(Game i_Game, string i_Asset, Color i_Color, int i_ScoreHit)
            : base(i_Game, i_Asset, i_Color, i_ScoreHit)
        {
        }

        public void Collided(ICollidable i_CollidedComponent)
        {
            if (!m_Dying)
            {
                SpaceCraftBullet bullet = i_CollidedComponent as SpaceCraftBullet;
                if (bullet != null)
                {
                    OnIntercepted(new InterceptionEventArgs(bullet.BulletSource));
                    m_Dying = true;

                    m_AudioManager.Play("MotherShipKill");
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            this.SpawnRate = 0.5f;
            this.m_Velocity = new Vector2(90f);
            this.m_MovementDirection.XAxis = eDirectionX.Right;
            this.Score = 800;

            initAnimations();
        }

        public override void InitBounds()
        {
            base.InitBounds();
            this.m_Position.X = this.Width * -1;
            this.m_Position.Y = this.Height;
        }

        public override void Update(GameTime i_GameTime)
        {
            const bool v_DrawMotherShip = true;
            if (this.Visible)
            {
                if (!this.Bounds.Intersects(this.Game.GraphicsDevice.Viewport.Bounds))
                {
                    this.Visible = !v_DrawMotherShip;
                }

                if (m_Dying)
                {
                    this.MovementDirection = Direction2D.NoMovement;
                    animateDying(i_GameTime);
                }
            }
            else
            {
                if (!m_Dying)
                {
                    this.Visible = RandomGenerator.Instance.RunGenerator(this.SpawnRate) ? v_DrawMotherShip : !v_DrawMotherShip;
                    InitBounds();
                }
            }

            base.Update(i_GameTime);
        }

        private void resetForNextSpawn()
        {
            m_Dying = false;
            this.MovementDirection.XAxis = eDirectionX.Right;
            InitBounds();
        }

        private void initAnimations()
        {
            m_OpacityAnimator = new OpacityAnimator("OpacityAnimator", TimeSpan.FromSeconds(2));
            this.Animations.Add(m_OpacityAnimator);

            m_ScaleAnimator = new ScaleAnimator("ScaleAnimator", TimeSpan.FromSeconds(2));
            this.Animations.Add(m_ScaleAnimator);

            m_BlinkAnimator = new BlinkAnimator("BlinkAnimator", k_BlinksPerSecond, TimeSpan.FromSeconds(2));
            this.Animations.Add(m_BlinkAnimator);

            this.Animations.Finished += new EventHandler(animators_Finished);
        }

        private void animateDying(GameTime i_GameTime)
        {
            if (!this.Animations.Enabled)
            {
                this.Animations.Resume();
            }
        }

        private void animators_Finished(object sender, EventArgs e)
        {
            this.Animations.Reset();
            this.Animations.Pause();
            resetForNextSpawn();
        }
    }
}

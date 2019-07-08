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
    using Ex03.Infrastracture.ServiceInterfaces;
    using Ex03.Infrastracture.Direction;
    using Ex03.Infrastracture.Managers;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ObjectModel.Animators;
    using Ex03.Infrastracture.ObjectModel.Animators.ConcreteAnimators;
    using Ex03.SpaceInvaders.Library.Sprites.Entities;
    using Ex03.SpaceInvaders.Library.Sprites.Bullets;
    using Ex03.SpaceInvaders.Library.GameServices;
    using Ex03.SpaceInvaders.Library.ServiceInterfaces;
    using Ex03.SpaceInvaders.Library.Screens;

    public class Invader : Enemy, ICollidablePerPixel
    {
        /// <summary>
        /// Represent's the chance for all invader's to spawn a bullet
        /// </summary>
        /// <remarks>
        /// Set with an integer value between 0 - 100
        /// where 100 is a 100% chance to fire each frame.
        /// </remarks>
        public static float BulletSpawnRate { get; set; }

        public static int BulletMax { get; set; }

        public Vector2? PointOfImpact
        {
            get { return m_PointOfImpact; }
            set { m_PointOfImpact = value; }
        }

        private BulletSpawner m_BulletSpawner;

        private Vector2? m_PointOfImpact = null;

        #region Animation Members
        public int StartCellIdx { get; set; }

        private CellAnimator m_CellAnimator;
        private CompositeAnimator m_DeathAnimator;
        private TimeSpan m_DeathAnimationLength = TimeSpan.FromSeconds(0.9);
        private const int k_RotationsPerSecond = 9;
        private bool m_IsDying = false;
        #endregion

        public Invader(Game i_Game, string i_Asset, int i_ScoreHit)
            : base(i_Game, i_Asset, i_ScoreHit)
        {
        }

        public Invader(Game i_Game, string i_Asset, Color i_Color, int i_ScoreHit)
            : base(i_Game, i_Asset, i_Color, i_ScoreHit)
        {
        }

        public Invader(Game i_Game, string i_Asset, Color i_Color, int i_ScoreHit, int i_SourceRectangleIdx)
            : base(i_Game, i_Asset, i_Color, i_ScoreHit)
        {
            m_SourceRectangleCount = 6;
            this.StartCellIdx = m_SourceRectangleIdx = i_SourceRectangleIdx;
        }

        public void Collided(ICollidable i_CollidedComponent)
        {
            if (!m_IsDying)
            {
                SpaceCraftBullet bullet = i_CollidedComponent as SpaceCraftBullet;
                if (bullet != null)
                {
                    OnIntercepted(new InterceptionEventArgs(bullet.BulletSource));
                    m_IsDying = true;

                    if (this.m_SourceRectangleIdx == 0)
                    {
                        m_AudioManager.Play("Enemy1Kill");
                    }

                    if(this.m_SourceRectangleIdx == 1)
                    {
                        m_AudioManager.Play("Enemy2Kill");
                    }

                    if (this.m_SourceRectangleIdx == 2)
                    {
                        m_AudioManager.Play("Enemy3Kill");
                    }

                    if(this.m_SourceRectangleIdx == 3)
                    {
                        m_AudioManager.Play("Enemy4Kill");
                    }

                    if (this.m_SourceRectangleIdx == 4)
                    {
                        m_AudioManager.Play("Enemy5Kill");
                    }

                    if(this.m_SourceRectangleIdx == 5)
                    {
                        m_AudioManager.Play("Enemy6Kill");
                    }
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            IInvaderManager manager = this.Game.Services.GetService(typeof(IInvaderManager)) as IInvaderManager;
            manager.GroupSpeedChanged += new EventHandler<TimeSpanArgs>(invaders_GroupSpeedChanged);

            IPlayScreen screen = this.Game.Services.GetService(typeof(IPlayScreen)) as IPlayScreen;
            m_BulletSpawner = new BulletSpawner(this.Game, BulletMax, new InvaderBulletBuilder(this.Game));
            screen.Add(m_BulletSpawner);

            this.Disposed += (sender, args) => m_BulletSpawner.Dispose();

            initAnimations();

            m_Velocity = new Vector2(16f);
            m_MovementDirection.XAxis = eDirectionX.Right;
            m_MovementDirection.YAxis = eDirectionY.Down;
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (!m_IsDying && RandomGenerator.Instance.RunGenerator(BulletSpawnRate))
            {
                if (m_BulletSpawner.TrySpawnBullet(this.Bounds))
                {
                    m_AudioManager.Play("InvadersFire");
                }
            }

            if (m_IsDying)
            {
                if (!this.Animations["DeathAnimator"].Enabled)
                {
                    this.Animations["DeathAnimator"].Resume();
                }
            }
        }

        protected override void InitPixelMap()
        {
            this.PixelMap = new Color[(int)(this.WidthBeforeScale * this.HeightBeforeScale)];
            this.Texture.GetData<Color>(0, m_SourceRectangle, this.PixelMap, 0, (int)(this.WidthBeforeScale * this.HeightBeforeScale));
        }

        protected override void UpdatePosition(GameTime i_GameTime)
        {
            ////position update is handled by the invader manager,
            ////overrided to cancel default movement logics
        }

        private void initAnimations()
        {
            m_CellAnimator = new CellAnimator(TimeSpan.FromSeconds(0.5), this.StartCellIdx, m_SourceRectangleIdx, m_SourceRectangleIdx + 1, TimeSpan.Zero, true);
            this.Animations.Add(m_CellAnimator);

            m_DeathAnimator = new CompositeAnimator(
                "DeathAnimator",
                m_DeathAnimationLength,
                this,
                new ScaleAnimator("ScaleAnimator", m_DeathAnimationLength),
                new RotateAnimator("RotateAnimator", k_RotationsPerSecond, m_DeathAnimationLength));
            m_DeathAnimator.ResetAfterFinish = false;
            m_DeathAnimator.Finished += new EventHandler(deathAnimation_Finished);
            this.Animations.Add(m_DeathAnimator);

            this.Animations.Enabled = true;
            this.Animations["DeathAnimator"].Enabled = false;
        }
        
        private void invaders_GroupSpeedChanged(object i_Sender, TimeSpanArgs i_TimeGapArgs)
        {
            m_CellAnimator.CellTime = i_TimeGapArgs.TimeSpan;
        }
        
        private void deathAnimation_Finished(object i_Sender, EventArgs i_Args)
        {
            this.Animations["DeathAnimator"].Reset();
            this.Animations["DeathAnimator"].Pause();
            doDying();
        }

        private void doDying()
        {
            m_IsDying = false;
            this.Visible = false;
            this.Enabled = false;
            this.Dispose();
        }
    }
}

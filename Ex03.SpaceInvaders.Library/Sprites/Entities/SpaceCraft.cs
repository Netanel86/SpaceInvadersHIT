namespace Ex03.SpaceInvaders.Library.Sprites.Entities
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
    using Ex03.Infrastracture.ServiceInterfaces;
    using Ex03.Infrastracture.Direction;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ObjectModel.Animators;
    using Ex03.Infrastracture.ObjectModel.Animators.ConcreteAnimators;
    using Ex03.SpaceInvaders.Library.Sprites.Bullets;
    using Ex03.SpaceInvaders.Library.Sprites.Entities;
    using Ex03.SpaceInvaders.Library.GameServices;
    using Ex03.SpaceInvaders.Library.ServiceInterfaces;

    using Ex03.SpaceInvaders.Library.Screens;
    using Ex03.SpaceInvaders.Library.Inputs;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public class SpaceCraft : Sprite, ICollidable2D, IInterceptable, IPlayer
    {
        private const int k_RotationsPerSecond = 5;
        private const int k_BlinksPerSecond = 5;

        private static readonly Dictionary<PlayerIndex, IPlayerInput> sr_Inputs;

        public static int PlayersCounter { get; set; }

        static SpaceCraft()
        {
            sr_Inputs = new Dictionary<PlayerIndex, IPlayerInput>();
            sr_Inputs.Add(PlayerIndex.One, new PlayerOneInput());
            sr_Inputs.Add(PlayerIndex.Two, new PlayerTwoInput());
        }

        public event EventHandler<EventArgs> SoulsChanged;

        public event EventHandler<EventArgs> Intercepted;

        public PlayerIndex PlayerIdx { get; set; }

        public bool Immortal { get; set; }

        public int Souls
        {
            get { return m_Souls; }
            set
            {
                m_Souls = value;
                OnSoulsChanged();
            }
        }

        public int Score { get; set; }

        public int MaxBullets { get; set; }

        private IAudioManager m_AudioManager;
        private IPlayerInput m_Input;
        private int m_Souls;
        private BulletSpawner m_BulletSpawner;

        #region Animation Members
        private CompositeAnimator m_DeathAnimator;
        private BlinkAnimator m_BlinkAnimator;

        private bool m_IsDying = false;

        #endregion

        public SpaceCraft(Game i_Game, string i_Asset)
            : base(i_Game, i_Asset)
        {
            this.PlayerIdx = (PlayerIndex)Enum.Parse(typeof(PlayerIndex), PlayersCounter.ToString());
            this.SourceRectangleIdx = PlayersCounter;
            this.SourceRectangleCount = 2;
            PlayersCounter++;
        }

        public void Collided(ICollidable i_CollidedComponent)
        {
            Bullet bullet = i_CollidedComponent as Bullet;
            if (!Immortal && bullet != null && !m_BulletSpawner.Contains(bullet))
            {
                m_IsDying = true;

                m_AudioManager.Play("LifeDie");
            }
        }

        public override void Initialize()
        {
            this.Immortal = false;
            this.m_Souls = 3;
            base.Initialize();

            this.Score = -500;

            this.MaxBullets = 2;
            this.m_Velocity.X = 190f;

            this.Disposed += new EventHandler<EventArgs>((sender, args) => SpaceCraft.PlayersCounter--);
            IInvaderManager invaders = this.Game.Services.GetService(typeof(IInvaderManager)) as IInvaderManager;
            invaders.GroupCountZero += new Action(invaderManager_GroupCountZero);

            IPlayScreen screen = this.Game.Services.GetService(typeof(IPlayScreen)) as IPlayScreen;
            m_BulletSpawner = new BulletSpawner(this.Game, MaxBullets, new SpaceCraftBulletBuilder(this.Game, PlayerIdx));
            screen.Add(m_BulletSpawner);

            m_Input = sr_Inputs[this.PlayerIdx];
            m_Input.InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;

            m_AudioManager = Game.Services.GetService(typeof(IAudioManager)) as IAudioManager;

            initAnimations();
        }

        public override void Update(GameTime i_GameTime)
        {
            if (m_IsDying)
            {
                this.MovementDirection.XAxis = eDirectionX.NoMovement;
                if (this.Souls == 1)
                {
                    if (!this.Animations["DeathAnimator"].Enabled)
                    {
                        this.Animations["DeathAnimator"].Resume();
                    }
                }

                if (this.Souls > 1)
                {
                    if (!this.Animations["BlinkAnimator"].Enabled)
                    {
                        this.Animations["BlinkAnimator"].Restart();
                    }
                }
            }
            else
            {
                checkUserInput(i_GameTime);
            }

            base.Update(i_GameTime);
        }

        public override void InitBounds()
        {
            base.InitBounds();

            m_Position.X = this.SourceRectangleIdx * this.Width;
            m_Position.Y = (float)this.Game.GraphicsDevice.Viewport.Height - (1.5f * this.Height);
        }

        protected virtual void OnIntercepted(object i_Sender, EventArgs i_Args)
        {
            if (Intercepted != null)
            {
                Intercepted.Invoke(i_Sender, i_Args);
            }
        }

        protected virtual void OnSoulsChanged()
        {
            if (SoulsChanged != null)
            {
                SoulsChanged.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void AutoGameServicesRegistration()
        {
            base.AutoGameServicesRegistration();

            IInterceptionManager inteceptMngr = this.Game.Services.GetService(typeof(IInterceptionManager)) as IInterceptionManager;
            if (inteceptMngr != null)
            {
                inteceptMngr.AttachInterceptable(this);
            }

            IPlayerManager playerMngr = this.Game.Services.GetService(typeof(IPlayerManager)) as IPlayerManager;
            if (playerMngr != null)
            {
                playerMngr.AddPlayer(this);
            }
        }

        protected override void UpdatePosition(GameTime i_GameTime)
        {
            base.UpdatePosition(i_GameTime);
            this.m_Position.X = MathHelper.Clamp(
                                            this.m_Position.X,
                                            0,
                                            this.Game.GraphicsDevice.Viewport.Width - this.SourceRectangle.Width);
        }

        private void checkUserInput(GameTime i_GameTime)
        {
            m_Input.CheckForUserInput();
            if (m_Input.PositionDelta != Vector2.Zero)
            {
                m_MovementDirection.XAxis = eDirectionX.NoMovement;
                this.m_Position.X += m_Input.PositionDelta.X;
            }

            m_MovementDirection.XAxis = m_Input.LeftKey ? eDirectionX.Left : m_Input.RightKey ? eDirectionX.Right : eDirectionX.NoMovement;

            if (m_Input.FireKey)
            {
                if (m_BulletSpawner.TrySpawnBullet(this.Bounds))
                {
                    m_AudioManager.Play("SpaceCraftFire");
                }
            }
        }

        private void doDying()
        {
            OnIntercepted(this, EventArgs.Empty);
            if (--this.Souls == 0)
            {
                this.Enabled = false;
                this.Visible = false;
                this.Dispose();
            }
            else
            {
                this.InitBounds();
            }

            m_IsDying = false;
        }

        private void initAnimations()
        {
            m_DeathAnimator = new CompositeAnimator(
                "DeathAnimator",
                TimeSpan.FromSeconds(2.5),
                this,
                new OpacityAnimator("OpacityAnimator", TimeSpan.FromSeconds(2.5)),
                new RotateAnimator("RotateAnimator", k_RotationsPerSecond, TimeSpan.FromSeconds(2.5)));
            m_DeathAnimator.ResetAfterFinish = false;
            m_DeathAnimator.Finished += new EventHandler(deathAnimation_Finished);
            this.Animations.Add(m_DeathAnimator);

            m_BlinkAnimator = new BlinkAnimator("BlinkAnimator", k_BlinksPerSecond, TimeSpan.FromSeconds(2.5));
            m_BlinkAnimator.Finished += new EventHandler(interceptionAnimation_Finished);
            this.Animations.Add(m_BlinkAnimator);

            this.Animations.Enabled = true;
            this.Animations["DeathAnimator"].Enabled = false;
            this.Animations["BlinkAnimator"].Enabled = false;
        }

        private void invaderManager_GroupCountZero()
        {
            m_BulletSpawner.Reset();
            this.InitBounds();
        }

        private void interceptionAnimation_Finished(object i_Sender, EventArgs i_Args)
        {
            this.Animations["BlinkAnimator"].Pause();
            doDying();
        }

        private void deathAnimation_Finished(object i_Sender, EventArgs i_Args)
        {
            this.Animations["DeathAnimator"].Reset();
            this.Animations["DeathAnimator"].Pause();
            doDying();
        }
    }
}

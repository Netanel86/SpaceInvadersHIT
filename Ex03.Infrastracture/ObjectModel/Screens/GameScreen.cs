namespace Ex03.Infrastracture.ObjectModel.Screens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Ex03.Infrastracture.ServiceInterfaces;
    using Ex03.Infrastracture.Managers;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class GameScreen : CompositeDrawableComponent<IGameComponent>
    {
        public IInputManager InputManager
        {
            get { return this.HasFocus ? m_InputManager : m_DummyInputManager; }
        }

        public IScreenManager ScreenManager
        {
            get { return m_ScreenManager; }
            set { m_ScreenManager = value; }
        }

        public event EventHandler Closed;

        public GameScreen PreviousScreen 
        {
            get { return m_PreviousScreen; }
            set { m_PreviousScreen = value; }
        }

        public bool IsOverlayed
        {
            get { return m_IsOverlayed; }
            set { m_IsOverlayed = value; }
        }

        public bool IsModal
        {
            get { return m_IsModal; }
            set { m_IsModal = value; }
        }

        public bool HasFocus
        {
            get { return m_HasFocus; }
            set { m_HasFocus = value; }
        }

        #region Faded Background Members

        public float BlackTintAlpha
        {
            get { return m_BlackTintAlpha; }
            set { m_BlackTintAlpha = value; }
        }
        
        protected bool m_UseGradientBackground = false;
        protected float m_BlackTintAlpha = 0;

        private Texture2D m_GradientTexture;
        private Texture2D m_BlankTexture;

        #endregion Faded Background Members

        protected bool m_IsOverlayed;
        protected bool m_IsModal = true;
        protected bool m_HasFocus;
        protected GameScreen m_PreviousScreen;
        protected IScreenManager m_ScreenManager;

        private bool m_FirstRun = true;
        
        private IInputManager m_InputManager;
        private IInputManager m_DummyInputManager = new DummyInputManager();
        
        public GameScreen(Game i_Game)
            : base(i_Game)
        {
            this.Enabled = false;
            this.Visible = false;
        }

        public void Activate()
        {
            this.Enabled = this.Visible = this.HasFocus = true;
        }

        public void Deactivate()
        {
            this.Enabled = this.Visible = this.HasFocus = false;
        }

        public override void Initialize()
        {
            m_InputManager = this.Game.Services.GetService(typeof(IInputManager)) as IInputManager;

            if (m_InputManager == null)
            {
                m_InputManager = m_DummyInputManager;
            }

            base.Initialize();

            if (m_FirstRun)
            {
                ExecuteOnFirstRun();
                m_FirstRun = false;
            }
            else
            {
                ExecuteOnReset();
            }

            InitiateScreenBoundries();
        }

        public override void Update(GameTime i_GameTime)
        {
            if (this.PreviousScreen != null && !this.IsModal)
            {
                this.PreviousScreen.Update(i_GameTime);
            }

            base.Update(i_GameTime);
        }

        public override void Draw(GameTime i_GameTime)
        {
            if (PreviousScreen != null && IsOverlayed)
            {
                PreviousScreen.Draw(i_GameTime);

                drawFadedDarkCoverIfNeeded();
            }

            base.Draw(i_GameTime);
        }

        protected virtual void ExecuteOnFirstRun()
        {
            this.Game.Window.ClientSizeChanged += (sender, args) => InitiateScreenBoundries();
        }

        protected virtual void ExecuteOnReset()
        {
        }

        protected virtual void InitiateScreenBoundries()
        {
        }

        protected virtual void OnClosed()
        {
            if (this.Closed != null)
            {
                Closed.Invoke(this, EventArgs.Empty);
            }
        }

        protected void ExitScreen()
        {
            Deactivate();
            OnClosed();
        }

        #region Faded Background Methods
        
        public bool UseGradientBackground
        {
            get { return m_UseGradientBackground; }
            set { m_UseGradientBackground = value; }
        }

        public void DrawFadedDarkCover(byte i_Alpha)
        {
            Viewport viewport = this.GraphicsDevice.Viewport;
            Texture2D background = UseGradientBackground ? m_GradientTexture : m_BlankTexture;

            SpriteBatch.Begin();
            SpriteBatch.Draw(background, new Rectangle(0, 0, viewport.Width, viewport.Height), new Color(0, 0, 0, i_Alpha));
            SpriteBatch.End();
        }
        
        protected override void LoadContent()
        {
            base.LoadContent();
            m_GradientTexture = this.ContentManager.Load<Texture2D>(@"Screens\gradient");
            m_BlankTexture = this.ContentManager.Load<Texture2D>(@"Screens\blank");
        }

        private void drawFadedDarkCoverIfNeeded()
        {
            if (BlackTintAlpha > 0 || UseGradientBackground)
            {
                DrawFadedDarkCover((byte)(m_BlackTintAlpha * byte.MaxValue));
            }
        }

        #endregion Faded Background Methods
    }
}

namespace Ex03.Infrastracture.ObjectModel.MenuItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework;
    using Ex03.Infrastracture.ServiceInterfaces;
    using Ex03.Infrastracture.ObjectModel.Animators.ConcreteAnimators;
    using Ex03.Infrastracture.ObjectModel.Animators;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public class MenuItem<SpriteType> : GameComponent, IMenuItem
        where SpriteType : Sprite
    {
        public event EventHandler MouseHover;

        public event EventHandler HasFocusChanged;

        public event EventHandler Clicked;

        public string Name { get; set; }

        public Vector2 Position
        {
            get { return BoundedSprite.Position; }
            set { BoundedSprite.Position = value; }
        }

        public float Width
        {
            get { return BoundedSprite.Width; }
        }

        public float Height
        {
            get { return BoundedSprite.Height; }
        }

        public SpriteType BoundedSprite
        {
            get { return m_BoundedSprite; }
        }

        public CompositeAnimator Animations 
        { 
            get { return BoundedSprite.Animations; } 
        }

        public Color InactiveColor
        {
            get { return m_InactiveColor; }
            set { m_InactiveColor = this.BoundedSprite.TintColor = value; }
        }

        public Color ActiveColor
        {
            get { return m_ActiveColor; }
            set { m_ActiveColor = value; }
        }

        public Keys Trigger
        {
            get { return m_Trigger; }
            set { m_Trigger = value; }
        }

        public bool Activatable
        {
            get { return m_Activatable; }
            set { m_Activatable = value; }
        }

        public bool HasFocus
        {
            get { return m_HasFocus; }
            private set
            {
                if (m_HasFocus != value)
                {
                    m_HasFocus = value;
                    OnHasFocusChanged();
                }
            }
        }

        public Rectangle BoundsForMouse
        {
            get
            {
                Vector2 pos = this.BoundedSprite.Position - this.BoundedSprite.PositionOrigin;
                return new Rectangle((int)pos.X, (int)pos.Y, (int)this.BoundedSprite.Width, (int)this.BoundedSprite.Height);
            }
        }

        public bool MouseSupport
        {
            get { return m_MouseSupport; }
            set { m_MouseSupport = value; }
        }

        public bool KeyboardSupport
        {
            get { return m_KeyboardSupport; }
            set { m_KeyboardSupport = value; }
        }

        protected SpriteType m_BoundedSprite;
        protected IInputManager m_InputManager;

        private Keys m_Trigger = Keys.Enter;
        private Color m_ActiveColor = Color.Black;
        private Color m_InactiveColor = Color.White;
        private bool m_Activatable = true;
        private bool m_HasFocus;
        private bool m_MouseSupport;
        private bool m_KeyboardSupport = true;

        public MenuItem(string i_Name, SpriteType i_BoundedSprite, bool i_Activatable)
            : base(i_BoundedSprite.Game)
        {
            Name = i_Name;
            m_BoundedSprite = i_BoundedSprite;
            m_MouseSupport = i_Activatable;
            m_KeyboardSupport = i_Activatable;
            m_Activatable = i_Activatable;
            this.Enabled = i_Activatable;
        }

        public MenuItem(string i_Name, SpriteType i_BoundedSprite, bool i_MouseSupport, bool i_KeyboardSupport, bool i_Activatable)
            : this(i_Name, i_BoundedSprite, i_Activatable)
        {
            m_KeyboardSupport = i_KeyboardSupport;
            m_MouseSupport = i_MouseSupport;
        }

        public override void Initialize()
        {
            base.Initialize();
            if (m_InputManager == null)
            {
                m_InputManager = this.Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            if (Activatable)
            {
                if (MouseSupport && this.Game.IsMouseVisible)
                {
                    if (m_InputManager.IsMouseHover(BoundsForMouse))
                    {
                        OnMouseHover();
                    }
                }

                if (HasFocus)
                {
                    ItemActive();
                }
            }
        }

        public void Activate()
        {
            if (this.Activatable)
            {
                this.HasFocus = true;
            }
        }

        public void Deactivate()
        {
            this.HasFocus = false;
        }

        protected virtual void ItemActive()
        {
            if (MouseSupport && this.Game.IsMouseVisible)
            {
                HandleMouseInput();
            }

            if (KeyboardSupport)
            {
                HandleKeyboardInput();
            }
        }

        protected virtual void HandleMouseInput()
        {
            if (m_InputManager.MouseState.LeftButton == ButtonState.Pressed
                && m_InputManager.PrevMouseState.LeftButton == ButtonState.Released)
            {
                OnClicked();
            }
        }

        protected virtual void HandleKeyboardInput()
        {
            if (m_InputManager.KeyPressed(Trigger))
            {
                OnClicked();
            }
        }

        protected virtual void OnClicked()
        {
            if (Clicked != null)
            {
                Clicked(this, EventArgs.Empty);
            }
        }

        protected virtual void OnMouseHover()
        {
            if (MouseHover != null && !HasFocus)
            {
                MouseHover(this, EventArgs.Empty);
            }
        }

        protected virtual void OnHasFocusChanged()
        {
            if (HasFocus)
            {
                this.BoundedSprite.TintColor = m_ActiveColor;

                if (!BoundedSprite.Animations.Empty)
                {
                    this.BoundedSprite.Animations.Resume();
                }
            }
            else
            {
                this.BoundedSprite.TintColor = m_InactiveColor;
                if (!BoundedSprite.Animations.Empty)
                {
                    this.BoundedSprite.Animations.Pause();
                    this.BoundedSprite.Animations.Reset();
                }
            }

            if (HasFocusChanged != null)
            {
                HasFocusChanged(this, EventArgs.Empty);
            }
        }
    }
}

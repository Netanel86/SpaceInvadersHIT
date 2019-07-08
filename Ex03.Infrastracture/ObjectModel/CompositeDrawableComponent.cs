namespace Ex03.Infrastracture.ObjectModel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Content;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public abstract class CompositeDrawableComponent<ComponentType> : DrawableGameComponent, ICollection<ComponentType>
        where ComponentType : IGameComponent
    {
        public event EventHandler<GameComponentEventArgs<ComponentType>> ComponentAdded;

        public event EventHandler<GameComponentEventArgs<ComponentType>> ComponentRemoved;

        private readonly List<ComponentType> r_UninitializedComponents;
        protected readonly List<ComponentType> r_Components;
        protected readonly List<IUpdateable> r_UpdateableComponents;
        protected readonly List<IDrawable> r_DrawableComponents;
        protected readonly List<Sprite> r_Sprites;

        public ContentManager ContentManager
        {
            get { return this.Game.Content; }
        }

        private bool m_IsInitialized = false;

        public CompositeDrawableComponent(Game i_Game)
            : base(i_Game)
        {
            r_Components = new List<ComponentType>();
            r_UninitializedComponents = new List<ComponentType>();

            r_UpdateableComponents = new List<IUpdateable>();
            r_DrawableComponents = new List<IDrawable>();
            r_Sprites = new List<Sprite>();
        }

        #region DrawableGameComponent Overrides

        public override void Initialize()
        {
            if (!m_IsInitialized)
            {
                while (r_UninitializedComponents.Count > 0)
                {
                    initializeComponent(r_UninitializedComponents[0]);
                }

                base.Initialize();
                m_IsInitialized = true;
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            m_SpriteBatch = new SpriteBatch(this.GraphicsDevice);

            foreach (Sprite sprite in r_Sprites)
            {
                sprite.SpriteBatch = m_SpriteBatch;
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            for (int i = 0; i < r_UpdateableComponents.Count; i++)
            {
                IUpdateable updatable = r_UpdateableComponents[i];
                if (updatable.Enabled)
                {
                    updatable.Update(i_GameTime);
                }
            }
        }

        public override void Draw(GameTime i_GameTime)
        {
            m_SpriteBatch.Begin(
                this.SpritesSortMode,
                this.BlendState,
                this.SamplerState,
                this.DepthStencilState,
                this.RasterizerState,
                this.Shader,
                this.TransformMatrix);

            foreach (Sprite sprite in r_Sprites)
            {
                if (sprite.Visible)
                {
                    sprite.Draw(i_GameTime);
                }
            }

            m_SpriteBatch.End();

            foreach (IDrawable drawable in r_DrawableComponents)
            {
                if (drawable.Visible)
                {
                    drawable.Draw(i_GameTime);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                for (int i = 0; i < Count; i++)
                {
                    IDisposable disposable = r_Components[i] as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }

            base.Dispose(disposing);
        }

        #endregion DrawableGameComponent Overrides

        private void initializeComponent(ComponentType i_Component)
        {
            Sprite sprite = i_Component as Sprite;
            if (sprite != null)
            {
                sprite.SpriteBatch = m_SpriteBatch;
            }

            i_Component.Initialize();
            r_UninitializedComponents.Remove(i_Component);
        }

        #region Composite Collection Extensions

        protected virtual void OnComponentAdded(GameComponentEventArgs<ComponentType> i_Args)
        {
            if (m_IsInitialized)
            {
                initializeComponent(i_Args.GameComponent);
            }
            else
            {
                r_UninitializedComponents.Add(i_Args.GameComponent);
            }

            IUpdateable updateable = i_Args.GameComponent as IUpdateable;
            if (updateable != null)
            {
                insertSorted(updateable);
                updateable.UpdateOrderChanged += new EventHandler<EventArgs>(childUpdateOrderChanged);
            }

            IDrawable drawable = i_Args.GameComponent as IDrawable;
            if (drawable != null)
            {
                insertSorted(drawable);
                drawable.DrawOrderChanged += new EventHandler<EventArgs>(childDrawOrderChanged);
            }

            if (ComponentAdded != null)
            {
                ComponentAdded(this, i_Args);
            }
        }

        protected virtual void OnComponentRemoved(GameComponentEventArgs<ComponentType> i_Args)
        {
            if (!m_IsInitialized)
            {
                r_UninitializedComponents.Remove(i_Args.GameComponent);
            }

            IUpdateable updateable = i_Args.GameComponent as IUpdateable;
            if (updateable != null)
            {
                r_UpdateableComponents.Remove(updateable);
                updateable.UpdateOrderChanged -= childUpdateOrderChanged;
            }

            Sprite sprite = i_Args.GameComponent as Sprite;
            if (sprite != null)
            {
                r_Sprites.Remove(sprite);
                sprite.DrawOrderChanged -= childDrawOrderChanged;
            }
            else
            {
                IDrawable drawable = i_Args.GameComponent as IDrawable;
                if (drawable != null)
                {
                    r_DrawableComponents.Remove(drawable);
                    drawable.DrawOrderChanged -= childDrawOrderChanged;
                }
            }

            if (ComponentRemoved != null)
            {
                ComponentRemoved(this, i_Args);
            }
        }

        protected virtual void InsertItem(int i_Idx, ComponentType i_Component)
        {
            if (r_Components.IndexOf(i_Component) != -1)
            {
                throw new ArgumentException("Duplicate components are not allowed in the same Component Manager");
            }

            if (i_Component != null)
            {
                r_Components.Insert(i_Idx, i_Component);

                OnComponentAdded(new GameComponentEventArgs<ComponentType>(i_Component));
            }
        }

        private void insertSorted(IUpdateable i_Updateable)
        {
            int idx = r_UpdateableComponents.BinarySearch(i_Updateable, UpdateableComparer.Default);
            r_UpdateableComponents.Insert(~idx, i_Updateable);
        }

        private void insertSorted(IDrawable i_Drawable)
        {
            Sprite sprite = i_Drawable as Sprite;
            if (sprite != null)
            {
                int idx = r_Sprites.BinarySearch(sprite, DrawableComparer<Sprite>.Default);
                r_Sprites.Insert(~idx, sprite);
            }
            else
            {
                int idx = r_DrawableComponents.BinarySearch(i_Drawable, DrawableComparer<IDrawable>.Default);
                r_DrawableComponents.Insert(~idx, i_Drawable);
            }
        }

        private void childUpdateOrderChanged(object i_Sender, EventArgs i_Args)
        {
            IUpdateable updateable = i_Sender as IUpdateable;

            r_UpdateableComponents.Remove(updateable);
            insertSorted(updateable);
        }

        private void childDrawOrderChanged(object i_Sender, EventArgs i_Args)
        {
            IDrawable drawable = i_Sender as IDrawable;

            Sprite sprite = i_Sender as Sprite;
            if (sprite != null)
            {
                r_Sprites.Remove(sprite);
            }
            else
            {
                r_DrawableComponents.Remove(drawable);
            }

            insertSorted(drawable);
        }

        #endregion Composite Collection Extensions

        #region ICollection Implementation

        public virtual void Add(ComponentType i_Component)
        {
            this.InsertItem(r_Components.Count, i_Component);
        }

        public void Clear()
        {
            for (int i = 0; i < Count; i++)
            {
                OnComponentRemoved(new GameComponentEventArgs<ComponentType>(r_Components[i]));
            }

            r_Components.Clear();
        }

        public bool Contains(ComponentType i_Component)
        {
            return r_Components.Contains(i_Component);
        }

        public void CopyTo(ComponentType[] io_ComponentArray, int i_ArrayIndex)
        {
            r_Components.CopyTo(io_ComponentArray, i_ArrayIndex);
        }

        public int Count
        {
            get { return r_Components.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(ComponentType i_Component)
        {
            bool removed = r_Components.Remove(i_Component);

            if (i_Component != null && removed)
            {
                OnComponentRemoved(new GameComponentEventArgs<ComponentType>(i_Component));
            }

            return removed;
        }

        public IEnumerator<ComponentType> GetEnumerator()
        {
            return r_Components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)r_Components).GetEnumerator();
        }

        #endregion Collection Implementation

        #region SpriteBatch Advanced Support

        public SpriteBatch SpriteBatch
        {
            get { return m_SpriteBatch; }
            set { m_SpriteBatch = value; }
        }

        public BlendState BlendState
        {
            get { return m_BlendState; }
            set { m_BlendState = value; }
        }

        public SpriteSortMode SpritesSortMode
        {
            get { return m_SpritesSortMode; }
            set { m_SpritesSortMode = value; }
        }

        public SamplerState SamplerState
        {
            get { return m_SamplerState; }
            set { m_SamplerState = value; }
        }

        public DepthStencilState DepthStencilState
        {
            get { return m_DepthStencilState; }
            set { m_DepthStencilState = value; }
        }

        public RasterizerState RasterizerState
        {
            get { return m_RasterizerState; }
            set { m_RasterizerState = value; }
        }

        public Effect Shader
        {
            get { return m_Shader; }
            set { m_Shader = value; }
        }

        public Matrix TransformMatrix
        {
            get { return m_TransformMatrix; }
            set { m_TransformMatrix = value; }
        }

        protected SpriteBatch m_SpriteBatch;
        protected BlendState m_BlendState = BlendState.AlphaBlend;
        protected SpriteSortMode m_SpritesSortMode = SpriteSortMode.Deferred;
        protected SamplerState m_SamplerState = null;
        protected DepthStencilState m_DepthStencilState = null;
        protected RasterizerState m_RasterizerState = null;
        protected Effect m_Shader = null;
        protected Matrix m_TransformMatrix = Matrix.Identity;
        #endregion SpriteBatch Advanced Support
    }
}

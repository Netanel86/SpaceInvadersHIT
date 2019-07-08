namespace Ex03.Infrastracture.ObjectModel.Sprites
{
    using System;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Ex03.Infrastracture;
    using Ex03.Infrastracture.ServiceInterfaces;
    using Ex03.Infrastracture.Managers;
    using Ex03.Infrastracture.Direction;
    using Ex03.Infrastracture.ObjectModel.Animators;

    public class Sprite : LoadableDrawableComponent, IBoundedComponent
    {
        #region Size And Position Properties

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)m_Position.X,
                    (int)m_Position.Y,
                    (int)this.Width,
                    (int)this.Height);
            }
        }

        public Rectangle BoundsBeforeScale
        {
            get
            {
                return new Rectangle(
                    (int)m_Position.X,
                    (int)m_Position.Y,
                    (int)m_WidthBeforeScale,
                    (int)m_HeightBeforeScale);
            }
        }

        public Vector2 Position
        {
            get { return this.m_Position; }
            set
            {
                if (m_Position != value)
                {
                    this.m_Position = value;
                    OnPositionChanged();
                }
            }
        }
        
        public Vector2 PositionOrigin
        {
            get { return m_PositionOrigin; }
            set { m_PositionOrigin = value; }
        }
        
        protected Vector2 PositionForDraw
        {
            get { return this.Position - this.PositionOrigin + this.RotationOrigin; }
        }
        
        public Vector2 Scales
        {
            get { return m_Scales; }
            set
            {
                if (m_Scales != value)
                {
                    m_Scales = value;
                    OnSizeChanged();
                }
            }
        }
        
        public float Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; }
        }
        
        public Vector2 RotationOrigin
        {
            get { return m_RotationOrigin; }
            set { m_RotationOrigin = value; }
        }
        
        public float WidthBeforeScale
        {
            get { return m_WidthBeforeScale; }
            set { m_WidthBeforeScale = value; }
        }
        
        public float HeightBeforeScale
        {
            get { return m_HeightBeforeScale; }
            set { m_HeightBeforeScale = value; }
        }
        
        public float Width
        {
            get { return m_WidthBeforeScale * m_Scales.X; }
            set { m_WidthBeforeScale = value / m_Scales.X; }
        }

        public float Height
        {
            get { return m_HeightBeforeScale * m_Scales.Y; }
            set { m_HeightBeforeScale = value / m_Scales.Y; }
        }
        
        protected Vector2 m_PositionOrigin;

        protected float m_Rotation = 0;

        protected Vector2 m_RotationOrigin = Vector2.Zero;

        protected Vector2 m_Position = Vector2.Zero;

        protected Vector2 m_Scales = Vector2.One;

        protected float m_WidthBeforeScale;

        protected float m_HeightBeforeScale;
        
        #endregion

        #region Texture Data Properties

        public bool UseSharedTexture 
        {
            get { return m_UseSharedTexture; }
            set { m_UseSharedTexture = value; }
        }

        public Texture2D Texture
        {
            get { return this.m_Texture; }
            set { this.m_Texture = value; }
        }
        
        public Color[] PixelMap
        {
            get { return m_PixelMap; }
            set { m_PixelMap = value; }
        }
        
        public Color TintColor
        {
            get { return this.m_TintColor; }
            set { this.m_TintColor = value; }
        }
        
        public float Opacity
        {
            get { return (float)m_TintColor.A / (float)byte.MaxValue; }
            set { m_TintColor.A = (byte)(value * (float)byte.MaxValue); }
        }
        
        public Rectangle SourceRectangle
        {
            get { return m_SourceRectangle; }
            set { m_SourceRectangle = value; }
        }
        
        public Vector2 SourceRectangleCenter
        {
            get { return new Vector2((float)(m_SourceRectangle.Width / 2), (float)(m_SourceRectangle.Height / 2)); }
        }

        public int SourceRectangleCount
        {
            get { return m_SourceRectangleCount; }
            set { m_SourceRectangleCount = value; }
        }

        public int SourceRectangleIdx
        {
            get { return m_SourceRectangleIdx; }
            set 
            {
                if (m_SourceRectangleIdx != value)
                {
                    m_SourceRectangleIdx = value;
                    OnSourceRectangleIdxChanged();
                }
            }
        }
        
        protected bool m_UseSharedTexture = true;
        
        protected Texture2D m_Texture;
        
        protected Rectangle m_SourceRectangle;
        
        protected int m_SourceRectangleCount = 1;
        
        protected int m_SourceRectangleIdx = 0;
        
        protected Color[] m_PixelMap;
        
        protected Color m_TintColor = Color.White;

        protected static readonly Color sr_TransparentPixel = new Color(Vector4.Zero);
        #endregion

        #region Movement Properties

        public Vector2 Velocity
        {
            get { return this.m_Velocity; }
            set { this.m_Velocity = value; }
        }

        public Direction2D MovementDirection
        {
            get { return this.m_MovementDirection; }
            set { this.m_MovementDirection = value; }
        }
        
        protected Vector2 m_Velocity = Vector2.Zero;
        
        protected Direction2D m_MovementDirection = Direction2D.NoMovement;

        #endregion

        #region Advanced SpriteBatch Options
        public SpriteBatch SpriteBatch
        {
            set
            {
                m_SpriteBatch = value;
                m_UseSharedBatch = true;
            }
        }
        
        public float LayerDepth
        {
            get { return m_LayerDepth; }
            set { m_LayerDepth = value; }
        }
        
        public SpriteEffects SpriteEffects
        {
            get { return m_SpriteEffects; }
            set { m_SpriteEffects = value; }
        }
        
        public SpriteSortMode SortMode
        {
            get { return m_SortMode; }
            set { m_SortMode = value; }
        }
        
        public BlendState BlendState
        {
            get { return m_BlendState; }
            set { m_BlendState = value; }
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
        protected float m_LayerDepth;
        protected SpriteEffects m_SpriteEffects = SpriteEffects.None;
        protected SpriteSortMode m_SortMode = SpriteSortMode.Deferred;
        protected BlendState m_BlendState = BlendState.AlphaBlend;
        protected SamplerState m_SamplerState = null;
        protected DepthStencilState m_DepthStencilState = null;
        protected RasterizerState m_RasterizerState = null;
        protected Effect m_Shader = null;
        protected Matrix m_TransformMatrix = Matrix.Identity;
        protected bool m_UseSharedBatch = true;
        #endregion Advanced SpriteBatch Options
        
        protected CompositeAnimator m_Animations;
        
        public CompositeAnimator Animations
        {
            get { return m_Animations; }
            set { m_Animations = value; }
        }
 
        public Sprite(Game i_Game, string i_AssetName)
            : base(i_Game, i_AssetName)
        {
            m_MovementDirection = new Direction2D();
        }

        public Sprite(Game i_Game, string i_AssetName, bool i_SharedTexture)
            : this(i_Game, i_AssetName)
        {
            this.UseSharedTexture = i_SharedTexture;
        }

        public Sprite(Game i_Game, string i_AssetName, Color i_Color)
            : this(i_Game, i_AssetName)
        {
            this.m_TintColor = i_Color;
        }

        public Sprite(Game i_Game, string i_AssetName, int i_UpdateOrder, int i_DrawOrder)
            : base(i_Game, i_AssetName, i_UpdateOrder, i_DrawOrder)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            InitPixelMap();
            m_Animations = new CompositeAnimator(this);
        }
        
        public override void Update(GameTime i_GameTime)
        {
            this.UpdatePosition(i_GameTime);

            base.Update(i_GameTime);
            this.Animations.Update(i_GameTime);
        }

        public override void Draw(GameTime i_GameTime)
        {
            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.Begin(this.SortMode, this.BlendState, this.SamplerState, this.DepthStencilState, this.RasterizerState, this.Shader, this.TransformMatrix);
            }

            m_SpriteBatch.Draw(
                m_Texture,
                this.PositionForDraw,
                this.SourceRectangle,
                this.TintColor,
                this.Rotation,
                this.RotationOrigin,
                this.Scales,
                SpriteEffects.None,
                this.LayerDepth);

            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.End();
            }
        }

        public override void InitBounds()
        {
            m_HeightBeforeScale = m_Texture.Height;
            m_WidthBeforeScale = m_Texture.Width / m_SourceRectangleCount;
            InitSourceRectangle();
            InitOrigins();
        }
        
        public Sprite ShallowClone()
        {
            return this.MemberwiseClone() as Sprite;
        }
        
        protected virtual void InitOrigins()
        {
            this.RotationOrigin = this.SourceRectangleCenter;
        }

        public virtual bool CheckCollision(ICollidable i_Source)
        {
            bool collided = false;
            ICollidable2D source = i_Source as ICollidable2D;
            if (source != null)
            {
                if (this.Bounds.Intersects(source.Bounds))
                {
                    collided = true;
                }
            }

            return collided;
        }

        public virtual bool CheckCollisionPerPixel(ICollidablePerPixel i_Source)
        {
            bool isCollided = false;

            IntersectRectangle intersectionRect = GetIntersectionBounds(i_Source.Bounds, this.Bounds);

            for (int y = intersectionRect.YMin; y < intersectionRect.YMax; y++)
            {
                for (int x = intersectionRect.XMin; x < intersectionRect.XMax; x++)
                {
                    int sourcePixelMapIdx = (int)(i_Source.Bounds.Width * (y - i_Source.Bounds.Y)) + (x - i_Source.Bounds.X);
                    int thisPixelMapIdx = (int)(this.Width * (y - (int)this.Position.Y)) + (x - (int)this.Position.X);

                    Color colorSource = i_Source.PixelMap[sourcePixelMapIdx];
                    Color colorThis = this.PixelMap[thisPixelMapIdx];

                    if (colorSource.A != 0 && colorThis.A != 0)
                    {
                        isCollided = true;
                        if (i_Source.PointOfImpact == null)
                        {
                            i_Source.PointOfImpact = new Vector2(x, y);
                        }

                        break;
                    }
                }

                if (isCollided)
                {
                    break;
                }
            }

            return isCollided;
        }
        
        protected override void LoadContent()
        {
            m_Texture = this.ContentManager.Load<Texture2D>(this.m_AssetName);

            if (m_SpriteBatch == null)
            {
                m_SpriteBatch = this.Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
                if (m_SpriteBatch == null)
                {
                    this.Game.Services.AddService(typeof(SpriteBatch), m_SpriteBatch = new SpriteBatch(this.Game.GraphicsDevice));
                }

                m_UseSharedBatch = false;
            }
        }
        
        protected Texture2D DuplicateTextureData(Texture2D i_Source, out Color[] o_PixelMap)
        {
            Texture2D dupTexture = new Texture2D(this.GraphicsDevice, i_Source.Width, i_Source.Height);

            Color[] pixelmap = new Color[i_Source.Width * i_Source.Height];

            i_Source.GetData<Color>(pixelmap);
            dupTexture.SetData<Color>(pixelmap);

            o_PixelMap = pixelmap;
            return dupTexture;
        }
        
        /// <summary>
        /// checks this <paramref name="PixelMap"/> for any non-transparent pixel's.
        /// </summary>
        /// <returns>
        /// true - if this texture pixel map is fully transparent, false otherwise
        /// </returns>
        protected bool CheckThisTextureTransparency()
        {
            bool isFullyTransparent = false;
            int transparentCounter = 0;

            foreach (Color pixel in this.PixelMap)
            {
                if (pixel != sr_TransparentPixel)
                {
                    isFullyTransparent = false;
                    break;
                }
                else if (++transparentCounter == this.PixelMap.Length)
                {
                    isFullyTransparent = true;
                    break;
                }
            }

            return isFullyTransparent;
        }
        
        protected IntersectRectangle GetIntersectionBounds(Rectangle i_RectA, Rectangle i_RectB)
        {
            int xMin = (int)MathHelper.Max(i_RectA.X, i_RectB.X);
            int xMax = (int)MathHelper.Min(i_RectA.X + i_RectA.Width, i_RectB.X + i_RectB.Width);
            int yMin = (int)MathHelper.Max(i_RectA.Y, i_RectB.Y);
            int yMax = (int)MathHelper.Min(i_RectA.Y + i_RectA.Height, i_RectB.Y + i_RectB.Height);
            return new IntersectRectangle(xMin, yMin, xMax, yMax);
        }
        
        protected virtual void UpdatePosition(GameTime i_GameTime)
        {
            this.m_Position.X += m_Velocity.X * (float)this.m_MovementDirection.XAxis * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            this.m_Position.Y += m_Velocity.Y * (float)this.m_MovementDirection.YAxis * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            this.OnPositionChanged();
        }
        
        protected virtual void InitPixelMap()
        {
            if (this is ICollidablePerPixel && m_PixelMap == null)
            {
                if (!m_UseSharedTexture)
                {
                    m_Texture = DuplicateTextureData(m_Texture, out m_PixelMap);
                }
                else
                {
                    m_PixelMap = new Color[(int)(m_WidthBeforeScale * m_HeightBeforeScale)];
                    this.Texture.GetData<Color>(0, m_SourceRectangle, this.PixelMap, 0, (int)(this.WidthBeforeScale * this.HeightBeforeScale));
                }
            }
        }
        
        protected virtual void InitSourceRectangle()
        {
            m_SourceRectangle = new Rectangle(m_SourceRectangleIdx * (int)m_WidthBeforeScale, 0, (int)m_WidthBeforeScale, (int)m_HeightBeforeScale);
        }

        protected virtual void OnSourceRectangleIdxChanged()
        {
        }

        public class IntersectRectangle
        {
            public int XMin { get; set; }

            public int XMax { get; set; }

            public int YMin { get; set; }

            public int YMax { get; set; }

            public IntersectRectangle(int i_XMin, int i_YMin, int i_XMax, int i_YMax)
            {
                this.XMin = i_XMin;
                this.YMin = i_YMin;
                this.XMax = i_XMax;
                this.YMax = i_YMax;
            }
        }
    }
}

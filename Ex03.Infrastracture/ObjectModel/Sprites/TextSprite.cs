namespace Ex03.Infrastracture.ObjectModel.Sprites
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;

    public class TextSprite : Sprite
    {
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        protected string m_FontName;
        protected SpriteFont m_Font;

        private string m_Text = string.Empty;

        public TextSprite(Game i_Game, string i_FontName)
            : base(i_Game, i_FontName)
        {
            m_FontName = i_FontName;
        }

        public TextSprite(Game i_Game, string i_FontName, string i_Text)
            : this(i_Game, i_FontName)
        {
            m_Text = i_Text;
        }

        public override void Draw(GameTime i_GameTime)
        {
            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.Begin(this.SortMode, this.BlendState, this.SamplerState, this.DepthStencilState, this.RasterizerState, this.Shader, this.TransformMatrix);
            }

            m_SpriteBatch.DrawString(
                m_Font,
                this.Text,
                this.PositionForDraw,
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
            if (m_Text != string.Empty)
            {
                Vector2 stringSize = m_Font.MeasureString(m_Text);
                m_WidthBeforeScale = stringSize.X;
                m_HeightBeforeScale = stringSize.Y;
                InitOrigins();
            }
        }

        protected override void LoadContent()
        {
            this.m_Font = this.Game.Content.Load<SpriteFont>(string.Format(@"Fonts\{0}", m_FontName));
        }

        protected override void InitOrigins()
        {
            m_PositionOrigin = new Vector2(this.Width / 2, this.Height / 2);
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            InitBounds();
        }
    }
}

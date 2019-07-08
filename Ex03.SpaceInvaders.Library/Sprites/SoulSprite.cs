namespace Ex03.SpaceInvaders.Library.Sprites
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
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public class SoulSprite : Sprite
    {
        public PlayerIndex PlayerIdx { get; set; }
        
        public int NumberOfSouls 
        {
            get { return m_NumberOfSouls; }
            set { m_NumberOfSouls = value; }
        }

        private new List<Vector2> m_Position;
        private int m_NumberOfSouls;
        
        public SoulSprite(Game i_Game, string i_AssetName)
            : base(i_Game, i_AssetName)
        {
            m_Position = new List<Vector2>();
            this.DrawOrder = int.MaxValue;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Opacity = 0.5f;
            this.Game.Window.ClientSizeChanged += (sender, args) => InitBounds();
        }
        
        public override void Draw(GameTime i_GameTime)
        {
            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.Begin();
            }

            for (int i = 0; i < NumberOfSouls; i++)
            {
                m_SpriteBatch.Draw(m_Texture, m_Position[i], this.SourceRectangle, this.TintColor, 0, Vector2.Zero, this.Scales, SpriteEffects.None, 1);
            }

            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.End();
            }
        }
        
        public override void InitBounds()
        {
            this.Scales = new Vector2(0.5f);

            m_WidthBeforeScale = m_Texture.Width / 2;
            m_HeightBeforeScale = m_Texture.Height;

            m_Position.Clear();
            for (int i = 0; i < NumberOfSouls; i++)
            {
                Vector2 position;
                position.X = this.Game.GraphicsDevice.Viewport.Width - (1.5f * this.Width ) - (1.5f * this.Width * i);
                position.Y = (this.Height / 2) + (1.5f * this.Height * (int)PlayerIdx);
                m_Position.Add(position);
            }
        }
    }
}

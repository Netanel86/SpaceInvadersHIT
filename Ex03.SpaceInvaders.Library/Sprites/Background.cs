namespace Ex03.SpaceInvaders.Library.Sprites
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public class Background : Sprite
    {
        private GraphicsDeviceManager m_Graphics;
        
        public Background(Game i_Game, string i_AssetName)
            : base(i_Game, i_AssetName)
        {
            m_Graphics = this.Game.Services.GetService(typeof(IGraphicsDeviceManager)) as GraphicsDeviceManager;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.BlendState = Microsoft.Xna.Framework.Graphics.BlendState.AlphaBlend;
            this.Game.Window.ClientSizeChanged += (sender, args) => this.InitBounds();
        }

        public override void InitBounds()
        {
            m_Position.X = 0;
            m_Position.Y = 0;
            m_WidthBeforeScale = this.Game.GraphicsDevice.Viewport.Width;
            m_HeightBeforeScale = this.Game.GraphicsDevice.Viewport.Height;

            this.InitSourceRectangle();
            this.DrawOrder = int.MinValue;
        }
        
        public override void Draw(GameTime i_GameTime)
        {
            m_SpriteBatch.Begin();
            m_SpriteBatch.Draw(this.Texture, SourceRectangle, TintColor);
            m_SpriteBatch.End();
        }
    }
}

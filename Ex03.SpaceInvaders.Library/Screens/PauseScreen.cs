namespace Ex03.SpaceInvaders.Library.Screens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Ex03.Infrastracture.ObjectModel.Screens;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Graphics;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public class PauseScreen : GameScreen
    {
        private Sprite m_Paused;
        private Sprite m_Resume;

        public PauseScreen(Game i_Game)
            : base(i_Game)
        {
            this.IsModal = true;
            this.IsOverlayed = true;
            this.BlackTintAlpha = 0.55f;

            this.Add(m_Paused = new Sprite(this.Game, @"Menus\Paused_680x100"));
            this.Add(m_Resume = new Sprite(this.Game, @"Menus\Resume_420x50"));
        }

        public override void Initialize()
        {
            base.Initialize();
            Viewport viewport = this.GraphicsDevice.Viewport;

            m_Paused.Position = new Vector2(viewport.Width / 2, viewport.Height / 4);
            m_Paused.PositionOrigin = m_Paused.SourceRectangleCenter;
            m_Paused.TintColor = Color.Orange;
            m_Resume.Position = new Vector2(viewport.Width / 2, viewport.Height / 2);
            m_Resume.PositionOrigin = m_Resume.SourceRectangleCenter;
            m_Resume.TintColor = Color.LightCoral;
        }

        public override void Update(GameTime i_GameTime)
        {
            if (this.InputManager.KeyPressed(Keys.R))
            {
                this.ExitScreen();
            }

            base.Update(i_GameTime);
        }
    }
}

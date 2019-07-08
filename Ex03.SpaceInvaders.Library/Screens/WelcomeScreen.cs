namespace Ex03.SpaceInvaders.Library.Screens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Graphics;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ObjectModel.Screens;
    using Ex03.SpaceInvaders.Library.Sprites;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public class WelcomeScreen : GameScreen
    {
        private MainMenu m_Menu;

        private Sprite m_Logo;
        private Sprite m_StartMessege;

        public WelcomeScreen(Game i_Game, MainMenu i_Menu)
            : base(i_Game)
        {
            m_Menu = i_Menu;
            this.Add(m_Logo = new Sprite(this.Game, @"Menus\Welcome\InvadersLogo_780x115"));
            this.Add(m_StartMessege = new Sprite(this.Game, @"Menus\Welcome\StartMessege_475x160"));
        }

        public override void Initialize()
        {
            base.Initialize();
            Viewport viewport = this.GraphicsDevice.Viewport;
            
            m_Logo.Position = new Vector2(viewport.Width / 2, viewport.Height / 4);
            m_Logo.PositionOrigin = m_Logo.SourceRectangleCenter;
            m_Logo.TintColor = Color.MediumVioletRed;

            m_StartMessege.Position = new Vector2(viewport.Width / 2, viewport.Height - (m_StartMessege.Height * 1.5f));
            m_StartMessege.PositionOrigin = m_StartMessege.SourceRectangleCenter;
            m_StartMessege.TintColor = Color.Navy;
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (this.InputManager.KeyPressed(Keys.Enter))
            {
                this.ExitScreen();
            }

            if(this.InputManager.KeyPressed(Keys.O))
            {
                this.ExitScreen();
                this.ScreenManager.SetCurrentScreen(m_Menu);
            }

            if(this.InputManager.KeyPressed(Keys.Escape))
            {
                this.Game.Exit();
            }
        }
    }
}

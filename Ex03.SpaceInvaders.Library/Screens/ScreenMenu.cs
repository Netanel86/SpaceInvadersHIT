namespace Ex03.SpaceInvaders.Library.Screens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Ex03.Infrastracture.ObjectModel.Screens;
    using Microsoft.Xna.Framework;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ObjectModel.MenuItems;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public class ScreenMenu : MenuScreenWithAudio
    {
        private GraphicsDeviceManager m_Graphics;

        private TextSprite m_Head;
        private TextSprite m_MouseVisability;
        private TextSprite m_WindowResize;
        private TextSprite m_FullScreen;
        private TextSprite m_Done;

        private TextToggleMenuItem m_ToggleMouse;
        private TextToggleMenuItem m_ToggleFullScreen;
        private TextToggleMenuItem m_ToggleResize;

        public ScreenMenu(Game i_Game)
            : base(i_Game)
        {
            m_Graphics = this.Game.Services.GetService(typeof(IGraphicsDeviceManager)) as GraphicsDeviceManager;
            this.Add(m_Head = new TextSprite(this.Game, "CalibriHead", "Screen Options"));
            this.Add(m_MouseVisability = new TextSprite(this.Game, "Calibri", "Mouse Visability: "));
            this.Add(m_WindowResize = new TextSprite(this.Game, "Calibri", "Allow Window Resizing: "));
            this.Add(m_FullScreen = new TextSprite(this.Game, "Calibri", "Full Screen Mode: "));
            this.Add(m_Done = new TextSprite(this.Game, "Calibri", "Done"));

            this.Add(new TextMenuItem("Head", m_Head, !k_Activatable));
            this.Add(m_ToggleMouse = new TextToggleMenuItem("MouseVisability", m_MouseVisability, 0, k_Activatable, "Visible", "Invisible"));
            this.Add(m_ToggleResize = new TextToggleMenuItem("WindowResize", m_WindowResize, 0, k_Activatable, "On", "Off"));
            this.Add(m_ToggleFullScreen = new TextToggleMenuItem("FullScreen", m_FullScreen, 0, k_Activatable, "On", "Off"));
            this.Add(new TextMenuItem("Done", m_Done, k_Activatable));
        }

        public override void Initialize()
        {
            base.Initialize();

            m_Head.Scales = new Vector2(1.5f);

            bindItems();
        }

        protected override void ExecuteOnFirstRun()
        {
            base.ExecuteOnFirstRun();

            this.MenuItemsDictionary["MouseVisability"].Clicked += (sender, args) => this.Game.IsMouseVisible = !this.Game.IsMouseVisible;
            this.MenuItemsDictionary["WindowResize"].Clicked += (sender, args) => this.Game.Window.AllowUserResizing = !this.Game.Window.AllowUserResizing;
            this.MenuItemsDictionary["FullScreen"].Clicked += (sender, args) => m_Graphics.ToggleFullScreen();
            this.MenuItemsDictionary["Done"].Clicked += (sender, args) => this.ExitScreen();
        }

        private void bindItems()
        {
            if (!this.Game.IsMouseVisible)
            {
                m_ToggleMouse.CurrentToggle = 1;
            }

            if (!this.Game.Window.AllowUserResizing)
            {
                m_ToggleResize.CurrentToggle = 1;
            }

            if (!m_Graphics.IsFullScreen)
            {
                m_ToggleFullScreen.CurrentToggle = 1;
            }
        }
    }
}

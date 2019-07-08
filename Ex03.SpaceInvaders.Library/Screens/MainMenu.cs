namespace Ex03.SpaceInvaders.Library.Screens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ObjectModel.Screens;
    using Ex03.SpaceInvaders.Library.ServiceInterfaces;
    using Ex03.SpaceInvaders.Library.Sprites.Entities;
    using Ex03.Infrastracture.ObjectModel.MenuItems;
    using Ex03.Infrastracture.ObjectModel.Sprites;
    using Ex03.Infrastracture.ServiceInterfaces;

    public class MainMenu : MenuScreenWithAudio
    {
        private int m_PlayerCount = 1;
        private TextSprite m_MenuHead;
        private TextSprite m_ScreenOptions;
        private TextSprite m_SoundOptions;
        private TextSprite m_Players;
        private TextSprite m_Play;
        private TextSprite m_Quit;

        private ScreenMenu m_ScreenMenu;
        private SoundMenu m_SoundMenu;

        public MainMenu(Game i_Game)
            : base(i_Game)
        {
            m_ScreenMenu = new ScreenMenu(this.Game);
            m_SoundMenu = new SoundMenu(this.Game);

            this.Add(m_MenuHead = new TextSprite(this.Game, "CalibriHead", "Main Menu"));
            this.Add(m_ScreenOptions = new TextSprite(this.Game, "Calibri", "Screen Options"));
            this.Add(m_SoundOptions = new TextSprite(this.Game, "Calibri", "Sound Options"));
            this.Add(m_Play = new TextSprite(this.Game, "Calibri", "Play"));
            this.Add(m_Quit = new TextSprite(this.Game, "Calibri", "Quit"));
            this.Add(m_Players = new TextSprite(this.Game, "Calibri", "Players:"));

            this.Add(new MenuItem<TextSprite>("Head", m_MenuHead, !k_Activatable));
            this.Add(new TextMenuItem("ScreenOptions", m_ScreenOptions, k_Activatable));
            this.Add(new TextMenuItem("SoundOptions", m_SoundOptions, k_Activatable));
            this.Add(new TextToggleMenuItem("Players", m_Players, 0, k_Activatable, "One", "Two"));
            this.Add(new TextMenuItem("Play", m_Play, k_Activatable));
            this.Add(new TextMenuItem("Quit", m_Quit, k_Activatable));
        }

        public override void Initialize()
        {
            base.Initialize();
            m_MenuHead.Scales = new Vector2(1.5f);
        }

        protected override void ExecuteOnFirstRun()
        {
            base.ExecuteOnFirstRun();

            this.MenuItemsDictionary["Players"].Clicked += (sender, args) => m_PlayerCount = (sender as TextToggleMenuItem).CurrentToggle + 1;
            this.MenuItemsDictionary["Play"].Clicked += play_Clicked;
            this.MenuItemsDictionary["Quit"].Clicked += (sender, args) => this.Game.Exit();
            this.MenuItemsDictionary["ScreenOptions"].Clicked += (sender, args) => this.ScreenManager.SetCurrentScreen(m_ScreenMenu);
            this.MenuItemsDictionary["SoundOptions"].Clicked += (sender, args) => this.ScreenManager.SetCurrentScreen(m_SoundMenu);
        }

        private void play_Clicked(object i_Sender, EventArgs i_Args)
        {
            IPlayScreen playScreen = this.Game.Services.GetService(typeof(IPlayScreen)) as IPlayScreen;

            while (playScreen.CurrentPlayerCount < m_PlayerCount)
            {
                playScreen.Add(new SpaceCraft(this.Game, @"Sprites\Ships_64x32"));
            }

            this.ExitScreen();
            this.Dispose();
        }
    }
}
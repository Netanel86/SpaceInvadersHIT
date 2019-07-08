namespace Ex03.SpaceInvaders.Library.Screens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ObjectModel.Screens;
    using Ex03.Infrastracture.ObjectModel.MenuItems;
    using Ex03.Infrastracture.ObjectModel.Sprites;
    using Ex03.Infrastracture.ServiceInterfaces;
    using Microsoft.Xna.Framework.Input;

    public class SoundMenu : MenuScreenWithAudio
    {
        private TextSprite m_Head;
        private TextSprite m_Sound;
        private TextSprite m_BackgroundVol;
        private TextSprite m_EffectsVol;
        private TextSprite m_Done;

        private TextToggleMenuItem m_ToggleSound;
        private ValueToggleMenuItem m_ToggleBackgroundVol;
        private ValueToggleMenuItem m_ToggleEffectsVol;

        public SoundMenu(Game i_Game)
            : base(i_Game)
        {
            this.Add(m_Head = new TextSprite(this.Game, "CalibriHead", "Sound Options"));
            this.Add(m_Sound = new TextSprite(this.Game, "Calibri", "Toggle Sound: "));
            this.Add(m_BackgroundVol = new TextSprite(this.Game, "Calibri", "Background Music Volume: "));
            this.Add(m_EffectsVol = new TextSprite(this.Game, "Calibri", "Sound Effects Volume: "));
            this.Add(m_Done = new TextSprite(this.Game, "Calibri", "Done"));

            this.Add(new TextMenuItem("Head", m_Head, !k_Activatable));
            this.Add(m_ToggleSound = new TextToggleMenuItem("ToggleSound", m_Sound, 0, k_Activatable, "Off", "On"));
            this.Add(m_ToggleBackgroundVol = new ValueToggleMenuItem("Music", m_BackgroundVol, 100, 0, 10, k_Activatable));
            this.Add(m_ToggleEffectsVol = new ValueToggleMenuItem("SoundFX", m_EffectsVol, 100, 0, 10, k_Activatable));
            this.Add(new TextMenuItem("Done", m_Done, k_Activatable));
        }

        public override void Initialize()
        {
            base.Initialize();
            m_Head.Scales = new Vector2(1.5f);

            bindItems();
        }

        private void bindItems()
        {
            if (!m_AudioManager.SoundEnabled)
            {
                m_ToggleSound.CurrentToggle = 0;
            }
        }

        protected override void ExecuteOnFirstRun()
        {
            base.ExecuteOnFirstRun();
            this.MenuItemsDictionary["Done"].Clicked += (sender, args) => this.ExitScreen();
            this.MenuItemsDictionary["ToggleSound"].Clicked += (sender, args) => m_AudioManager.ToggleSound();
            this.MenuItemsDictionary["Music"].Clicked += volumeItem_Clicked;
            this.MenuItemsDictionary["SoundFX"].Clicked += volumeItem_Clicked;
            m_ToggleSound.CurrentToggle = 1;
            m_ToggleBackgroundVol.CurrentToggle = 100;
            m_ToggleEffectsVol.CurrentToggle = 100;
        }

        private void volumeItem_Clicked(object i_Sender, EventArgs i_Args)
        {
            ValueToggleMenuItem volItem = i_Sender as ValueToggleMenuItem;

            if (volItem != null)
            {
                m_AudioManager.SetCategoryVolume(volItem.Name, volItem.CurrentToggle);
                if (!m_AudioManager.SoundEnabled)
                {
                    m_AudioManager.SetCategoryVolume(volItem.Name, 0);
                }
            }
        }
    }
}

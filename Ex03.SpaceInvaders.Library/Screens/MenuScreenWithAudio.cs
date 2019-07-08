namespace Ex03.SpaceInvaders.Library.Screens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Ex03.Infrastracture.ServiceInterfaces;
    using Microsoft.Xna.Framework;
    using Ex03.Infrastracture.ObjectModel.Screens;

    public class MenuScreenWithAudio : MenuScreen
    {
        protected IAudioManager m_AudioManager;
        
        public MenuScreenWithAudio(Game i_Game)
            : base(i_Game)
        {
        }

        protected override void ExecuteOnFirstRun()
        {
            base.ExecuteOnFirstRun();
            m_AudioManager = this.Game.Services.GetService(typeof(IAudioManager)) as IAudioManager;
        }

        protected override void OnActiveItemChanged(EventArgs i_Args)
        {
            base.OnActiveItemChanged(i_Args);

            m_AudioManager.Play("MenuMove");
        }
    }
}

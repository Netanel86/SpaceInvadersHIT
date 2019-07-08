using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ex03.Infrastracture.ObjectModel;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Ex03.Infrastracture.ServiceInterfaces;

namespace Ex03.Infrastracture.Managers.Audio
{
    /// <summary>
    /// Abstract sound manager to work with Xact
    /// </summary>
    public abstract class AudioManager : GameService
    {
        protected AudioEngine m_AudioEngine;
        protected WaveBank m_WaveBank;
        protected SoundBank m_SoundBank;
        protected List<SoundCategory> m_Categories;

        public AudioManager(Game i_Game, AudioEngine i_AudioEngine, WaveBank i_WaveBank, SoundBank i_SoundBank)
            : base(i_Game)
        {
            m_Categories = new List<SoundCategory>();
            m_AudioEngine = i_AudioEngine;
            m_WaveBank = i_WaveBank;
            m_SoundBank = i_SoundBank;

            initialize();
        }

        private void initialize()
        {
            InitializeSoundCategoryList(m_Categories);
        }

        protected abstract void InitializeSoundCategoryList(List<SoundCategory> i_SoundCategoryList);

        protected override void RegisterAsService()
        {
            Game.Services.AddService(typeof(IAudioManager), this);
        }
    }
}

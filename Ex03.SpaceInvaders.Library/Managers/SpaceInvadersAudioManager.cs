using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ex03.Infrastracture.Managers.Audio;
using Ex03.Infrastracture.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Ex03.SpaceInvaders.Library.Managers
{
    public class SpaceInvadersAudioManager : AudioManager, IAudioManager
    {
        public bool SoundEnabled
        {
            get { return m_ToggleSound; }
        }

        private bool m_ToggleSound = true;
        private SoundCategory m_MusicCategory;
        private SoundCategory m_SoundFXCategory;

        public SpaceInvadersAudioManager(Game i_Game, AudioEngine i_AudioEngine, WaveBank i_WaveBank, SoundBank i_SoundBank)
            : base(i_Game, i_AudioEngine, i_WaveBank, i_SoundBank)
        {
        }

        protected override void InitializeSoundCategoryList(List<SoundCategory> i_SoundCategoryList)
        {
            m_MusicCategory = new SoundCategory("Music");
            m_SoundFXCategory = new SoundCategory("SoundFX");

            i_SoundCategoryList.Add(m_MusicCategory);
            i_SoundCategoryList.Add(m_SoundFXCategory);

            foreach (SoundCategory category in m_Categories)
            {
                SetCategoryVolume(category.Name, category.Volume);
            }
        }

        public void Play(string i_Sound)
        {
            m_SoundBank.PlayCue(i_Sound);
        }

        public void ToggleSound()
        {
            m_ToggleSound = !m_ToggleSound;

            if (m_ToggleSound)
            {
                enableSound();
            }
            else
            {
                disableSound();
            }
        }

        private void disableSound()
        {
            foreach (SoundCategory category in m_Categories)
            {
                m_AudioEngine.GetCategory(category.Name).SetVolume(0);
            }
        }

        private void enableSound()
        {
            foreach (SoundCategory category in m_Categories)
            {
                m_AudioEngine.GetCategory(category.Name).SetVolume(category.PrevVolume);
            }
        }

        public void SetCategoryVolume(string i_SoundCategory, float i_Volume)
        {
            SoundCategory soundCategory = m_Categories.Find((category) => category.Name.Equals(i_SoundCategory, StringComparison.CurrentCultureIgnoreCase));
            if (soundCategory != null)
            {
                soundCategory.PrevVolume = soundCategory.Volume;
                soundCategory.Volume = MathHelper.Clamp(i_Volume, 0, 100);

                m_AudioEngine.GetCategory(soundCategory.Name).SetVolume(i_Volume);
            }
        }
    }
}

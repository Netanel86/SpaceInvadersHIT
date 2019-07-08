using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex03.Infrastracture.ServiceInterfaces
{
    public interface IAudioManager
    {
        bool SoundEnabled { get; }
        
        void Play(string i_Sound);
        
        void SetCategoryVolume(string i_SoundCategory, float i_Volume);
        
        void ToggleSound();
    }
}

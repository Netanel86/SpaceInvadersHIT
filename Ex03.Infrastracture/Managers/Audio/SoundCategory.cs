using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex03.Infrastracture.Managers.Audio
{
    public class SoundCategory
    {
        public string Name { get; private set; }
        
        public float PrevVolume { get; set; }

        private float m_Volume;
        
        public float Volume
        {
            get { return m_Volume; }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    m_Volume = value;
                }
            }
        }

        public SoundCategory(string i_Name)
        {
            Name = i_Name;
            Volume = 100;
            PrevVolume = 100;
        }
    }
}

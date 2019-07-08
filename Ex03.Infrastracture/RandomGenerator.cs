namespace Ex03.Infrastracture
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class RandomGenerator
    {
        public static RandomGenerator Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new RandomGenerator();
                }

                return m_Instance;
            }
        }
        
        private static RandomGenerator m_Instance;

        private Random m_Generator;

        private RandomGenerator()
        {
            m_Generator = new Random();
        }

        public bool RunGenerator(float i_SuccessChance)
        {
            i_SuccessChance.ThrowIfNotInRange(0, 100);

            bool isInRange = false;
            if (m_Generator.Next(0, int.MaxValue) <= i_SuccessChance * (int.MaxValue / 100))
            {
                isInRange = true;
            }

            return isInRange;
        }
    }
}

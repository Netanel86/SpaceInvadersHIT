namespace Ex03.Infrastracture.ObjectModel.Sprites
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public class MultiDigitSprite : MultiBoundedComponent<DigitSprite>
    {
        public int Number
        {
            get { return m_Number; }
        }

        private int m_Number;

        public MultiDigitSprite(Game i_Game, string i_AssetName, int i_Number)
            : base(i_Game)
        {
            Stack<int> digits = new Stack<int>();
            m_Number = i_Number;
            int remain = i_Number % 10;
            int devision = i_Number / 10;

            while (remain != 0 || devision != 0)
            {
                digits.Push(remain);
                remain = devision % 10;
                devision = devision / 10;
            }

            while (digits.Count != 0)
            {
                this.Add(new DigitSprite(this.Game, i_AssetName, digits.Pop()));
            }
        }
    }
}

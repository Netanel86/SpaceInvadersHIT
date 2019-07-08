namespace Ex03.Infrastracture.ObjectModel.Sprites
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public class DigitSprite : Sprite
    {
        private static readonly List<Rectangle> r_DigitSourceRectangles;

        private static List<Rectangle> rs_Size72Font
        {
            get
            {
                return new List<Rectangle>() 
                { 
                    new Rectangle(419, 0, 47, 66), 
                    new Rectangle(0, 0, 42, 65), 
                    new Rectangle(43, 0, 45, 65), 
                    new Rectangle(89, 0, 43, 66), 
                    new Rectangle(132, 0, 52, 64), 
                    new Rectangle(184, 0, 43, 68), 
                    new Rectangle(228, 0, 47, 66), 
                    new Rectangle(275, 0, 47, 64), 
                    new Rectangle(322, 0, 48, 66), 
                    new Rectangle(370, 0, 48, 66) 
                };
            }
        }

        private static List<Rectangle> rs_Size28Font
        {
            get
            {
                return new List<Rectangle>() 
                { 
                    new Rectangle(163, 0, 18, 25), 
                    new Rectangle(0, 0, 16, 25), 
                    new Rectangle(16, 0, 18, 25), 
                    new Rectangle(34, 0, 17, 25), 
                    new Rectangle(51, 0, 20, 25), 
                    new Rectangle(71, 0, 17, 26), 
                    new Rectangle(88, 0, 19, 25), 
                    new Rectangle(107, 0, 18, 25), 
                    new Rectangle(125, 0, 19, 25), 
                    new Rectangle(144, 0, 19, 25) 
                };
            }
        }

        static DigitSprite()
        {
            r_DigitSourceRectangles = rs_Size72Font;
        }

        private int m_Number;

        public DigitSprite(Game i_Game, string i_AssetName, int i_Number)
            : base(i_Game, i_AssetName)
        {
            m_Number = i_Number;
        }

        public override void InitBounds()
        {
            m_HeightBeforeScale = r_DigitSourceRectangles[m_Number].Height;
            m_WidthBeforeScale = r_DigitSourceRectangles[m_Number].Width;
            InitSourceRectangle();
            InitOrigins();
        }

        protected override void InitSourceRectangle()
        {
            this.SourceRectangle = r_DigitSourceRectangles[m_Number];
        }

        protected override void OnSourceRectangleIdxChanged()
        {
            this.SourceRectangle = r_DigitSourceRectangles[SourceRectangleIdx];
        }
    }
}

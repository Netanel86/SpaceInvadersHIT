namespace Ex03.Infrastracture.ObjectModel.MenuItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public class TextMenuItem : MenuItem<TextSprite>
    {
        public TextMenuItem(string i_Name, TextSprite i_BoundedSprite, bool i_Activatable)
            : base(i_Name, i_BoundedSprite, i_Activatable)
        {
        }
    }
}

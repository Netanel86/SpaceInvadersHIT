namespace Ex03.Infrastracture.ObjectModel.MenuItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class MenuItemEventArgs : EventArgs
    {
        public IMenuItem MenuItem { get; private set; }

        public MenuItemEventArgs(IMenuItem i_Item)
        {
            MenuItem = i_Item;
        }
    }
}

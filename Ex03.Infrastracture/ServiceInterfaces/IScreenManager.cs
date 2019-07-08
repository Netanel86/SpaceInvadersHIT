namespace Ex03.Infrastracture.ServiceInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Ex03.Infrastracture.ObjectModel.Screens;

    public interface IScreenManager
    {
        GameScreen ActiveScreen { get; }
        
        void SetCurrentScreen(GameScreen i_NewScreen);
        
        bool Remove(GameScreen i_Screen);
        
        void Add(GameScreen i_Screen);
    }
}

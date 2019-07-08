namespace Ex03.SpaceInvaders.Library.ServiceInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public interface IPlayScreen
    {
        int CurrentPlayerCount { get; }
        
        void Add(IGameComponent i_Component);
        
        void ResetNextLevel(int i_Level);
        
        void Deactivate();
        
        void Activate();
    }
}

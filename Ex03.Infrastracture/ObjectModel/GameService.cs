namespace Ex03.Infrastracture.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public abstract class GameService : GameComponent
    {
        public GameService(Game i_Game)
            : base(i_Game)
        {
            RegisterAsService();
        }

        protected abstract void RegisterAsService();
    }
}

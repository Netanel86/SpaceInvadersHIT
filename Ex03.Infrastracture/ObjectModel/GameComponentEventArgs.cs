namespace Ex03.Infrastracture.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public class GameComponentEventArgs<ComponentType> : EventArgs
        where ComponentType : IGameComponent
    {
        public ComponentType GameComponent
        {
            get { return m_Component; }
        }

        private ComponentType m_Component;

        public GameComponentEventArgs(ComponentType i_Component)
        {
            m_Component = i_Component;
        }
    }
}

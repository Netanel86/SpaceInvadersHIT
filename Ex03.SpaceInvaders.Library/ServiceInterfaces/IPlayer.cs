namespace Ex03.SpaceInvaders.Library.ServiceInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public interface IPlayer
    {
        event EventHandler<EventArgs> Disposed;

        event EventHandler<EventArgs> SoulsChanged;

        event EventHandler<EventArgs> EnabledChanged;

        Rectangle SourceRectangle { get; }

        PlayerIndex PlayerIdx { get; }

        bool Enabled { get; }

        int Souls { get; }
    }
}

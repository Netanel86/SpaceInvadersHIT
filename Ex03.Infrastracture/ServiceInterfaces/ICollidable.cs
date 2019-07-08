namespace Ex03.Infrastracture.ServiceInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using Microsoft.Xna.Framework;

    public interface ICollidable
    {
        event EventHandler<EventArgs> Disposed;
        
        event EventHandler<EventArgs> SizeChanged;
        
        event EventHandler<EventArgs> PositionChanged;
        
        event EventHandler<EventArgs> VisibleChanged;
        
        event EventHandler<EventArgs> EnabledChanged;

        bool Enabled { get; }
        
        bool Visible { get; }
        
        bool CheckCollision(ICollidable i_Source);
        
        void Collided(ICollidable i_CollidedComponent);
    }
}

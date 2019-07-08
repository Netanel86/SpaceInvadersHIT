namespace Ex03.SpaceInvaders.Library.ServiceInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IInterceptable
    {
        event EventHandler<EventArgs> Disposed;

        event EventHandler<EventArgs> Intercepted;

        int Score { get; }
    }
}

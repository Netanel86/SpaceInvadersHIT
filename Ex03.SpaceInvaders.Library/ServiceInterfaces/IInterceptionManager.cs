namespace Ex03.SpaceInvaders.Library.ServiceInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IInterceptionManager
    {
        void AttachInterceptable(IInterceptable i_Interceptable);
    }
}

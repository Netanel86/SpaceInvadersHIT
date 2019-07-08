namespace Ex03.SpaceInvaders.Library.ServiceInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Ex03.Infrastracture.ObjectModel;

    public interface IInvaderManager
    {
        /// <summary>
        /// Raised when one of the invader's handled by the 
        /// manager leaves the boundries of the viewport.
        /// </summary>
        event Action OutOfScreenBoundries;

        /// <summary>
        /// Raised when there are no invader's left.
        /// </summary>
        event Action GroupCountZero;

        event EventHandler<TimeSpanArgs> GroupSpeedChanged;
    }
    
    public class TimeSpanArgs : EventArgs
    {
        public TimeSpan TimeSpan { get; set; }
        
        public TimeSpanArgs(TimeSpan i_TimeSpan)
        {
            this.TimeSpan = i_TimeSpan;
        }
    }
}

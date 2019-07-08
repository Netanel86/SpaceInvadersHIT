namespace Ex03.Infrastracture.ObjectModel.MenuItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ObjectModel.Animators;

    public interface IMenuItem : IUpdateable, IGameComponent
    {
        /// <summary>
        /// Raised when the mouse pointer intersects with this item boundries
        /// </summary>
        /// <remarks>
        /// event only raised if <paramref name="UsingMouse"/> is enabled
        /// </remarks>
        event EventHandler MouseHover;
        
        /// <summary>
        /// Raised when this item <paramref name="HasFocus"/> is changed
        /// </summary>
        event EventHandler HasFocusChanged;
        
        /// <summary>
        /// Raised when this menu item is selected
        /// </summary>
        event EventHandler Clicked;
        
        string Name { get; set; }
        
        bool Activatable { get; set; }
        
        Color ActiveColor { get; set; }
        
        Color InactiveColor { get; set; }

        CompositeAnimator Animations { get; }

        bool HasFocus { get; }
        
        Vector2 Position { get; set; }
        
        float Width { get; }
        
        float Height { get; }
        
        void Activate();
        
        void Deactivate();
    }
}

namespace Ex03.SpaceInvaders.Library.Inputs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Ex03.Infrastracture.ServiceInterfaces;
    using Microsoft.Xna.Framework;

    public interface IPlayerInput
    {
        IInputManager InputManager { get; set; }
        
        Vector2 PositionDelta { get; }
        
        bool LeftKey { get; }
        
        bool RightKey { get; }
        
        bool FireKey { get; }

        void CheckForUserInput();
    }

    public abstract class PlayerInput : IPlayerInput
    {
        public IInputManager InputManager { get; set; }

        public bool LeftKey { get; private set; }
        
        public bool RightKey { get; private set; }
        
        public bool FireKey { get; private set; }

        public Vector2 PositionDelta { get; protected set; }

        protected bool m_isLeftPressed;
        protected bool m_isRightPressed;
        protected bool m_isFirePressed;

        public PlayerInput()
        {
        }

        public void CheckForUserInput()
        {
            const bool v_Pressed = true;

            if (InputManager != null)
            {
                SetKeysState();

                this.LeftKey = m_isLeftPressed ? v_Pressed : !v_Pressed;
                this.RightKey = m_isRightPressed ? v_Pressed : !v_Pressed;
                this.FireKey = m_isFirePressed ? v_Pressed : !v_Pressed;
            }
            else
            {
                throw new NullReferenceException("InputManager is null, set the property before initializing");
            }
        }

        /// <summary>
        /// user injection point for wraping the user input,
        /// using the <paramref name="InputManager"/>.
        /// </summary>
        /// <remarks>
        /// set the three protected boolean members:
        /// m_isLeftPressed,m_isRightPressed,m_isFirePressed
        /// with its equivelant input keys.
        /// </remarks>
        protected abstract void SetKeysState();
    }
}

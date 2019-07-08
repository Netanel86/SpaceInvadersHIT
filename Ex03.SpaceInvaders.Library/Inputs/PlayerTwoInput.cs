namespace Ex03.SpaceInvaders.Library.Inputs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Input;

    public class PlayerTwoInput : PlayerInput
    {
        public PlayerTwoInput()
        {
        }

        protected override void SetKeysState()
        {
            this.m_isLeftPressed = InputManager.KeyPressed(Keys.A) || InputManager.KeyHeld(Keys.A);
            this.m_isRightPressed = InputManager.KeyPressed(Keys.D) || InputManager.KeyHeld(Keys.D);
            this.m_isFirePressed = InputManager.KeyPressed(Keys.S);
        }
    }
}

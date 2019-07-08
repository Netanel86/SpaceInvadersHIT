namespace Ex03.SpaceInvaders.Library.Inputs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Input;

    public class PlayerOneInput : PlayerInput
    {
        public PlayerOneInput()
        {
        }

        protected override void SetKeysState()
        {
            this.m_isLeftPressed = InputManager.KeyPressed(Keys.Left) || InputManager.KeyHeld(Keys.Left);
            this.m_isRightPressed = InputManager.KeyPressed(Keys.Right) || InputManager.KeyHeld(Keys.Right);
            this.m_isFirePressed =
                InputManager.MouseState.LeftButton == ButtonState.Pressed && InputManager.PrevMouseState.LeftButton == ButtonState.Released;

            this.PositionDelta = InputManager.MousePositionDelta;
        }
    }
}

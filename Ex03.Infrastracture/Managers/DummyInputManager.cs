namespace Ex03.Infrastracture.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Ex03.Infrastracture.ServiceInterfaces;
using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework;

    public class DummyInputManager : IInputManager
    {
        #region Devices States
        public GamePadState GamePadState 
        { 
            get { return m_DummyPadState; } 
        }
        
        public GamePadState PrevGamePadState
        {
            get { return m_DummyPadState; }
        }
        
        public KeyboardState KeyboardState 
        { 
            get { return m_DummyKeyboardState; } 
        }
        
        public KeyboardState PrevKeyboardState
        {
            get { return m_DummyKeyboardState; }
        }
        
        public MouseState MouseState 
        { 
            get { return m_DummyMouseState; } 
        }
        
        public MouseState PrevMouseState
        {
            get { return m_DummyMouseState; }
        }

        private GamePadState m_DummyPadState = new GamePadState();
        private KeyboardState m_DummyKeyboardState = new KeyboardState();
        private MouseState m_DummyMouseState = new MouseState();
        #endregion Devices States

        #region Mouse & GamePad Current States
        public bool ButtonIsDown(eInputButtons i_MouseButtons) 
        { 
            return false; 
        }
        
        public bool ButtonIsUp(eInputButtons i_MouseButtons) 
        { 
            return true; 
        }
        
        public bool ButtonsAreDown(eInputButtons i_MouseButtons) 
        { 
            return false; 
        }
        
        public bool ButtonsAreUp(eInputButtons i_MouseButtons) 
        { 
            return true; 
        }
        #endregion Mouse & GamePad Current States

        #region Input Devices State Changes
        public bool ButtonPressed(eInputButtons i_Buttons) 
        { 
            return false; 
        }
        
        public bool ButtonReleased(eInputButtons i_Buttons) 
        { 
            return false; 
        }
        
        public bool ButtonsPressed(eInputButtons i_Buttons) 
        { 
            return false; 
        }
        
        public bool ButtonsReleased(eInputButtons i_Buttons) 
        { 
            return false; 
        }

        public bool KeyPressed(Keys i_Key) 
        { 
            return false; 
        }
        
        public bool KeyReleased(Keys i_Key) 
        { 
            return false; 
        }
        
        public bool KeyHeld(Keys i_Key) 
        { 
            return false; 
        }
        #endregion Input Devices State Changes

        #region Input Devices Delta's
        public Vector2 MousePositionDelta 
        { 
            get { return Vector2.Zero; } 
        }
        
        public int ScrollWheelDelta 
        { 
            get { return 0; } 
        }
        
        public bool IsMouseHover(Rectangle i_RegionBounds)
        {
            return false;
        }
        
        public Vector2 LeftThumbDelta 
        { 
            get { return Vector2.Zero; } 
        }
        
        public Vector2 RightThumbDelta 
        { 
            get { return Vector2.Zero; } 
        }
        
        public float LeftTrigerDelta 
        { 
            get { return 0; } 
        }
        
        public float RightTrigerDelta 
        { 
            get { return 0; } 
        }
        #endregion Input Devices Delta's
    }
}

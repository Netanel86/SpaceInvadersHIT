namespace Ex03.Infrastracture.ObjectModel.MenuItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Input;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public class ToggleMenuItem<SpriteType> : MenuItem<SpriteType>
        where SpriteType : Sprite
    {
        public int CurrentToggle
        {
            get { return m_CurrentToggle; }
            set 
            { 
                m_CurrentToggle = value;
                Initialize();
            }
        }
        
        public Keys NextKey
        {
            get { return m_NextKey; }
            set { m_NextKey = value; }
        }
        
        public Keys PreviousKey
        {
            get { return m_PreviousKey; }
            set { m_PreviousKey = value; }
        }
        
        protected const bool k_Changed = true;
        protected bool m_isChanged = false;
        protected int m_CurrentToggle;
        
        private int m_ToggleCount;
        private int m_DefaultToggle;
        private int m_Interval;

        private Keys m_NextKey = Keys.PageUp;
        private Keys m_PreviousKey = Keys.PageDown;

        public ToggleMenuItem(string i_Name, SpriteType i_BoundedSprite, int i_ToggleOptionsCount, int i_DefaultToggleOption, int i_Interval, bool i_Activatable)
            : base(i_Name, i_BoundedSprite, i_Activatable)
        {
            m_ToggleCount = i_ToggleOptionsCount;
            m_Interval = i_Interval;
            m_DefaultToggle = i_DefaultToggleOption;
        }

        protected override void ItemActive()
        {
            base.ItemActive();

            if (m_isChanged)
            {
                OnClicked();
                m_isChanged = !k_Changed;
            }
        }

        protected override void HandleKeyboardInput()
        {
            if (m_InputManager.KeyPressed(NextKey))
            {
                Next();
            }

            if (m_InputManager.KeyPressed(PreviousKey))
            {
                Previous();
            }
        }

        protected override void HandleMouseInput()
        {
            if (m_InputManager.ScrollWheelDelta != 0)
            {
                if (m_InputManager.ScrollWheelDelta > 0)
                {
                    Next();
                }
                else
                {
                    Previous();
                }
            }

            if (m_InputManager.MouseState.RightButton == ButtonState.Pressed
                && m_InputManager.PrevMouseState.RightButton == ButtonState.Released)
            {
                Next();
            }
        }

        protected virtual void Next()
        {
            m_CurrentToggle += m_Interval;
            if (m_CurrentToggle >= m_ToggleCount)
            {
                m_CurrentToggle = m_DefaultToggle;
            }

            m_isChanged = k_Changed;
        }

        protected virtual void Previous()
        {
            m_CurrentToggle -= m_Interval;
            if (m_CurrentToggle < 0)
            {
                m_CurrentToggle = m_ToggleCount - 1;
            }

            m_isChanged = k_Changed;
        }
    }
}

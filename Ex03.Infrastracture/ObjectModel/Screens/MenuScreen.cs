namespace Ex03.Infrastracture.ObjectModel.Screens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Graphics;
    using Ex03.Infrastracture.ServiceInterfaces;
    using Ex03.Infrastracture.ObjectModel.Animators.ConcreteAnimators;
    using Ex03.Infrastracture.ObjectModel.MenuItems;

    public class MenuScreen : GameScreen
    {
        /// <summary>
        /// Raised when the active item on the menu is changed
        /// </summary>
        public event EventHandler ActiveItemChanged;

        public Dictionary<string, IMenuItem> MenuItemsDictionary
        {
            get { return r_MenuItemsDictionary; }
        }

        protected const bool k_UseMouse = true;
        protected const bool k_Activatable = true;
        protected const bool k_HasFocus = true;
        protected int m_SelectedIdx;

        private readonly List<IMenuItem> r_MenuItems;
        private readonly Dictionary<string, IMenuItem> r_MenuItemsDictionary;
        private IMenuItem m_CurrentItemSelected;
        private Keys m_LastKeyInput;
        private int m_OptionsCount;
        
        public MenuScreen(Game i_Game)
            : base(i_Game)
        {
            r_MenuItems = new List<IMenuItem>();
            r_MenuItemsDictionary = new Dictionary<string, IMenuItem>();
        }
        
        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (InputManager.KeyPressed(Keys.Down))
            {
                m_SelectedIdx++;
                m_LastKeyInput = Keys.Down;
                keyboard_ActiveItemChange();
            }

            if (InputManager.KeyPressed(Keys.Up))
            {
                m_SelectedIdx--;
                m_LastKeyInput = Keys.Up;
                keyboard_ActiveItemChange();
            }
        } 
        
        public void Add(IMenuItem i_Component)
        {
            base.Add(i_Component);
            IMenuItem item = i_Component as IMenuItem;
            if (item != null)
            {
                m_OptionsCount++;
                r_MenuItems.Add(item);
                if (item.Activatable)
                {
                    r_MenuItemsDictionary.Add(item.Name, item);
                }
            }
        }
        
        protected override void ExecuteOnFirstRun()
        {
            base.ExecuteOnFirstRun();
            
            initItemsAnimation();
            this.Game.Window.ClientSizeChanged += (sender, args) => ResetMenu();
        }

        protected override void InitiateScreenBoundries()
        {
            Viewport viewport = this.Game.GraphicsDevice.Viewport;

            IMenuItem lastSprite = null;
            bool defaultSelection = true;
            for (int i = 0; i < r_MenuItems.Count; i++)
            {
                Vector2 position = new Vector2();

                if (lastSprite == null)
                {
                    position.X = viewport.Width / 2;
                    position.Y = r_MenuItems[i].Height;
                    r_MenuItems[i].InactiveColor = Color.Red;
                }
                else
                {
                    r_MenuItems[i].ActiveColor = Color.Sienna;
                    r_MenuItems[i].InactiveColor = Color.Navy;

                    if (defaultSelection)
                    {
                        m_SelectedIdx = i;
                        m_CurrentItemSelected = r_MenuItems[i];
                        m_CurrentItemSelected.Activate();
                        defaultSelection = false;
                    }

                    r_MenuItems[i].MouseHover += new EventHandler(menuItem_MouseHover);
                    position.X = viewport.Width / 2;
                    position.Y = lastSprite.Position.Y + lastSprite.Height + 20;
                }

                r_MenuItems[i].Position = position;
                lastSprite = r_MenuItems[i];
            }
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            ResetMenu();
        }

        protected virtual void OnActiveItemChanged(EventArgs i_Args)
        {
            if (ActiveItemChanged != null)
            {
                ActiveItemChanged(this, i_Args);
            }
        }

        protected void ResetMenu()
        {
            foreach (IMenuItem item in r_MenuItems)
            {
                item.Deactivate();
            }

            m_SelectedIdx = 1;
            m_CurrentItemSelected = this.r_MenuItems[m_SelectedIdx];
        }

        private void initItemsAnimation()
        {
            foreach (IMenuItem item in r_MenuItemsDictionary.Values)
            {
                item.Animations.Add(new PulseAnimator("PulseAnimator", TimeSpan.Zero, 1.2f, 1.5f));
                item.Animations.Enabled = false;
            }
        }
        
        private void keyboard_ActiveItemChange()
        {
            m_CurrentItemSelected.Deactivate();
            clampSelectedIndex();

            while (!r_MenuItems[m_SelectedIdx].Activatable)
            {
                if (m_LastKeyInput == Keys.Up)
                {
                    m_SelectedIdx--;
                }

                if (m_LastKeyInput == Keys.Down)
                {
                    m_SelectedIdx++;
                }

                clampSelectedIndex();
            }

            m_CurrentItemSelected = r_MenuItems[m_SelectedIdx];
            m_CurrentItemSelected.Activate();
            OnActiveItemChanged(new MenuItemEventArgs(m_CurrentItemSelected));
        }
        
        private void mouse_ActiveItemChange(IMenuItem i_Item)
        {
            m_CurrentItemSelected.Deactivate();
            m_CurrentItemSelected = i_Item;
            m_SelectedIdx = r_MenuItems.IndexOf(i_Item, 0, r_MenuItems.Count);
            m_CurrentItemSelected.Activate();
            OnActiveItemChanged(new MenuItemEventArgs(m_CurrentItemSelected));
        }
        
        private void menuItem_MouseHover(object i_Sender, EventArgs i_Args)
        {
            IMenuItem item = i_Sender as IMenuItem;
            if (item != null)
            {
                mouse_ActiveItemChange(item);
            }
        }

        private void clampSelectedIndex()
        {
            if (m_SelectedIdx >= m_OptionsCount)
            {
                m_SelectedIdx = 0;
            }
            else if (m_SelectedIdx < 0)
            {
                m_SelectedIdx = m_OptionsCount - 1;
            }
        }
    }
}

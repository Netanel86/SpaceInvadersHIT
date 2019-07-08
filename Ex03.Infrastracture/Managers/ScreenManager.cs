namespace Ex03.Infrastracture.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ObjectModel.Screens;
    using Ex03.Infrastracture.ServiceInterfaces;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class ScreenManager : CompositeDrawableComponent<GameScreen>, IScreenManager
    {
        public ScreenManager(Game i_Game)
            : base(i_Game)
        {
            i_Game.Components.Add(this);
            this.Game.Services.AddService(typeof(IScreenManager), this);
        }

        private readonly Stack<GameScreen> r_ScreenStack = new Stack<GameScreen>();

        public GameScreen ActiveScreen
        {
            get { return r_ScreenStack.Count != 0 ? r_ScreenStack.Peek() : null; }
        }

        public void SetCurrentScreen(GameScreen i_NewScreen)
        {
            if (ActiveScreen != null)
            {
                ActiveScreen.Deactivate();
            }

            Push(i_NewScreen);
            i_NewScreen.Activate();
        }
        
        public void Push(GameScreen i_Screen)
        {
            i_Screen.ScreenManager = this;

            if (!this.Contains(i_Screen))
            {
                this.Add(i_Screen);
                i_Screen.Closed += new EventHandler(screen_Closed);
            }

            if (ActiveScreen != i_Screen)
            {
                if (ActiveScreen != null)
                {
                    i_Screen.PreviousScreen = ActiveScreen;

                    ActiveScreen.Deactivate();
                }

                r_ScreenStack.Push(i_Screen);
            }

            i_Screen.DrawOrder = r_ScreenStack.Count;
        }

        public void Pop()
        {
            r_ScreenStack.Pop();
            if (r_ScreenStack.Count != 0)
            {
                ActiveScreen.Activate();
            }
        }

        protected override void OnComponentRemoved(GameComponentEventArgs<GameScreen> i_Args)
        {
            base.OnComponentRemoved(i_Args);

            i_Args.GameComponent.Closed -= screen_Closed;

            if (r_ScreenStack.Count == 0)
            {
                this.Game.Exit();
            }
        }

        private void screen_Closed(object sender, EventArgs e)
        {
            this.Pop();
            this.Remove(sender as GameScreen);
        }
    }
}
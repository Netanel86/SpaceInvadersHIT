namespace Ex03.SpaceInvaders.Library.Screens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Ex03.Infrastracture.Managers;
    using Microsoft.Xna.Framework;
    using Ex03.Infrastracture.ObjectModel.Screens;
    using Ex03.SpaceInvaders.Library.ServiceInterfaces;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ObjectModel.Animators.ConcreteAnimators;
    using Microsoft.Xna.Framework.Input;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public class GameOverScreen : GameScreen, IGameOverScreen
    {
        public string GameEndMessege
        {
            get { return m_EndMessage.Text; }
            set
            {
                m_EndMessage.Text = value;
                m_EndMessage.InitBounds();
                InitiateScreenBoundries();
            }
        }

        private TextSprite m_EndMessage;
        private TextSprite m_Options;
        private Sprite m_GameOver;
        
        private PlayScreen m_PlayScreen;
        private MainMenu m_Menu;

        public GameOverScreen(Game i_Game, PlayScreen i_PlayScreen, MainMenu i_Menu)
            : base(i_Game)
        {
            this.Game.Services.AddService(typeof(IGameOverScreen), this);
            m_PlayScreen = i_PlayScreen;
            m_Menu = i_Menu;
            this.Add(m_GameOver = new Sprite(this.Game, @"Menus\GameOver\GameOver_296x48"));
            this.Add(m_EndMessage = new TextSprite(this.Game, "Calibri"));
            this.Add(m_Options = new TextSprite(this.Game, "Calibri"));
        }

        public override void Initialize()
        {
            base.Initialize();

            m_GameOver.Animations.Add(new PulseAnimator("PulseAnimator", TimeSpan.Zero, 1.3f, 1.5f));
            m_GameOver.Animations.Enabled = true;
            m_GameOver.TintColor = Color.MediumVioletRed;
            m_GameOver.Opacity = 0.8f;

            m_EndMessage.TintColor = Color.BlanchedAlmond;
            m_EndMessage.Scales = new Vector2(0.7f);

            m_Options.Text = @"'Esc' To Exit
'S' To Start New Game
'M' To Return To Menu";
            m_Options.InitBounds();
            m_Options.TintColor = Color.Red;
        }
        
        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (this.InputManager.KeyPressed(Keys.S))
            {
                ScreenManager.SetCurrentScreen(m_PlayScreen);
                ScreenManager.SetCurrentScreen(new LevelTransitionScreen(this.Game, 0));
            }

            if (this.InputManager.KeyPressed(Keys.M))
            {
                ScreenManager.SetCurrentScreen(m_PlayScreen);
                ScreenManager.SetCurrentScreen(new LevelTransitionScreen(this.Game, 0));
                ScreenManager.SetCurrentScreen(m_Menu);
            }

            if (this.InputManager.KeyPressed(Keys.Escape))
            {
                this.Game.Exit();
            }
        }
        
        protected override void InitiateScreenBoundries()
        {
            m_GameOver.Position = new Vector2(this.GraphicsDevice.Viewport.Width / 2, m_GameOver.Height * 2);
            m_GameOver.PositionOrigin = m_GameOver.SourceRectangleCenter;

            m_EndMessage.Position = new Vector2(this.GraphicsDevice.Viewport.Width / 2, m_GameOver.Height * 5);

            m_Options.Position = new Vector2(this.GraphicsDevice.Viewport.Width / 2, m_EndMessage.Position.Y + (m_Options.Height * 1.5f));
        }
    }
}

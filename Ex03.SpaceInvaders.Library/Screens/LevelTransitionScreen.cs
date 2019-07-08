namespace Ex03.SpaceInvaders.Library.Screens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Ex03.Infrastracture.ObjectModel.Screens;
    using Microsoft.Xna.Framework;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ObjectModel.Animators;
    using Ex03.Infrastracture.ObjectModel.Animators.ConcreteAnimators;
    using Ex03.Infrastracture.Direction;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public class LevelTransitionScreen : GameScreen
    {
        private Sprite m_Seconds;
        private MultiBoundedComponent<IBoundedComponent> m_Level;
        private MultiDigitSprite m_Number;

        public LevelTransitionScreen(Game i_Game, int i_Level)
            : base(i_Game)
        {
            this.Add(m_Level = new MultiBoundedComponent<IBoundedComponent>(this.Game));
            m_Level.Add(
                new Sprite(this.Game, @"Menus\Level\Level_226x61"),
                m_Number = new MultiDigitSprite(this.Game, @"Menu\Numbers_466x68", i_Level + 1));
                
            this.Add(m_Seconds = new DigitSprite(this.Game, @"Menu\Numbers_466x68", 3));

            m_Number.Gap = 5;
            m_Level.Gap = 40;
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (m_Seconds.Animations.IsFinished)
            {
                m_Seconds.Animations.Pause();
                m_Seconds.Animations.Reset();
                this.ExitScreen();
                this.Dispose();
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            m_Seconds.Animations.Enabled = true;
        }

        protected override void ExecuteOnFirstRun()
        {
            base.ExecuteOnFirstRun();
            
            m_Seconds.Animations.Add(
                new CompositeAnimator(
                    "TimeCountAnimator", 
                    TimeSpan.FromSeconds(3f), 
                    m_Seconds,
                    new CellAnimator(TimeSpan.FromSeconds(1f), 3, 1, TimeSpan.FromSeconds(3f), false, eDirectionX.Left),
                    new PulseAnimator("PulseAnimator", TimeSpan.FromSeconds(3f), 1.5f, 1f)));
            
            m_Seconds.Animations.ResetAfterFinish = true;
        }

        protected override void InitiateScreenBoundries()
        {
            m_Level.Position = new Vector2(this.GraphicsDevice.Viewport.Width / 2, this.GraphicsDevice.Viewport.Height / 4);
            m_Seconds.Position = new Vector2(this.GraphicsDevice.Viewport.Width / 2, (this.GraphicsDevice.Viewport.Height / 2) + m_Seconds.Height);
            m_Seconds.PositionOrigin = m_Seconds.SourceRectangleCenter;
        }
    }
}

namespace Ex03.SpaceInvaders.Library.Sprites
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.GamerServices;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public class ScoreBoard : TextSprite
    {
        public ScoreBoard(Game i_Game, string i_FontName)
            : base(i_Game, i_FontName)
        {
        }

        public PlayerIndex PlayerIdx { get; set; }

        public long TargetScore { get; private set; }

        private long m_CurrentScore;
        
        public override void Initialize()
        {
            this.Text = string.Format("P{0} Score: 0", (int)PlayerIdx);
            base.Initialize();
        }

        public override void Update(GameTime i_GameTime)
        {
            if (m_CurrentScore < TargetScore)
            {
                m_CurrentScore += 5;
            }
            else if (TargetScore < m_CurrentScore && m_CurrentScore > 0)
            {
                m_CurrentScore -= 5;
            }

            this.Text = string.Format("P{0} Score: {1}", (int)PlayerIdx + 1, m_CurrentScore);
        }

        public override void InitBounds()
        {
            base.InitBounds();
            m_Position.X = this.PositionOrigin.X;
            m_Position.Y = this.PositionOrigin.Y + (1.5f * this.Height * (int)PlayerIdx);
        }

        public void UpdateScoreBoard(int i_AddToScore)
        {
            if (i_AddToScore < 0 && Math.Abs(i_AddToScore) > TargetScore)
            {
                this.TargetScore = 0;
            }
            else
            {
                this.TargetScore += i_AddToScore;
            }
        }
    }
}

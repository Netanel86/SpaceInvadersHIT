namespace Ex03.Infrastracture.ObjectModel.Animators.ConcreteAnimators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public class OpacityAnimator : SpriteAnimator
    {
        private float m_OpacitySpeed;

        public OpacityAnimator(string i_Name, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            m_OpacitySpeed = 1 / (float)this.AnimationLength.TotalSeconds;
        }

        public OpacityAnimator(string i_Name, float i_OpacitySpeed, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            m_OpacitySpeed = i_OpacitySpeed;
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Opacity = this.m_OriginalSpriteInfo.Opacity;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            if (this.BoundSprite.Opacity > 0)
            {
                this.BoundSprite.Opacity -= (float)(m_OpacitySpeed * i_GameTime.ElapsedGameTime.TotalSeconds);
            }
        }
    }
}

namespace Ex03.Infrastracture.ObjectModel.Animators.ConcreteAnimators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Scales Sprite objects
    /// </summary>
    public class ScaleAnimator : SpriteAnimator
    {
        private float m_ScaleSpeed;

        public ScaleAnimator(string i_Name, float i_ScaleSpeed, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            m_ScaleSpeed = i_ScaleSpeed;
        }

        public ScaleAnimator(string i_Name, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            m_ScaleSpeed = 1 / (float)i_AnimationLength.TotalSeconds;
        }
        
        protected override void RevertToOriginal()
        {
            this.BoundSprite.Scales = this.m_OriginalSpriteInfo.Scales;
            this.BoundSprite.Position = this.m_OriginalSpriteInfo.Position;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            this.BoundSprite.Scales -= new Vector2(m_ScaleSpeed * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}

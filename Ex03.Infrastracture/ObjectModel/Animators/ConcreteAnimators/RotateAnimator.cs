namespace Ex03.Infrastracture.ObjectModel.Animators.ConcreteAnimators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Rotates Sprite objects from the center of the object
    /// </summary>
    public class RotateAnimator : SpriteAnimator
    {
        private float m_RotationSpeed;

        public RotateAnimator(string i_Name, float i_RotationSpeed, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            m_RotationSpeed = i_RotationSpeed;
        }

        public RotateAnimator(string i_Name, int i_RotationsPerSecond, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            m_RotationSpeed = MathHelper.TwoPi * i_RotationsPerSecond;
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.RotationOrigin = this.m_OriginalSpriteInfo.RotationOrigin;
            this.BoundSprite.Rotation = this.m_OriginalSpriteInfo.Rotation;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            this.BoundSprite.Rotation += m_RotationSpeed * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}

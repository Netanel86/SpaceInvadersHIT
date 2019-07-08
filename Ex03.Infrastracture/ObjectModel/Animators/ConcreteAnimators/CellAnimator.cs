namespace Ex03.Infrastracture.ObjectModel.Animators.ConcreteAnimators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Ex03.Infrastracture.Direction;

    public class CellAnimator : SpriteAnimator
    {
        public TimeSpan CellTime
        {
            get { return m_CellTime; }

            set { m_CellTime = value; }
        }

        private TimeSpan m_CellTime;
        private TimeSpan m_TimeLeftForCell;
        private bool m_Loop = true;
        private int m_CurrCellIdx = 0;
        private int m_StartCellIdx = 0;
        private int m_MinCellIdx;
        private int m_MaxCellIdx;
        private bool m_EvenSources;
        private eDirectionX m_Direction = eDirectionX.Right;

        private CellAnimator(TimeSpan i_CellTime, TimeSpan i_AnimationLength, bool i_EvenSources)
            : base("CellAnimator", i_AnimationLength)
        {
            m_EvenSources = i_EvenSources;
            m_CellTime = m_TimeLeftForCell = i_CellTime;
            m_Loop = i_AnimationLength == TimeSpan.Zero;
        }

        public CellAnimator(TimeSpan i_CellTime, int i_StartCellIdx, int i_MinCellIdx, int i_MaxCellIdx, TimeSpan i_AnimationLength, bool i_EvenSources)
            : this(i_CellTime, i_AnimationLength, i_EvenSources)
        {
            this.m_StartCellIdx = i_StartCellIdx;
            this.m_CurrCellIdx = i_StartCellIdx;
            this.m_MinCellIdx = i_MinCellIdx;
            this.m_MaxCellIdx = i_MaxCellIdx;
        }

        public CellAnimator(TimeSpan i_CellTime, int i_FromCellIdx, int i_ToCellIdx, TimeSpan i_AnimationLength, bool i_EvenSources, eDirectionX i_Direction)
            : this(i_CellTime, i_AnimationLength, i_EvenSources)
        {
            m_Direction = i_Direction;

            m_CurrCellIdx = m_StartCellIdx = i_FromCellIdx;
            m_MaxCellIdx = (int)MathHelper.Max(i_FromCellIdx, i_ToCellIdx);
            m_MinCellIdx = (int)MathHelper.Min(i_FromCellIdx, i_ToCellIdx);
            m_Loop = i_AnimationLength == TimeSpan.Zero;
        }

        public CellAnimator(TimeSpan i_CellTime, int i_NumOfCells, TimeSpan i_AnimationLength)
            : this(i_CellTime, 0, 0, i_NumOfCells, i_AnimationLength, true)
        {
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.SourceRectangle = m_OriginalSpriteInfo.SourceRectangle;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            if (m_CellTime != TimeSpan.Zero)
            {
                m_TimeLeftForCell -= i_GameTime.ElapsedGameTime;
                if (m_TimeLeftForCell.TotalSeconds <= 0)
                {
                    /// we have elapsed, so blink
                    goToNextFrame();
                    m_TimeLeftForCell = m_CellTime;
                }
            }

            if (m_EvenSources)
            {
                this.BoundSprite.SourceRectangle = new Rectangle(
                    m_CurrCellIdx * this.BoundSprite.SourceRectangle.Width,
                    this.BoundSprite.SourceRectangle.Top,
                    this.BoundSprite.SourceRectangle.Width,
                    this.BoundSprite.SourceRectangle.Height);
            }
            else
            {
                this.BoundSprite.SourceRectangleIdx = m_CurrCellIdx;
            }
        }

        private void goToNextFrame()
        {
            m_CurrCellIdx += (int)m_Direction;

            if (m_CurrCellIdx < m_MinCellIdx)
            {
                if (m_Loop)
                {
                    m_CurrCellIdx = m_MaxCellIdx;
                }
                else
                {
                    m_CurrCellIdx = m_MinCellIdx;
                    this.IsFinished = true;
                }
            }

            if (m_CurrCellIdx > m_MaxCellIdx)
            {
                if (m_Loop)
                {
                    m_CurrCellIdx = m_MinCellIdx;
                }
                else
                {
                    m_CurrCellIdx = m_MaxCellIdx;
                    this.IsFinished = true;
                }
            }
        }
    }
}

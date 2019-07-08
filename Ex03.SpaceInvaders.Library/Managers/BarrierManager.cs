namespace Ex03.SpaceInvaders.Library.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Ex03.Infrastracture.ObjectModel;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;
    using Ex03.SpaceInvaders.Library.Sprites.Entities;
    using Ex03.Infrastracture.Direction;

    public class BarrierManager : CompositeDrawableComponent<Barrier>
    {
        public Vector2 Velocity
        {
            get { return m_Velocity; }
            set
            {
                if (m_Velocity != value)
                {
                    m_Velocity = value;
                    OnGroupVelocityChange();
                }
            }
        }

        private Texture2D m_TextureBarrier;
        private int m_DistanceMoved;
        private Vector2 m_LastPosition;
        private Vector2 m_Velocity;

        public BarrierManager(Game i_Game, int i_Size)
            : base(i_Game)
        {
            CreateBarrierLine(i_Size);
        }

        public override void Initialize()
        {
            base.Initialize();
            InitiateBarrierLine();
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (r_Components.Count != 0)
            {
                Barrier firstBarrier = r_Components[0];

                m_DistanceMoved += (int)(firstBarrier.Position.X - m_LastPosition.X) * (int)firstBarrier.MovementDirection.XAxis;

                m_LastPosition = firstBarrier.Position;

                if (m_DistanceMoved >= firstBarrier.Width / 2)
                {
                    foreach (Barrier barrier in this)
                    {
                        barrier.MovementDirection.XAxis = barrier.MovementDirection.XAxis == eDirectionX.Left ? eDirectionX.Right : eDirectionX.Left;
                    }

                    m_DistanceMoved = 0;
                }
            }
        }

        public void CreateBarrierLine(int i_Size)
        {
            const bool v_SharedTexture = true;
            for (int i = 0; i < i_Size; i++)
            {
                Barrier barrier = new Barrier(this.Game, @"Sprites\Barrier_44x32", !v_SharedTexture);
                barrier.Disposed += barrier_Disposed;
                this.Add(barrier);
            }
        }

        public void ResetNextLevel(int i_Level)
        {
            this.Clear();
            this.CreateBarrierLine(4);
            this.Initialize();

            if (i_Level == 0 || i_Level == 1)
            {
                this.Velocity = new Vector2(i_Level * 70f);
            }
            else
            {
                this.Velocity *= 1.4f;
            }
        }

        public void InitiateBarrierLine(Vector2 i_LineCenter, float i_GapX)
        {
            float lineLength = (Count * m_TextureBarrier.Width) + ((Count - 1) * m_TextureBarrier.Width * i_GapX);

            for (int i = 0; i < Count; i++)
            {
                Vector2 position = new Vector2();
                position.X = i_LineCenter.X - (lineLength / 2) + ((m_TextureBarrier.Width + (m_TextureBarrier.Width * i_GapX)) * i);
                position.Y = i_LineCenter.Y - (m_TextureBarrier.Height / 2);
                r_Components[i].Position = position;
                if (i == 0)
                {
                    m_LastPosition = position;
                }
            }
        }

        public void InitiateBarrierLine()
        {
            InitiateBarrierLine(new Vector2(this.GraphicsDevice.Viewport.Width / 2, this.GraphicsDevice.Viewport.Height - (3f * m_TextureBarrier.Height)), 1.4f);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_TextureBarrier = this.ContentManager.Load<Texture2D>(@"Sprites\Barrier_44x32");
        }

        protected void OnGroupVelocityChange()
        {
            foreach (Barrier barrier in r_Components)
            {
                barrier.Velocity = m_Velocity;
            }
        }

        private void barrier_Disposed(object sender, EventArgs e)
        {
            Barrier barrier = sender as Barrier;
            barrier.Disposed -= barrier_Disposed;
            this.Remove(barrier);
        }
    }
}

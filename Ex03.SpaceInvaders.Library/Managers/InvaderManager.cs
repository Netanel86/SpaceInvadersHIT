namespace Ex03.SpaceInvaders.Library.GameServices
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
    using Ex03.Infrastracture;
    using Ex03.Infrastracture.Direction;
    using Ex03.Infrastracture.ObjectModel;
    using Ex03.SpaceInvaders.Library.Sprites.Entities;
    using Ex03.SpaceInvaders.Library.ServiceInterfaces;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public class InvaderManager : CompositeDrawableComponent<Invader>, IInvaderManager
    {
        public event EventHandler<TimeSpanArgs> GroupSpeedChanged;

        public event Action OutOfScreenBoundries;

        public event Action GroupCountZero;

        private readonly List<Sprite> r_DisposedInvaders;

        public float TimeGap
        {
            get { return m_TimeGap; }
            set
            {
                if (value != m_TimeGap)
                {
                    m_TimeGap = value;
                    OnGroupSpeedChanged(new TimeSpanArgs(TimeSpan.FromSeconds(m_TimeGap)));
                }
            }
        }

        private MatrixSize m_MatrixSize;
        private Direction2D m_GroupDirection;

        private int m_BaseColumns;
        private int m_BaseRows;
        private int m_CurrentLeftMostCol;
        private int m_CurrentRightMostCol;
        private int m_CurrentBottomMostRow;

        private TimeSpan m_TimeAccumulator;

        private float m_TimeGap;
        private bool m_ChangedDirection;

        public InvaderManager(Game i_Game, Vector2 i_BaseMatrixSize)
            : base(i_Game)
        {
            this.Game.Services.AddService(typeof(IInvaderManager), this);
            m_MatrixSize = new MatrixSize();
            m_GroupDirection = new Direction2D();
            r_DisposedInvaders = new List<Sprite>();

            this.m_MatrixSize.Width = this.m_BaseColumns = (int)i_BaseMatrixSize.X;
            this.m_MatrixSize.Height = this.m_BaseRows = (int)i_BaseMatrixSize.Y;

            this.CreateInvadersMatrix(
                new InvaderManager.eInvaderType[] 
                { 
                    InvaderManager.eInvaderType.InvaderPink, 
                    InvaderManager.eInvaderType.InvaderCyan, 
                    InvaderManager.eInvaderType.InvaderYellow 
                },
                new int[] { m_BaseColumns, m_BaseColumns * 2, m_BaseColumns * 2 });
        }

        public override void Add(Invader i_Component)
        {
            base.Add(i_Component);
            i_Component.Disposed += invader_Disposed;
        }
        
        public override void Initialize()
        {
            base.Initialize();
            m_TimeAccumulator = TimeSpan.Zero;
            m_TimeGap = 0.5f;
            m_ChangedDirection = false;
            this.InitiateInvadersMatrix(new Vector2(16, 96), new Vector2(0.6f));
        }
        
        public override void Update(GameTime i_GameTime)
        {
            m_TimeAccumulator += i_GameTime.ElapsedGameTime;

            if (r_DisposedInvaders.Count == this.Count)
            {
                this.Clear();
                r_DisposedInvaders.Clear();
                OnGroupCountZero();
            }

            if (m_TimeAccumulator.TotalSeconds >= m_TimeGap && this.Count > 0)
            {
                m_TimeAccumulator = TimeSpan.Zero;

                if (m_ChangedDirection)
                {
                    moveGroupVertically();
                    this.TimeGap -= this.TimeGap * 0.09f;
                    m_ChangedDirection = false;

                    if (checkGroupVerticalDeviation())
                    {
                        OnOutOfScreenBoundries();
                    }
                }
                else
                {
                    moveGroupHorizontally();

                    float deviation = checkGroupHorizontalDeviation();

                    if (deviation != 0)
                    {
                        m_ChangedDirection = true;
                        clampGroup(deviation);
                    }
                }
            }

            base.Update(i_GameTime);
        }

        #region Creation Methods

        /// <summary>
        /// Creates an <typeparamref name="Invader"/>.
        /// </summary>
        /// <param name="i_TypeToCreate">Indicates the type of invader to create</param>
        /// <param name="i_Game">refrence to your <typeparamref name="Game"/></param>
        /// <returns></returns>
        private Invader createInvader(eInvaderType i_TypeToCreate, Game i_Game)
        {
            Invader component = null;
            switch (i_TypeToCreate)
            {
                case eInvaderType.InvaderPink:
                    component = new Invader(i_Game, @"Sprites\Enemy6_192x32", Color.LightPink, 180, 0);
                    break;

                case eInvaderType.InvaderCyan:
                    component = new Invader(i_Game, @"Sprites\Enemy6_192x32", Color.LightSkyBlue, 150, 2);
                    break;

                case eInvaderType.InvaderYellow:
                    component = new Invader(i_Game, @"Sprites\Enemy6_192x32", Color.LightYellow, 130, 4);
                    break;
            }

            return component;
        }

        /// <summary>
        /// Creates a matrix of invader's
        /// </summary>
        /// <param name="i_InvaderTypes">a collection of <typeparamref name="eInvaderType"/></param>
        /// <param name="i_SumOfEachType">a collection of integers representing how many objects to create of each type in <paramref name="i_InvaderTypes"/></param>
        public void CreateInvadersMatrix(eInvaderType[] i_InvaderTypes, int[] i_SumOfEachType)
        {
            int typeCounter = 0;
            bool isChangeCellIdx = false;

            if (i_InvaderTypes.Length != i_SumOfEachType.Length)
            {
                throw new ArgumentException("List's must be the same size", "i_InvaderTypes.Count");
            }

            for (int i = 0; i < i_InvaderTypes.Length; i++)
            {
                for (int j = 0; j < i_SumOfEachType[i]; j++)
                {
                    typeCounter++;
                    Invader invader = createInvader(i_InvaderTypes[i], this.Game);
                    if (isChangeCellIdx)
                    {
                        invader.StartCellIdx = invader.SourceRectangleIdx + 1;
                    }

                    this.Add(invader);
                    isChangeCellIdx = typeCounter % m_MatrixSize.Width == 0 ? !isChangeCellIdx : isChangeCellIdx;
                }

                typeCounter = 0;
                isChangeCellIdx = false;
            }
        }

        /// <summary>
        /// Initiate the created invader's positions as a matrix
        /// </summary>
        /// <param name="i_StartPosition">Start position of matrix on screen</param>
        /// <param name="i_Gap">Gap between each two invader's (relative to invader texture width/height)</param>
        /// <param name="i_MatrixSize">Size of matrix, where X = rows & Y = columns</param>
        /// <remarks>
        /// Should be called after 'CreateInvadersMatrix' method is called
        /// </remarks>
        public void InitiateInvadersMatrix(Vector2 i_StartPosition, Vector2 i_Gap)
        {
            int index = 0;

            m_GroupDirection.XAxis = eDirectionX.Right;

            m_CurrentLeftMostCol = 0;
            m_CurrentRightMostCol = m_MatrixSize.Width - 1;

            m_CurrentBottomMostRow = m_MatrixSize.Height - 1;
            if (this.Count != 0)
            {
                for (int row = 0; row < m_MatrixSize.Height; row++)
                {
                    for (int col = 0; col < m_MatrixSize.Width; col++)
                    {
                        Sprite invader = r_Components[index];

                        Vector2 pos = new Vector2();
                        pos.X = i_StartPosition.X + ((invader.Width + (invader.Width * i_Gap.X)) * col);
                        pos.Y = i_StartPosition.Y + ((invader.Height + (invader.Height * i_Gap.Y)) * row);
                        invader.Position = pos;
                        index++;
                    }
                }
            }
        }

        public void ResetNewGame()
        {
            foreach (Invader invader in r_Components)
            {
                invader.Dispose();
            }

            r_DisposedInvaders.Clear();

            ResetNextLevel(0);
        }

        public void ResetNextLevel(int i_Level)
        {
            this.Clear();
            m_MatrixSize.Width = i_Level == 0 ? m_BaseColumns : ++m_MatrixSize.Width;

            Invader.BulletMax = i_Level == 0 ? 1 : 2 + Invader.BulletMax;

            this.CreateInvadersMatrix(
                new InvaderManager.eInvaderType[] 
                { 
                    InvaderManager.eInvaderType.InvaderPink, 
                    InvaderManager.eInvaderType.InvaderCyan, 
                    InvaderManager.eInvaderType.InvaderYellow 
                },
                new int[] { m_MatrixSize.Width, m_MatrixSize.Width * 2, m_MatrixSize.Width * 2 });

            this.IncreaseScores(i_Level * 50);

            this.Initialize();
        }
        #endregion

        #region Movement Handle Methods

        private void moveGroupHorizontally()
        {
            foreach (Sprite invader in this)
            {
                if (!r_DisposedInvaders.Contains(invader))
                {
                    Vector2 position = new Vector2();
                    position.X = invader.Position.X + (invader.Velocity.X * (float)invader.MovementDirection.XAxis);
                    position.Y = invader.Position.Y;
                    invader.Position = position;
                }
            }
        }

        private void moveGroupVertically()
        {
            foreach (Sprite invader in this)
            {
                if (!r_DisposedInvaders.Contains(invader))
                {
                    Vector2 position = new Vector2();
                    position.X = invader.Position.X;
                    position.Y = invader.Position.Y + (invader.Velocity.Y * (float)invader.MovementDirection.YAxis);
                    invader.Position = position;
                }
            }
        }

        private void clampGroup(float i_Deviation)
        {
            foreach (Sprite invader in this)
            {
                if (!r_DisposedInvaders.Contains(invader))
                {
                    invader.MovementDirection.XAxis = m_GroupDirection.XAxis = invader.MovementDirection.XAxis == eDirectionX.Right ? eDirectionX.Left : eDirectionX.Right;

                    Vector2 position = new Vector2();
                    position.X = invader.Position.X + (i_Deviation * (float)invader.MovementDirection.XAxis);
                    position.Y = invader.Position.Y;
                    invader.Position = position;
               }
            }
        }

        #endregion

        #region Horizontal Deviation Handle Methods

        private float checkGroupHorizontalDeviation()
        {
            Rectangle deviatedObjectBounds = Rectangle.Empty;

            const bool v_Deviated = true;

            bool isDeviatedLeft = false;
            bool isDeviatedRight = false;

            float deviation = 0;

            ////check deviation of last/first column in current movement direction only.
            if (m_GroupDirection.XAxis == eDirectionX.Left)
            {
                deviatedObjectBounds = getDeviatedObjectBoundsInColumn(ref m_CurrentLeftMostCol, eMatrixColumnEdge.LeftMost, checkHorizontalLeftDeviation);

                if (isDeviatedLeft = deviatedObjectBounds != Rectangle.Empty ? v_Deviated : !v_Deviated)
                {
                    deviation = (deviatedObjectBounds.Width / 2) - deviatedObjectBounds.X;
                }
            }
            else if (m_GroupDirection.XAxis == eDirectionX.Right)
            {
                deviatedObjectBounds = getDeviatedObjectBoundsInColumn(ref m_CurrentRightMostCol, eMatrixColumnEdge.RightMost, checkHorizontalRightDeviation);

                if (isDeviatedRight = deviatedObjectBounds != Rectangle.Empty ? v_Deviated : !v_Deviated)
                {
                    deviation = deviatedObjectBounds.X + ((3 * deviatedObjectBounds.Width) / 2) - this.Game.GraphicsDevice.Viewport.Bounds.Width;
                }
            }

            return deviation;
        }

        private bool checkHorizontalLeftDeviation(Rectangle i_Bounds)
        {
            return i_Bounds.X < i_Bounds.Width / 2;
        }

        private bool checkHorizontalRightDeviation(Rectangle i_Bounds)
        {
            return i_Bounds.X + ((3 * i_Bounds.Width) / 2) > this.Game.GraphicsDevice.Viewport.Bounds.Width;
        }

        private Rectangle getDeviatedObjectBoundsInColumn(ref int io_ColumnNumber, eMatrixColumnEdge i_EdgeColumnSide, Func<Rectangle, bool> i_DeviationCheckFunc)
        {
            Rectangle deviatedObjectBounds = Rectangle.Empty;
            Sprite invader = null;
            int sumOfInvadersOnColumn = m_MatrixSize.Height;

            for (int idxRow = 0; idxRow < m_MatrixSize.Height; idxRow++)
            {
                if (io_ColumnNumber < m_MatrixSize.Width && io_ColumnNumber >= 0)
                {
                    invader = getInvaderFromMatrix(idxRow, io_ColumnNumber);

                    ////validate that current invader hasn't been disposed.
                    if (!r_DisposedInvaders.Contains(invader))
                    {
                        deviatedObjectBounds = i_DeviationCheckFunc(invader.Bounds) ? invader.Bounds : Rectangle.Empty;
                    }
                    else
                    {
                        if (--sumOfInvadersOnColumn == 0)
                        {
                            ////current column is empty, try next inner column.
                            io_ColumnNumber += (int)i_EdgeColumnSide;
                            idxRow = 0;
                            continue;
                        }
                    }
                }
                else
                {
                    ////no more invader's left, stop.
                    break;
                }
            }

            return deviatedObjectBounds;
        }

        #endregion

        #region Vertical Deviation Handle Methods

        private bool checkGroupVerticalDeviation()
        {
            bool isDeviatedDown = false;
            int sumOfInvadersOnRow = m_MatrixSize.Width;
            Sprite invader = null;

            for (int idxCol = 0; idxCol < m_MatrixSize.Width; idxCol++)
            {
                if (m_CurrentBottomMostRow >= 0)
                {
                    invader = getInvaderFromMatrix(m_CurrentBottomMostRow, idxCol);

                    if (!r_DisposedInvaders.Contains(invader))
                    {
                        if (invader.Bounds.Y + invader.Height > this.Game.GraphicsDevice.Viewport.Height - (1.5f * invader.Height))
                        {
                            isDeviatedDown = true;
                            break;
                        }
                    }
                    else
                    {
                        if (--sumOfInvadersOnRow == 0)
                        {
                            m_CurrentBottomMostRow--;
                            idxCol = 0;
                            continue;
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            return isDeviatedDown;
        }

        #endregion

        public void IncreaseScores(int i_Amount)
        {
            if (i_Amount != 0)
            {
                foreach (Enemy invader in r_Components)
                {
                    invader.Score += i_Amount;
                }
            }
        }

        protected virtual void OnGroupSpeedChanged(TimeSpanArgs i_TimeGap)
        {
            if (GroupSpeedChanged != null)
            {
                GroupSpeedChanged(this, i_TimeGap);
            }
        }

        protected virtual void OnOutOfScreenBoundries()
        {
            if (this.OutOfScreenBoundries != null)
            {
                this.OutOfScreenBoundries.Invoke();
            }
        }

        protected virtual void OnGroupCountZero()
        {
            if (GroupCountZero != null)
            {
                GroupCountZero.Invoke();
            }
        }

        private Sprite getInvaderFromMatrix(int i_Row, int i_Col)
        {
            return r_Components[(m_MatrixSize.Width * i_Row) + i_Col];
        }

        private void invader_Disposed(object i_Sender, EventArgs i_Args)
        {
            Invader invader = i_Sender as Invader;
            invader.Disposed -= this.invader_Disposed;
            this.TimeGap -= this.TimeGap * 0.04f;
            r_DisposedInvaders.Add(invader);
        }
        
        public enum eInvaderType
        {
            InvaderPink,
            InvaderCyan,
            InvaderYellow,
        }

        private enum eMatrixColumnEdge
        {
            LeftMost = 1,
            RightMost = -1,
        }

        private class MatrixSize
        {
            private Vector2 m_MatrixSize;

            public int Width
            {
                get { return (int)m_MatrixSize.X; }
                set { m_MatrixSize.X = value; }
            }

            public int Height
            {
                get { return (int)m_MatrixSize.Y; }
                set { m_MatrixSize.Y = value; }
            }

            public MatrixSize()
            {
                m_MatrixSize = new Vector2();
            }
        }
    }
}

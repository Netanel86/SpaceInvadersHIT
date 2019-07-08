namespace Ex03.Infrastracture.Direction
{
    public class Direction2D
    {
        /// <summary>
        /// returns a Direction2D with all of its components set to NoMovement.
        /// </summary>
        public static Direction2D NoMovement
        {
            get
            {
                Direction2D zero = new Direction2D();
                zero.XAxis = eDirectionX.NoMovement;
                zero.YAxis = eDirectionY.NoMovement;
                return zero;
            }
        }
        
        public static Direction2D Left
        {
            get
            {
                Direction2D left = new Direction2D();
                left.XAxis = eDirectionX.Left;
                left.YAxis = eDirectionY.NoMovement;
                return left;
            }
        }
        
        public static Direction2D Right
        {
            get
            {
                Direction2D right = new Direction2D();
                right.XAxis = eDirectionX.Right;
                right.YAxis = eDirectionY.NoMovement;
                return right;
            }
        }
        
        public static Direction2D Up
        {
            get
            {
                Direction2D up = new Direction2D();
                up.XAxis = eDirectionX.NoMovement;
                up.YAxis = eDirectionY.Up;
                return up;
            }
        }
        
        public static Direction2D Down
        {
            get
            {
                Direction2D down = new Direction2D();
                down.XAxis = eDirectionX.NoMovement;
                down.YAxis = eDirectionY.Down;
                return down;
            }
        }
        
        public static Direction2D RightDown
        {
            get
            {
                Direction2D right_down = new Direction2D();
                right_down.XAxis = eDirectionX.Right;
                right_down.YAxis = eDirectionY.Down;
                return right_down;
            }
        }
        
        public Direction2D()
        {
        }

        public Direction2D(eDirectionX i_XAxis, eDirectionY i_YAxis)
        {
            this.XAxis = i_XAxis;
            this.YAxis = i_YAxis;
        }

        public eDirectionX XAxis { get; set; }

        public eDirectionY YAxis { get; set; }
    }
}

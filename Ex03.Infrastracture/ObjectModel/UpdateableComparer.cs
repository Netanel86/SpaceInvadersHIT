namespace Ex03.Infrastracture.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public class UpdateableComparer : IComparer<IUpdateable>
    {
        public static readonly UpdateableComparer Default;

        static UpdateableComparer()
        {
            Default = new UpdateableComparer();
        }

        private UpdateableComparer()
        {
        }

        public int Compare(IUpdateable x, IUpdateable y)
        {
            const int k_XBigger = 1;
            const int k_Equals = 0;
            const int k_YBigger = -1;

            int compareResult = k_YBigger;

            if (x == null && y == null)
            {
                compareResult = k_Equals;
            }
            else if (x != null)
            {
                if (y == null)
                {
                    compareResult = k_XBigger;
                }
                else if (x.Equals(y))
                {
                    compareResult = k_Equals;
                }
                else if (x.UpdateOrder > y.UpdateOrder)
                {
                    compareResult = k_XBigger;
                }
            }

            return compareResult;
        }
    }
}

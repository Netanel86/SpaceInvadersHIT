// -----------------------------------------------------------------------
// <copyright file="DrawableComparer.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Ex03.Infrastracture.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public class DrawableComparer<TDrawable> : IComparer<TDrawable>
        where TDrawable : class, IDrawable
    {
        public static readonly DrawableComparer<TDrawable> Default;

        static DrawableComparer()
        {
            Default = new DrawableComparer<TDrawable>();
        }

        private DrawableComparer()
        {
        }

        public int Compare(TDrawable x, TDrawable y)
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
                else if (x.DrawOrder > y.DrawOrder)
                {
                    compareResult = k_XBigger;
                }
            }

            return compareResult;
        }
    }
}

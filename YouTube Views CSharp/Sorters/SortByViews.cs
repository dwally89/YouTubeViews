namespace YouTube_Views_CSharp
{
    using System;
    using System.Collections;

    /// <summary>
    /// To sort by views
    /// </summary>
    public class SortByViews : IComparer
    {
        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero <paramref name="x" /> is less than <paramref name="y" />. Zero <paramref name="x" /> equals <paramref name="y" />. Greater than zero <paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        int IComparer.Compare(object x, object y)
        {
            Video sx = (Video)x;
            Video sy = (Video)y;
            if (sx.Views > sy.Views)
            {
                return -1;
            }
            else if (sx.Views < sy.Views)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}

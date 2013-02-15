namespace YouTube_Views_CSharp
{
    using System;
    using System.Collections;

    /// <summary>
    /// Used to sort by date added
    /// </summary>
    public class SortByDateAdded : IComparer
    {
        /// <summary>
        /// If sorting ascending or not
        /// </summary>
        private bool ascending;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortByDateAdded" /> class.
        /// </summary>
        /// <param name="ascending">if set to <c>true</c> [ascending].</param>
        public SortByDateAdded(bool ascending)
        {
            this.ascending = ascending;
        }

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
            if (sx.DateAdded > sy.DateAdded)
            {
                if (ascending)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else if (sx.DateAdded < sy.DateAdded)
            {
                if (ascending)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}

namespace YouTube_Views_CSharp
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Used to sort by total views
    /// </summary>
    public class SortByTotalViews : IComparer<Video>
    {
        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="video1">The video1.</param>
        /// <param name="video2">The video2.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero <paramref name="x" /> is less than <paramref name="y" />. Zero <paramref name="x" /> equals <paramref name="y" />. Greater than zero <paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        public int Compare(Video video1, Video video2)
        {            
            if (video1.TotalViews > video2.TotalViews)
            {
                return -1;
            }
            else if (video1.TotalViews < video2.TotalViews)
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

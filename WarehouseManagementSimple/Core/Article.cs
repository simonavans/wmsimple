using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Article
    {
        /// <summary>
        /// Part Identifier, not unique.
        /// Primary Key.
        /// </summary>
        public string Partno { get; set; } = string.Empty;

        /// <summary>
        /// Part revision.
        /// Primary Key.
        /// Example: A 0.5L bottle and a 1.5L bottle of Cola can be considered two revisions of the same article.
        /// </summary>
        public string Revision { get; set; } = string.Empty;

        /// <summary>
        /// Unique ID for a specific article.
        /// Unique Key.
        /// </summary>
        public int ArticleID { get; set; }

        /// <summary>
        /// Dimension length in Meters.
        /// </summary>
        public decimal DimensionsLength { get; set; }

        /// <summary>
        /// Dimension width in Meters.
        /// </summary>
        public decimal DimensionsWidth { get; set; }

        /// <summary>
        /// Dimension height in Meters.
        /// </summary>
        public decimal DimensionsHeight { get; set; }

        public override bool Equals(object? obj)
        {
          return obj is Article article &&
                 ArticleID == article.ArticleID;
        }

        public override int GetHashCode()
        {
          return HashCode.Combine(ArticleID);
        }

        public override string ToString()
        {
            return $"Partno {Partno}, Revision {Revision}, ArticleID: {ArticleID}";
        }
    }
}

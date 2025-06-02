using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Representation of the contents of a Storage Unit.
    /// </summary>
    public class StUnitContent
    {
        /// <summary>
        /// ID of the StUnit this record belong to.
        /// Foreign Key.
        /// Primary Key.
        /// </summary>
        public int FK_StUnitID { get; set; }

        /// <summary>
        /// ID of the Article which is in the StUnit.
        /// Foreign Key.
        /// Primary Key.
        /// </summary>
        public int FK_ArticleID { get; set; }

        /// <summary>
        /// ID of the Order which the content is assigned to.
        /// An ID of 0 or less indicates that the content isn't assigned to any Order.
        /// Foreign Key.
        /// Primary Key.
        /// </summary>
        public int FK_OrderID { get; set; } = 0;

        /// <summary>
        /// Quantity of the Article inside the StUnit.
        /// </summary>
        public int Quantity { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Representation of the articles required for an order.
    /// </summary>
    public class OrderContent
    {
        /// <summary>
        /// ID of the Order this record belong to.
        /// Foreign Key.
        /// Primary Key.
        /// </summary>
        public int FK_OrderID { get; set; }

        /// <summary>
        /// ID of the Article needed for the Order.
        /// Foreign Key.
        /// Primary Key.
        /// </summary>
        public int FK_ArticleID { get; set; }

        /// <summary>
        /// Quantity of the Article for the Order.
        /// </summary>
        public int Quantity { get; set; }
    }
}

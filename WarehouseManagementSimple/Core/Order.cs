using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Order
    {
        /// <summary>
        /// Unique ID for an Order.
        /// Primary Key.
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// Name of the Customer attached to the Order.
        /// </summary>
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>
        /// Address of the Customer attached to the Order.
        /// </summary>
        public string CustomerAddress { get; set; } = string.Empty;
    }
}

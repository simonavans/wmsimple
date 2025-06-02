using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Location
    {
        /// <summary>
        /// Material Handling Area, the top level identifier of a location.
        /// Primary Key.
        /// </summary>
        public string Mha { get; set; } = string.Empty;

        /// <summary>
        /// Rack identifier.
        /// Primary Key.
        /// </summary>
        public string Rack { get; set; } = string.Empty;

        /// <summary>
        /// Horizontal Coordinate.
        /// Primary Key.
        /// </summary>
        public string HorCoor { get; set; } = string.Empty;

        /// <summary>
        /// Vertical Coordinate.
        /// Primary Key.
        /// </summary>
        public string VerCoor { get; set; } = string.Empty;

        /// <summary>
        /// Unique ID of the location.
        /// Unique Key.
        /// </summary>
        public int LocationID { get; set; }

        public override string ToString()
        {
            return $"Mha {Mha}, Rack {Rack}, HorCoor {HorCoor}, VerCoor {VerCoor}, LocationID {LocationID}";
        }
    }
}

namespace Core
{
    public class StUnit
    {
        /// <summary>
        /// The Unique ID of the Storage Unit.
        /// Primary Key.
        /// </summary>
        public int StUnitID { get; set; }

        /// <summary>
        /// Type of storage unit.
        /// Example: A small box for small items, a bigger box, a pallet etc.
        /// </summary>
        public string StUnitType { get; set; } = string.Empty;

        /// <summary>
        /// Current location of the StUnit.
        /// FK to <see cref="Location.LocationID"/>.
        /// </summary>
        public int FK_LocationID { get; set; } = 0;
    }
}

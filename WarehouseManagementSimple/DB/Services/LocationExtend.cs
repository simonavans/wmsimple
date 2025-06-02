using Core;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Services
{
    public class LocationExtend
    {
        private readonly int locationId;
        private readonly SqliteConnection connection;

        public LocationExtend(int locationId, SqliteConnection connection)
        {
            this.locationId = locationId;
            this.connection = connection;
        }

        /// <summary>
        /// A Location is available when there is no StUnit placed upon it.
        /// </summary>
        /// <returns></returns>
        public bool IsAvailable()
        {
            StUnit stUnit = new StUnit();
            stUnit.FK_LocationID = locationId;

            DBStUnit dBStUnit =  new DBStUnit(connection);
            return dBStUnit.GetByLocation(ref stUnit) == false;
        }

        /// <summary>
        /// Checks if a location with the provided id exists.
        /// </summary>
        /// <param name="id">LocationID</param>
        /// <returns></returns>
        public bool IsExisting()
        {
            Location location = new Location();
            location.LocationID = locationId;

            DBLocation dbLocation = new DBLocation(connection);
            return dbLocation.GetByLocationID(ref location);
        }
    }
}

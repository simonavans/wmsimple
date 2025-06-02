using Core;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Services
{
    public class StUnitExtend
    {
        private int stUnitID;
        private readonly SqliteConnection connection;

        /// <summary>
        /// Creates a new stunit
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="stunitType"></param>
        /// <param name="locationID"></param>
        public StUnitExtend(SqliteConnection connection, string stunitType, int locationID)
        {
            this.connection = connection;

            CreateStUnit(stunitType, locationID);
        }

        /// <summary>
        /// Gets an existing stunit
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="stUnitID"></param>
        public StUnitExtend(SqliteConnection connection, int stUnitID)
        {
            this.connection = connection;
            this.stUnitID = stUnitID;
        }

        /// <summary>
        /// Create a new StUnit at the given location
        /// </summary>
        /// <param name="stunitType">The type of stunit (pallet, crate).</param>
        /// <param name="locationID">The location where the stunit should be created.</param>
        /// <exception cref="Exception"></exception>
        private void CreateStUnit(string stunitType, int locationID)
        {
            StUnit stUnit = new StUnit();
            stUnit.StUnitType = stunitType;
            stUnit.FK_LocationID = locationID;

            DBStUnit dBStUnit = new DBStUnit(this.connection);
            if (!dBStUnit.Create(ref stUnit))
            {
                throw new Exception("Can't create StUnit!");
            }

            this.stUnitID = stUnit.StUnitID;
        }

        /// <summary>
        /// Add a StUnitContent record to this StUnit
        /// </summary>
        /// <param name="articleID">The article to add.</param>
        /// <param name="quantity">The amount of the given article to add.</param>
        /// <returns></returns>
        public bool AddContent(int articleID, int quantity)
        {
            StUnit stUnit = new StUnit();
            stUnit.StUnitID = this.stUnitID;

            DBStUnit dBStUnit = new DBStUnit(this.connection);
            if (!dBStUnit.Get(ref stUnit))
            {
                return false;
            }

            StUnitContent content = new StUnitContent();
            content.FK_StUnitID = stUnit.StUnitID;
            content.FK_ArticleID = articleID;
            content.Quantity = quantity;

            DBStUnitContent dBStUnitContent = new DBStUnitContent(this.connection);
            if (!dBStUnitContent.Create(ref content))
            {
                return false;
            }

            return true;
        }
    }
}

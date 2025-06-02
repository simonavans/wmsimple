using Core;
using DB;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.DB
{
    public class DBLocationTests : IDisposable
    {
        SqliteConnection sqliteConnection;
        DBLocation dbLocation;

        public DBLocationTests()
        {
            sqliteConnection = new SqliteConnection("Data Source = DBLocationTests.db");
            sqliteConnection.Open();

            dbLocation = new(sqliteConnection);
            dbLocation.InitTable(true);
        }

        public void Dispose()
        {
            sqliteConnection.Close();
        }

        [Fact]
        public void Create()
        {
            //Arrange
            bool result;
            Location location1 = new() {Mha = "IN01", Rack = "01", HorCoor = "00", VerCoor = "01" };
            Location location2 = new() {Mha = "IN01", Rack = "01", HorCoor = "00", VerCoor = "02" };

            //Act
            result = dbLocation.Create(ref location1);
            result = dbLocation.Create(ref location2);

            //Assert
            Assert.True(result);
            Assert.NotEqual(-1, location1.LocationID);
            Assert.NotEqual(-1, location2.LocationID);
            Assert.NotEqual(location1.LocationID, location2.LocationID);
        }
    }
}

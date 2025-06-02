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
    public class DBStUnitTests : IDisposable
    {
        SqliteConnection sqliteConnection;
        DBStUnit dbStUnit;

        public DBStUnitTests()
        {
            sqliteConnection = new SqliteConnection("Data Source = DBStUnitTests.db");
            sqliteConnection.Open();

            dbStUnit = new(sqliteConnection);
            dbStUnit.InitTable(true);
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
            StUnit stUnit1 = new() {StUnitType = "A"};
            StUnit stUnit2 = new() {StUnitType = "B"};

            //Act
            result = dbStUnit.Create(ref stUnit1);
            result = dbStUnit.Create(ref stUnit2);

            //Assert
            Assert.True(result);
            Assert.NotEqual(-1, stUnit1.StUnitID);
            Assert.NotEqual(-1, stUnit2.StUnitID);
            Assert.NotEqual(stUnit1.StUnitID, stUnit2.StUnitID);
        }
    }
}

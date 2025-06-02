using CLI;
using DB;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.CLI
{
    public class DBTestDataFactoryTests : IDisposable
    {
        SqliteConnection sqliteConnection;
        DBTestDataFactory dbTestDataFactory;

        public DBTestDataFactoryTests()
        {
            sqliteConnection = new SqliteConnection("Data Source = DBTestDataFactoryTests.db");
            sqliteConnection.Open();

            dbTestDataFactory = new(sqliteConnection);
            dbTestDataFactory.CreateTables(true);
        }

        public void Dispose()
        {
            sqliteConnection.Close();
        }

        [Fact]
        public void LoadArticleData()
        {
            //Arrange
            DBArticle dbArticle = new(sqliteConnection);
            IList<Core.Article> articles;

            //Act
            dbTestDataFactory.LoadArticleData();
            articles = dbArticle.GetAll();

            //Assert
            Assert.NotEmpty(articles);
        }

        [Fact]
        public void LoadLocationData()
        {
            //Arrange
            DBLocation dbLocation = new(sqliteConnection);
            IList<Core.Location> location;

            //Act
            dbTestDataFactory.LoadLocationData();
            location = dbLocation.GetAll();

            //Assert
            Assert.NotEmpty(location);
        }

        [Fact]
        public void LoadOrderData()
        {
            //Arrange
            DBOrderContent dbOrderContent = new(sqliteConnection);
            DBOrder dbOrder = new(sqliteConnection);
            IList<Core.Order> orders;
            IList<Core.OrderContent> orderContents;

            //Act
            dbTestDataFactory.LoadArticleData();
            dbTestDataFactory.LoadOrderData();
            orders = dbOrder.GetAll();
            orderContents = dbOrderContent.GetAll();

            //Assert
            Assert.NotEmpty(orders);
            Assert.NotEmpty(orderContents);
        }

        [Fact]
        public void LoadStUnitsData()
        {
            //Arrange
            DBStUnit dbStUnit = new(sqliteConnection);
            DBStUnitContent dbStUnitContent = new(sqliteConnection);
            IList<Core.StUnit> stUnits;
            IList<Core.StUnitContent> stUnitContents;

            //Act
            dbTestDataFactory.LoadArticleData();
            dbTestDataFactory.LoadLocationData();
            dbTestDataFactory.LoadStUnitData();
            stUnits = dbStUnit.GetAll();
            stUnitContents = dbStUnitContent.GetAll();

            //Assert
            Assert.NotEmpty(stUnits);
            Assert.NotEmpty(stUnitContents);
        }
    }
}

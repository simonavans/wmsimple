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
    public class DBStUnitContentTests : IDisposable
    {
        SqliteConnection sqliteConnection;
        DBStUnit         dbStUnit;
        DBArticle        dbArticle;
        DBStUnitContent  dbStUnitContent;

        public DBStUnitContentTests()
        {
            sqliteConnection = new SqliteConnection("Data Source = DBStUnitContentTests.db");
            sqliteConnection.Open();

            dbStUnitContent = new(sqliteConnection);
            dbStUnitContent.InitTable(true);

            dbStUnit = new(sqliteConnection);
            dbStUnit.InitTable(true);

            dbArticle = new(sqliteConnection);
            dbArticle.InitTable(true);
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
            Article article1 = new() {ArticleID = -1, Partno = "milk", Revision = "", DimensionsHeight = 0.19m, DimensionsLength = 0.07m, DimensionsWidth = 0.07m };

            StUnitContent content1 = new() {  FK_ArticleID = 1, FK_StUnitID = 1, Quantity = 10 };
            StUnitContent content2 = new() {  FK_ArticleID = 1, FK_StUnitID = 2, Quantity = 10 };
            StUnitContent content3 = new() {  FK_ArticleID = 2, FK_StUnitID = 1, Quantity = 10 };

            //Act
            dbStUnit.Create(ref stUnit1);
            dbArticle.Create(ref article1);

            result = dbStUnitContent.Create(ref content1);
            Action actionFail1 = () => { dbStUnitContent.Create(ref content2); };
            Action actionFail2 = () => { dbStUnitContent.Create(ref content3); };

            //Assert
            Assert.True(result);
            Assert.ThrowsAny<SqliteException>(actionFail1);
            Assert.ThrowsAny<SqliteException>(actionFail2);
        }
    }
}

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
    public class DBOrderContentTests : IDisposable
    {
        SqliteConnection sqliteConnection;
        DBOrder          dbOrder;
        DBArticle        dbArticle;
        DBOrderContent   dbOrderContent;

        public DBOrderContentTests()
        {
            sqliteConnection = new SqliteConnection("Data Source = DBOrderContentTests.db");
            sqliteConnection.Open();

            dbOrderContent = new(sqliteConnection);
            dbOrderContent.InitTable(true);

            dbOrder = new(sqliteConnection);
            dbOrder.InitTable(true);

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
            Order order1 = new() {OrderID = -1, CustomerName = "Hello", CustomerAddress = "World" };
            Article article1 = new() {ArticleID = -1, Partno = "milk", Revision = "", DimensionsHeight = 0.19m, DimensionsLength = 0.07m, DimensionsWidth = 0.07m };

            OrderContent content1 = new() {  FK_ArticleID = 1, FK_OrderID = 1, Quantity = 10 };
            OrderContent content2 = new() {  FK_ArticleID = 1, FK_OrderID = 2, Quantity = 10 };
            OrderContent content3 = new() {  FK_ArticleID = 2, FK_OrderID = 1, Quantity = 10 };

            //Act
            dbOrder.Create(ref order1);
            dbArticle.Create(ref article1);

            result = dbOrderContent.Create(ref content1);
            Action actionFail1 = () => { dbOrderContent.Create(ref content2); };
            Action actionFail2 = () => { dbOrderContent.Create(ref content3); };

            //Assert
            Assert.True(result);
            Assert.ThrowsAny<SqliteException>(actionFail1);
            Assert.ThrowsAny<SqliteException>(actionFail2);
        }
    }
}

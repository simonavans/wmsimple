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
    public class DBArticleTests : IDisposable
    {
        SqliteConnection sqliteConnection;
        DBArticle dbArticle;

        public DBArticleTests()
        {
            sqliteConnection = new SqliteConnection("Data Source = DBArticleTests.db");
            sqliteConnection.Open();

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
            bool result1;
            bool result2;
            Article article1 = new() {ArticleID = -1, Partno = "milk", Revision = "", DimensionsHeight = 0.19m, DimensionsLength = 0.07m, DimensionsWidth = 0.07m };
            Article article2 = new() {ArticleID = -1, Partno = "milk", Revision = "half-full", DimensionsHeight = 0.19m, DimensionsLength = 0.07m, DimensionsWidth = 0.07m };

            //Act
            result1 = dbArticle.Create(ref article1);
            result2 = dbArticle.Create(ref article2);

            //Assert
            Assert.True(result1);
            Assert.True(result2);
            Assert.NotEqual(-1, article1.ArticleID);
            Assert.NotEqual(-1, article2.ArticleID);
            Assert.NotEqual(article1.ArticleID, article2.ArticleID);
        }
    }
}

using Core;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class DBOrderContent : IDBCrud<OrderContent>
    {
        public const string tableName = "tblOrderContent";

        private readonly SqliteConnection _connection;

        public DBOrderContent(SqliteConnection connection)
        {
            _connection = connection;
        }

        public void InitTable(bool dropTable = false)
        {
            SqliteCommand command = _connection.CreateCommand();

            if (dropTable)
                command.CommandText = $"DROP TABLE IF EXISTS {tableName};";

            command.CommandText += $"CREATE TABLE IF NOT EXISTS {tableName} (" +
                "FK_OrderID INTEGER," +
                "FK_ArticleID INTEGER," +
                "Quantity INTEGER," +
                "PRIMARY KEY (FK_OrderID, FK_ArticleID)," +
                $"FOREIGN KEY (FK_OrderID) REFERENCES {DBOrder.tableName}(OrderID)," +
                $"FOREIGN KEY (FK_ArticleID) REFERENCES {DBArticle.tableName}(ArticleID)" +
                ");";
            command.ExecuteNonQuery();
        }

        public void Purge()
        {
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"DELETE FROM {tableName}";
            command.ExecuteNonQuery();
        }

        public bool Create(ref OrderContent obj)
        {
            bool result;
            SqliteCommand commandInsert;

            using (var transaction = _connection.BeginTransaction())
            {
                commandInsert = _connection.CreateCommand();
                commandInsert.Transaction = transaction;
                commandInsert.CommandText = $"INSERT INTO {tableName} (" +
                    $"FK_OrderID," +
                    $"FK_ArticleID," +
                    $"Quantity" +
                    $")" +
                    $"VALUES (" +
                    $"{obj.FK_OrderID}," +
                    $"{obj.FK_ArticleID}," +
                    $"{obj.Quantity}" +
                    $");";
                result = commandInsert.ExecuteNonQuery() > 0;
                transaction.Commit();
            }

            return result;
        }

        public bool Delete(OrderContent obj)
        {
            bool result;
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"DELETE FROM {tableName} WHERE FK_OrderID = {obj.FK_OrderID} AND FK_ArticleID = {obj.FK_ArticleID}";
            result = command.ExecuteNonQuery() > 0;

            return result;
        }

        public bool Get(ref OrderContent obj)
        {
            bool result = false;
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"SELECT * FROM {tableName} WHERE FK_OrderID = {obj.FK_OrderID} AND FK_ArticleID = {obj.FK_ArticleID}";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    result = true;
                    obj.Quantity = reader.GetInt32(2);
                }
            }

            return result;
        }

        public IList<OrderContent> GetAll()
        {
            List<OrderContent> result = new();
            SqliteCommand command = _connection.CreateCommand();
            OrderContent element;

            command.CommandText = $"SELECT * FROM {tableName}";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    element = new();
                    element.FK_OrderID = reader.GetInt32(0);
                    element.FK_ArticleID = reader.GetInt32(1);
                    element.Quantity = reader.GetInt32(2);
                    result.Add(element);
                }
            }

            return result;
        }

        public bool Update(OrderContent obj)
        {
            bool result;
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"UPDATE {tableName} SET " +
                $"Quantity = {obj.Quantity}" +
                $"WHERE FK_OrderID = {obj.FK_OrderID} AND FK_ArticleID = {obj.FK_ArticleID}";
            result = command.ExecuteNonQuery() > 0;

            return result;
        }
    }
}

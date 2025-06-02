using Core;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class DBStUnitContent : IDBCrud<StUnitContent>
    {
        public const string tableName = "tblStUnitContent";

        private readonly SqliteConnection _connection;

        public DBStUnitContent(SqliteConnection connection)
        {
            _connection = connection;
        }

        public void InitTable(bool dropTable = false)
        {
            SqliteCommand command = _connection.CreateCommand();

            if (dropTable)
                command.CommandText = $"DROP TABLE IF EXISTS {tableName};";

            command.CommandText += $"CREATE TABLE IF NOT EXISTS {tableName} (" +
                "FK_StUnitID INTEGER," +
                "FK_ArticleID INTEGER," +
                "FK_OrderID INTEGER," +
                "Quantity INTEGER," +
                "PRIMARY KEY (FK_StUnitID, FK_ArticleID, FK_OrderID)," +
                $"FOREIGN KEY (FK_StUnitID) REFERENCES {DBStUnit.tableName}(StUnitID)," +
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

        public bool Create(ref StUnitContent obj)
        {
            bool result;
            SqliteCommand commandInsert;

            using (var transaction = _connection.BeginTransaction())
            {
                commandInsert = _connection.CreateCommand();
                commandInsert.Transaction = transaction;
                commandInsert.CommandText = $"INSERT INTO {tableName} (" +
                    $"FK_StUnitID," +
                    $"FK_ArticleID," +
                    $"FK_OrderID," +
                    $"Quantity" +
                    $")" +
                    $"VALUES (" +
                    $"{obj.FK_StUnitID}," +
                    $"{obj.FK_ArticleID}," +
                    $"{obj.FK_OrderID}," +
                    $"{obj.Quantity}" +
                    $");";
                result = commandInsert.ExecuteNonQuery() > 0;
                transaction.Commit();
            }

            return result;
        }

        public bool Delete(StUnitContent obj)
        {
            bool result;
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"DELETE FROM {tableName} WHERE FK_StUnitID = {obj.FK_StUnitID} AND FK_ArticleID = {obj.FK_ArticleID} AND FK_OrderID = {obj.FK_OrderID}";
            result = command.ExecuteNonQuery() > 0;

            return result;
        }

        public bool Get(ref StUnitContent obj)
        {
            bool result = false;
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"SELECT * FROM {tableName} WHERE FK_StUnitID = {obj.FK_StUnitID} AND FK_ArticleID = {obj.FK_ArticleID} AND FK_OrderID = {obj.FK_OrderID}";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    result = true;
                    obj.Quantity = reader.GetInt32(3);
                }
            }

            return result;
        }

        public IList<StUnitContent> GetAll()
        {
            List<StUnitContent> result = new();
            SqliteCommand command = _connection.CreateCommand();
            StUnitContent element;

            command.CommandText = $"SELECT * FROM {tableName}";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    element = new();
                    element.FK_StUnitID = reader.GetInt32(0);
                    element.FK_ArticleID = reader.GetInt32(1);
                    element.FK_OrderID = reader.GetInt32(2);
                    element.Quantity = reader.GetInt32(3);
                    result.Add(element);
                }
            }

            return result;
        }

        public bool Update(StUnitContent obj)
        {
            bool result;
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"UPDATE {tableName} SET " +
                $"Quantity = {obj.Quantity} " +
                $"WHERE FK_StUnitID = {obj.FK_StUnitID} AND FK_ArticleID = {obj.FK_ArticleID} AND FK_OrderID = {obj.FK_OrderID}";
            result = command.ExecuteNonQuery() > 0;

            return result;
        }
    }
}

using Core;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class DBArticle : IDBCrud<Article>
    {
        public const string tableName = "tblArticle";

        private readonly SqliteConnection _connection;
        public DBArticle(SqliteConnection connection)
        {
            _connection = connection;
        }

        public void InitTable(bool dropTable = false)
        {
            SqliteCommand command = _connection.CreateCommand();

            if (dropTable)
                command.CommandText = $"DROP TABLE IF EXISTS {tableName};";

            command.CommandText += $"CREATE TABLE IF NOT EXISTS {tableName} (" +
                "Partno TEXT," +
                "Revision TEXT," +
                "ArticleID INTEGER UNIQUE," +
                "DimensionsLength TEXT," +
                "DimensionsWidth TEXT," +
                "DimensionsHeight TEXT," +
                "PRIMARY KEY (Partno, Revision)" +
                ");";
            command.ExecuteNonQuery();
        }

        public void Purge()
        {
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"DELETE FROM {tableName}";
            command.ExecuteNonQuery();
        }

        public bool Create(ref Article obj)
        {
            bool result;
            int uniqueKey;
            SqliteCommand commandUniqueKey;
            SqliteDataReader readerUniqueKey;
            SqliteCommand commandInsert;

            using (var transaction = _connection.BeginTransaction())
            {
                commandUniqueKey = _connection.CreateCommand();
                commandUniqueKey.Transaction = transaction;
                commandUniqueKey.CommandText = $"SELECT MAX(ArticleID) FROM {tableName}";
                readerUniqueKey = commandUniqueKey.ExecuteReader();
                if (readerUniqueKey.Read() && readerUniqueKey.IsDBNull(0) == false)
                {
                    uniqueKey = readerUniqueKey.GetInt32(0) + 1;
                }
                else
                {
                    uniqueKey = 1;
                }

                commandInsert = _connection.CreateCommand();
                commandInsert.Transaction = transaction;
                commandInsert.CommandText = $"INSERT INTO {tableName} (" +
                    $"Partno," +
                    $"Revision," +
                    $"ArticleID," +
                    $"DimensionsLength," +
                    $"DimensionsWidth," +
                    $"DimensionsHeight" +
                    $")" +
                    $"VALUES (" +
                    $"'{obj.Partno}'," +
                    $"'{obj.Revision}'," +
                    $"{uniqueKey}," +
                    $"'{Convert.ToString(obj.DimensionsLength)}'," +
                    $"'{Convert.ToString(obj.DimensionsWidth)}'," +
                    $"'{Convert.ToString(obj.DimensionsHeight)}'" +
                    $");";
                result = commandInsert.ExecuteNonQuery() > 0;

                if (result)
                {
                    obj.ArticleID = uniqueKey;
                }

                transaction.Commit();
            }

            return result;
        }

        public bool Delete(Article obj)
        {
            bool result;
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"DELETE FROM {tableName} WHERE Partno = '{obj.Partno}' AND Revision = '{obj.Revision}'";
            result = command.ExecuteNonQuery() > 0;

            return result;
        }

        public bool Get(ref Article obj)
        {
            bool result = false;
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"SELECT * FROM {tableName} WHERE Partno = '{obj.Partno}' AND Revision = '{obj.Revision}'";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    result = true;
                    obj.ArticleID = reader.GetInt32(2);
                    obj.DimensionsLength = reader.GetDecimal(3);
                    obj.DimensionsWidth  = reader.GetDecimal(4);
                    obj.DimensionsHeight = reader.GetDecimal(5);
                }
            }

            return result;
        }

        /// <summary>
        /// Uses ArticleID instead of Partno and Revision to get Article data.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool GetByArticleID(ref Article obj)
        {
            bool result = false;
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"SELECT * FROM {tableName} WHERE ArticleID = {obj.ArticleID}";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    result = true;
                    obj.Partno           = reader.GetString(0);
                    obj.Revision         = reader.GetString(1);
                    obj.DimensionsLength = reader.GetDecimal(3);
                    obj.DimensionsWidth  = reader.GetDecimal(4);
                    obj.DimensionsHeight = reader.GetDecimal(5);
                }
            }

            return result;

        }

        public IList<Article> GetAll()
        {
            List<Article> result = new();
            SqliteCommand command = _connection.CreateCommand();
            Article element;

            command.CommandText = $"SELECT * FROM {tableName}";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    element = new();
                    element.Partno           = reader.GetString(0);
                    element.Revision         = reader.GetString(1);
                    element.ArticleID        = reader.GetInt32(2);
                    element.DimensionsLength = reader.GetDecimal(3);
                    element.DimensionsWidth  = reader.GetDecimal(4);
                    element.DimensionsHeight = reader.GetDecimal(5);
                    result.Add(element);
                }
            }

            return result;
        }

        public bool Update(Article obj)
        {
            bool result;
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"UPDATE {tableName} SET " +
                $"DimensionsLength = '{Convert.ToString(obj.DimensionsLength)}'," +
                $"DimensionsWidth  = '{Convert.ToString(obj.DimensionsWidth)}'," +
                $"DimensionsHeight = '{Convert.ToString(obj.DimensionsHeight)}'" +
                $"WHERE Partno = '{obj.Partno}' AND Revision = '{obj.Revision}'";
            result = command.ExecuteNonQuery() > 0;

            return result;
        }
    }
}

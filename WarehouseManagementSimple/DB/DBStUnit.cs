using Core;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class DBStUnit : IDBCrud<StUnit>
    {
        public const string tableName = "tblStUnit";

        private readonly SqliteConnection _connection;
        public DBStUnit(SqliteConnection connection)
        {
            _connection = connection;
        }

        public void InitTable(bool dropTable = false)
        {
            SqliteCommand command = _connection.CreateCommand();

            if (dropTable)
                command.CommandText = $"DROP TABLE IF EXISTS {tableName};";

            command.CommandText += $"CREATE TABLE IF NOT EXISTS {tableName} (" +
                "StUnitID INTEGER PRIMARY KEY AUTOINCREMENT," +
                "StUnitType TEXT," +
                "FK_LocationID INTEGER" +
                ");";
            command.ExecuteNonQuery();
        }

        public void Purge()
        {
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"DELETE FROM {tableName}";
            command.ExecuteNonQuery();
        }

        public bool Create(ref StUnit obj)
        {
            bool result;
            SqliteCommand commandInsert;
            SqliteCommand commandSelect;

            using (var transaction = _connection.BeginTransaction())
            {
                commandInsert = _connection.CreateCommand();
                commandInsert.Transaction = transaction;
                commandInsert.CommandText = $"INSERT INTO {tableName} (" +
                    $"StUnitType," +
                    $"FK_LocationID" +
                    $")" +
                    $"VALUES (" +
                    $"'{obj.StUnitType}'," +
                    $"{obj.FK_LocationID}" +
                    $");";
                result = commandInsert.ExecuteNonQuery() > 0;

                if (result)
                {
                    commandSelect = _connection.CreateCommand();
                    commandSelect.Transaction = transaction;
                    commandSelect.CommandText = "SELECT last_insert_rowid()";

                    obj.StUnitID = Convert.ToInt32(commandSelect.ExecuteScalar());
                }

                transaction.Commit();
            }

            return result;
        }

        public bool Delete(StUnit obj)
        {
            bool result;
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"DELETE FROM {tableName} WHERE StUnitID = {obj.StUnitID}";
            result = command.ExecuteNonQuery() > 0;

            return result;
        }

        public bool Get(ref StUnit obj)
        {
            bool result = false;
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"SELECT * FROM {tableName} WHERE StUnitID = {obj.StUnitID}";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    result = true;
                    obj.StUnitType    = reader.GetString(1);
                    obj.FK_LocationID = reader.GetInt32(2);
                }
            }

            return result;
        }

        public bool GetByLocation(ref StUnit obj)
        {
            bool result = false;
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"SELECT * FROM {tableName} WHERE FK_LocationID = {obj.FK_LocationID}";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    result = true;
                    obj.StUnitType    = reader.GetString(1);
                    obj.FK_LocationID = reader.GetInt32(2);
                }
            }

            return result;
        }

        public IList<StUnit> GetAll()
        {
            List<StUnit> result    = new();
            SqliteCommand command = _connection.CreateCommand();
            StUnit element;

            command.CommandText = $"SELECT * FROM {tableName}";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    element = new();
                    element.StUnitID      = reader.GetInt32(0);
                    element.StUnitType    = reader.GetString(1);
                    element.FK_LocationID = reader.GetInt32(2);
                    result.Add(element);
                }
            }

            return result;
        }

        public bool Update(StUnit obj)
        {
            bool result;
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"UPDATE {tableName} SET " +
                $"StUnitType = '{obj.StUnitType}'," +
                $"FK_LocationID = {obj.FK_LocationID} " +
                $"WHERE StUnitID = {obj.StUnitID}";
            result = command.ExecuteNonQuery() > 0;

            return result;
        }
    }
}

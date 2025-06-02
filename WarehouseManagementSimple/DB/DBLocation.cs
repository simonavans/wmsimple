using Core;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class DBLocation : IDBCrud<Location>
    {
        public const string tableName = "tblLocation";

        private readonly SqliteConnection _connection;

        public DBLocation(SqliteConnection connection)
        {
            _connection = connection;
        }

        public void InitTable(bool dropTable = false)
        {
            SqliteCommand command = _connection.CreateCommand();

            if (dropTable)
                command.CommandText = $"DROP TABLE IF EXISTS {tableName};";

            command.CommandText += $"CREATE TABLE IF NOT EXISTS {tableName} (" +
                "Mha TEXT," +
                "Rack TEXT," +
                "HorCoor TEXT," +
                "VerCoor TEXT," +
                "LocationID INTEGER UNIQUE," +
                "PRIMARY KEY (Mha, Rack, HorCoor, VerCoor)" +
                ");";
            command.ExecuteNonQuery();
        }

        public void Purge()
        {
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"DELETE FROM {tableName}";
            command.ExecuteNonQuery();
        }

        public bool Create(ref Location obj)
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
                commandUniqueKey.CommandText = $"SELECT MAX(LocationID) FROM {tableName}";
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
                    $"Mha," +
                    $"Rack," +
                    $"HorCoor," +
                    $"VerCoor," +
                    $"LocationID" +
                    $")" +
                    $"VALUES (" +
                    $"'{obj.Mha}'," +
                    $"'{obj.Rack}'," +
                    $"'{obj.HorCoor}'," +
                    $"'{obj.VerCoor}'," +
                    $"{uniqueKey}" +
                    $");";
                result = commandInsert.ExecuteNonQuery() > 0;

                if (result)
                {
                    obj.LocationID = uniqueKey;
                }

                transaction.Commit();
            }

            return result;
        }

        public bool Delete(Location obj)
        {
            bool result;
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"DELETE FROM {tableName} WHERE Mha = '{obj.Mha}' AND Rack = '{obj.Rack}' AND HorCoor = '{obj.HorCoor}' AND VerCoor = '{obj.VerCoor}'";
            result = command.ExecuteNonQuery() > 0;

            return result;
        }

        public bool Get(ref Location obj)
        {
            bool result = false;
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {tableName} WHERE Mha = '{obj.Mha}' AND Rack = '{obj.Rack}' AND HorCoor = '{obj.HorCoor}' AND VerCoor = '{obj.VerCoor}'";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    result = true;
                    obj.LocationID = reader.GetInt32(4);
                }
            }

            return result;
        }

        public IList<Location> GetAll()
        {
            List<Location> result = new();
            SqliteCommand command = _connection.CreateCommand();
            Location element;

            command.CommandText = $"SELECT * FROM {tableName}";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    element = new();
                    element.Mha           = reader.GetString(0);
                    element.Rack         = reader.GetString(1);
                    element.HorCoor          = reader.GetString(2);
                    element.VerCoor          = reader.GetString(3);
                    element.LocationID       = reader.GetInt32(4);
                    result.Add(element);
                }
            }

            return result;
        }

        /// <summary>
        /// Uses LocationId instead of Mha, Rack, Horcoor, vercoor to get Location data.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool GetByLocationID(ref Location obj)
        {
            bool result = false;
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"SELECT * FROM {tableName} WHERE LocationID = {obj.LocationID}";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    result = true;
                    obj.Mha= reader.GetString(0);
                    obj.Rack= reader.GetString(1);
                    obj.HorCoor= reader.GetString(2);
                    obj.VerCoor= reader.GetString(3);
                    obj.LocationID = reader.GetInt32(4);
                }
            }

            return result;
        }

        public bool Update(Location obj)
        {
            throw new NotSupportedException("Location does not support any update functionality.");
        }
    }
}

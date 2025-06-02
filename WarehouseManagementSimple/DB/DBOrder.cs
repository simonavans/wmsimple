using Core;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography.X509Certificates;

namespace DB
{
    public class DBOrder : IDBCrud<Order>
    {
        public const string tableName = "tblOrder";

        private readonly SqliteConnection _connection;
        public DBOrder(SqliteConnection connection)
        {
            _connection = connection;
        }

        public void InitTable(bool dropTable = false)
        {
            SqliteCommand command = _connection.CreateCommand();

            if (dropTable)
                command.CommandText = $"DROP TABLE IF EXISTS {tableName};";

            command.CommandText += $"CREATE TABLE IF NOT EXISTS {tableName} (" +
                "OrderID INTEGER PRIMARY KEY AUTOINCREMENT," +
                "CustomerName TEXT," +
                "CustomerAddress TEXT" +
                ");";
            command.ExecuteNonQuery();
        }

        public void Purge()
        {
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"DELETE FROM {tableName}";
            command.ExecuteNonQuery();
        }

        public IList<Order> GetAll()
        {
            List<Order> result    = new();
            SqliteCommand command = _connection.CreateCommand();
            Order element;

            command.CommandText = $"SELECT * FROM {tableName}";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    element = new();
                    element.OrderID         = reader.GetInt32(0);
                    element.CustomerName    = reader.GetString(1);
                    element.CustomerAddress = reader.GetString(2);
                    result.Add(element);
                }
            }

            return result;
        }

        public bool Get(ref Order obj)
        {
            bool result = false;
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"SELECT * FROM {tableName} WHERE OrderID = {obj.OrderID}";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    result = true;
                    obj.CustomerName    = reader.GetString(1);
                    obj.CustomerAddress = reader.GetString(2);
                }
            }

            return result;
        }

        public bool Create(ref Order obj)
        {
            bool result;
            SqliteCommand commandInsert;
            SqliteCommand commandSelect;

            using (var transaction = _connection.BeginTransaction())
            {
                commandInsert = _connection.CreateCommand();
                commandInsert.Transaction = transaction;
                commandInsert.CommandText = $"INSERT INTO {tableName} (" +
                    $"CustomerName," +
                    $"CustomerAddress" +
                    $")" +
                    $"VALUES (" +
                    $"'{obj.CustomerName}'," +
                    $"'{obj.CustomerAddress}'" +
                    $");";
                result = commandInsert.ExecuteNonQuery() > 0;

                if (result)
                {
                    commandSelect = _connection.CreateCommand();
                    commandSelect.Transaction = transaction;
                    commandSelect.CommandText = "SELECT last_insert_rowid()";

                    obj.OrderID = Convert.ToInt32(commandSelect.ExecuteScalar());
                }

                transaction.Commit();
            }

            return result;
        }

        public bool Update(Order obj)
        {
            bool result;
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"UPDATE {tableName} SET " +
                $"CustomerName = '{obj.CustomerName}'," +
                $"CustomerAddress = '{obj.CustomerAddress}' " +
                $"WHERE OrderID = {obj.OrderID}";
            result = command.ExecuteNonQuery() > 0;

            return result;
        }

        public bool Delete(Order obj)
        {
            bool result;
            SqliteCommand command = _connection.CreateCommand();

            command.CommandText = $"DELETE FROM {tableName} WHERE OrderID = {obj.OrderID}";
            result = command.ExecuteNonQuery() > 0;

            return result;
        }
    }
}

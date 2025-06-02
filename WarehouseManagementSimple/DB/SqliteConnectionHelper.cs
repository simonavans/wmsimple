using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public static class SqliteConnectionHelper
    {
        private static SqliteConnection? _connection = null;
        private static object _lock = new();

        public static void Init(string connectionString = "Data Source = WarehouseManagementSimple.db", bool dropTables = false)
        {
            lock(_lock)
            {
                if (_connection != null)
                {
                    throw new InvalidOperationException("SqliteConnection has already been initialized!");
                }
                _connection = new SqliteConnection(connectionString);
                _connection.Open();
            }
        }

        public static SqliteConnection GetInstance()
        {
            if (_connection == null)
            {
                throw new InvalidOperationException("SqliteConnection has not been initialized!");
            }

            return _connection;
        }

        private static void InitTables(bool dropTables = false)
        {
            DBArticle dBArticle = new(GetInstance());
            dBArticle.InitTable(dropTables);

            DBLocation dBLocation = new(GetInstance());
            dBLocation.InitTable(dropTables);

            DBOrder dBOrder = new(GetInstance());
            dBOrder.InitTable(dropTables);

            DBOrderContent dBOrderContent = new(GetInstance());
            dBOrderContent.InitTable(dropTables);

            DBStUnit dBStUnit = new(GetInstance());
            dBStUnit.InitTable(dropTables);

            DBStUnitContent dBStUnitContent = new(GetInstance());
            dBStUnitContent.InitTable(dropTables);
        }
    }
}

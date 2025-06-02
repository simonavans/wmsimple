using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Menu
{
    internal abstract class DatabaseMenuComponent : MenuComposite
    {
        protected readonly SqliteConnection _connection;

        protected DatabaseMenuComponent(string menuName, SqliteConnection connection, params MenuComponent[] menuItems) : base(menuName, menuItems)
        {
            _connection = connection;
        }
    }
}

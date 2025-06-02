using Core;
using DB;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Menu.SetMenus
{
    internal class SetLocationIDMenu : DatabaseMenuComponent
    {
        protected DBLocation locationTable;
        protected IList<Location> locations;

        public SetLocationIDMenu(string menuName, SqliteConnection connection, params MenuComponent[] menuItems) : base(menuName, connection, menuItems)
        {
            locationTable = new DBLocation(_connection);
        }

        public override void ShowOptions()
        {
            base.ShowOptions();

            locations = locationTable.GetAll();

            if (locations.Count <= 0)
            {
                Console.WriteLine("No options available");
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < locations.Count; i++)
            {
                sb.AppendLine($"{i} - {locations[i].ToString()}");
            }

            Console.WriteLine(sb.ToString());
        }

        protected override MenuComponent UseInput(string input)
        {
            try
            {
                if (int.TryParse(input, out int id))
                {
                    IRequireLocationID setLocationParent = Parent as IRequireLocationID;
                    setLocationParent.SetLocationId(locations[id].LocationID);
                    return Parent;
                }
                else
                {
                    throw new ArgumentException("Input invalid");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this;
            }

            return this;
        }
    }
}

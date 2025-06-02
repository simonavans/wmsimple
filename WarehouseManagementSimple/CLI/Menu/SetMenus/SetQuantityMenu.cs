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
    internal class SetQuantityMenu : DatabaseMenuComponent
    {
        public SetQuantityMenu(string menuName, SqliteConnection connection, params MenuComponent[] menuItems) : base(menuName, connection, menuItems)
        {

        }

        public override void ShowOptions()
        {
            base.ShowOptions();

            Console.WriteLine("Enter the required quantity:");
        }

        protected override MenuComponent UseInput(string input)
        {
            try
            {
                if (int.TryParse(input, out int quantity))
                {
                    IRequireQuantity setQuantityParent = Parent as IRequireQuantity;
                    setQuantityParent.SetQuantity(quantity);
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

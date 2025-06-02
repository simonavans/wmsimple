using Core;
using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Menu.Picking
{
    internal class PickingMenu : MenuComponent, IPickingMenuState
    {
        public StUnit? LoadedStUnit { get; set; }
        public Location MobileUnitLocation { get; init; }

        private bool _validConfig = true;

        public PickingMenu(string menuName, params MenuComponent[] menuItems)
            : base(menuName, menuItems)
        {
            DBLocation dbLocation = new(SqliteConnectionHelper.GetInstance());
            Location location = new() { Mha = "MOBILEUNIT" };

            if (!dbLocation.Get(ref location))
            {
                Console.WriteLine($"Could not find MobileUnitLocation using MHA <{location.Mha}>");
                _validConfig = false;
            }

            LoadedStUnit = null;
            MobileUnitLocation = location;
        }


        public override void ShowOptions()
        {
            StringBuilder sb = new();
            ConsoleColor previousColor = Console.ForegroundColor;

            if (!_validConfig)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                sb.AppendLine("Config is invalid, press q to return.");
                Console.WriteLine(sb.ToString());
                Console.ForegroundColor = previousColor;
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Loaded Box: {(LoadedStUnit == null ? "None" : LoadedStUnit.StUnitID) }");
            Console.ForegroundColor = previousColor;

            sb.AppendLine();
            sb.AppendLine("Options:");
            sb.AppendLine("1 Load Box");
            sb.AppendLine("2 Pick Order");
            sb.AppendLine("3 Put Box");
            sb.AppendLine();
            sb.AppendLine("q Return to previous menu.");
            Console.WriteLine(sb.ToString());
        }

        protected override MenuComponent UseInput(string input)
        {
            if (!_validConfig)
                return this;

            switch(input)
            {
                case "1":
                    return new PickLoadMenu("PickLoadMenu", this, this);
                case "2":
                    if (LoadedStUnit == null)
                    {
                        var originalColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Load a box before picking an Order!");
                        Console.ForegroundColor = originalColor;
                        return this;
                    }
                    else
                        return new PickOrderMenu("PickOrderMenu", this, this);
                case "3":
                    if (LoadedStUnit == null)
                    {
                        var originalColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Load a box before putting it!");
                        Console.ForegroundColor = originalColor;
                        return this;
                    }
                    else
                        return new PickPutMenu("PickPutMenu", this, this);
            }
            return this;
        }
    }
}

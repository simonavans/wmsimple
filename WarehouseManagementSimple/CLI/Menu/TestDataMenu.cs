using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Menu
{
    internal class TestDataMenu : MenuComponent
    {
        DBTestDataFactory testDataFactory = new(SqliteConnectionHelper.GetInstance());

        public TestDataMenu(string menuName, params MenuComponent[] menuItems)
            : base(menuName, menuItems)
        {
        }

        public override void ShowOptions()
        {
            StringBuilder sb = new();
            sb.AppendLine("0 Insert TestData into Database.");
            sb.AppendLine("1 PURGE ALL existing data and insert TestData.");
            sb.AppendLine("2 PURGE ALL existing data.");
            sb.AppendLine();
            sb.AppendLine("q Return to previous menu.");
            Console.WriteLine(sb.ToString());
        }

        protected override MenuComponent UseInput(string input)
        {
            StringBuilder sb;

            lock (testDataFactory)
            {
                switch (input)
                {
                    case "0":
                        try
                        {
                            testDataFactory.LoadAllData();
                        }
                        catch (Exception ex)
                        {
                            sb = new();
                            sb.AppendLine("Could not insert TestData, the data most likely already exists.");
                            sb.AppendLine($"Internal Error: {ex.Message}");
                            Console.WriteLine(sb.ToString());
                        }
                        break;
                    case "1":
                        testDataFactory.PurgeData();
                        testDataFactory.LoadAllData();
                        break;
                    case "2":
                        testDataFactory.PurgeData();
                        break;
                }
            }

            return this;
        }
    }
}

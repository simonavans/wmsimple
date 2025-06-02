using CLI.Menu.InboundStock;
using CLI.Menu.SetMenus;
using Microsoft.Data.Sqlite;
using CLI.Menu.Picking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Menu;

internal static class Menu
{
    public static async Task RunAsync(MenuComponent menuComponent, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            menuComponent.ShowOptions();

            Task<string?> inputTask = Console.In.ReadLineAsync();
            await inputTask;
            var input = inputTask.Result;
            if (input == null)
                continue;

            input = input.Trim();

            MenuComponent newComponent = menuComponent.UseUserInput(input);
            if (newComponent != null)
                menuComponent = newComponent;

            Console.WriteLine(string.Empty);
        }
    }

    public static MenuComponent CreateMenu(CancellationTokenSource cts, SqliteConnection sqliteConnection, MessageBus.IMessageBus messageBus)
    {
        MenuComponent topMenu = new TopMenu(
          "Top menu",
          cts,
          new InboundStockMenu("Create inbound stock", messageBus, [
              new SetArticleMenu("Set article", sqliteConnection),
            new SetLocationIDMenu("Set location", sqliteConnection),
            new SetQuantityMenu("Set quantity", sqliteConnection)
              ]),
          new PickingMenu("Picking"),
          new TestDataMenu("Test data options")
          );
      return topMenu;
    }
}

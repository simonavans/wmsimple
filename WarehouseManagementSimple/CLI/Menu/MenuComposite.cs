using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Menu;

internal class MenuComposite : MenuComponent
{
  public MenuComposite(string name, params MenuComponent[] menuItems) : base(name, menuItems)
  {
    foreach (MenuComponent child in menuItems)
    {
      child.Parent = this;
    }
  }

  public override void ShowOptions()
  {
    StringBuilder sb = new StringBuilder();
    ConsoleColor currentColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Green;

    sb.AppendLine($"Current menu: {this.MenuName}.");
    sb.AppendLine("Select an item using it's index, q to return or CTRL + C to exit.");
    Console.WriteLine(sb.ToString());
    Console.ForegroundColor = currentColor;

    sb.Clear();
    for (int i = 0; i < MenuItems.Count; i++)
    {
      sb.AppendLine($"{i} - {MenuItems[i].MenuName}");
    }
    Console.WriteLine(sb.ToString());
  }

  protected override MenuComponent UseInput(string input)
  {
    try
    {
      if (int.TryParse(input, out int id))
      {
        if (id < 0 || id >= MenuItems.Count)
        {
          throw new IndexOutOfRangeException("Index out of bounds");
        }

        return MenuItems[id];
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
  }
}

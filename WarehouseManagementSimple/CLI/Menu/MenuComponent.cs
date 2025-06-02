using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Menu
{
  internal abstract class MenuComponent
  {
    public IList<MenuComponent> MenuItems { get; init; }
    public string MenuName { get; init; }
    public MenuComponent Parent { get; set; }

    protected MenuComponent(string menuName, params MenuComponent[] menuItems)
    {
      MenuItems = menuItems;
      MenuName = menuName;
    }

    public abstract void ShowOptions();

    public override string ToString()
    {
      return MenuName;
    }

    protected abstract MenuComponent UseInput(string input);

    public virtual MenuComponent UseUserInput(string input)
    {
      if (input.Equals("q"))
          return Parent;

      return UseInput(input);
    }
  }
}
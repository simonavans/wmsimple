using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Menu
{
    internal class TopMenu : MenuComposite
    {
        private readonly CancellationTokenSource cancellationTokenSource;

        public TopMenu(string name, CancellationTokenSource cts, params MenuComponent[] menuItems) : base(name, menuItems)
        {
            cancellationTokenSource = cts;
        }

        public override MenuComponent UseUserInput(string input)
        {
            if (input.Equals("q"))
                cancellationTokenSource.Cancel();

            return base.UseUserInput(input);
        }
    }
}

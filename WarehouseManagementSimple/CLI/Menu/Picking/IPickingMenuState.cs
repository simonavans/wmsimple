using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Menu.Picking
{
    internal interface IPickingMenuState
    {
        public StUnit? LoadedStUnit { get; set; }
        public Location MobileUnitLocation { get; init; }
    }
}

using Core;
using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Menu.Picking
{
    internal class PickLoadMenu : MenuComponent
    {
        private readonly IPickingMenuState _pickingMenuState;
        private IList<StUnit> _cachedStUnits;

        public PickLoadMenu(string menuName, MenuComponent parent, IPickingMenuState pickingMenuState)
            : base(menuName)
        {
            Parent = parent;
            _pickingMenuState = pickingMenuState;
        }

        public override void ShowOptions()
        {
            DBStUnit dBStUnit = new(SqliteConnectionHelper.GetInstance());
            StringBuilder sb = new();

            _cachedStUnits = dBStUnit.GetAll().Where(s => s.FK_LocationID == _pickingMenuState.MobileUnitLocation.LocationID).ToArray();

            sb.AppendLine($"There's currently {_cachedStUnits.Count} box(es) available to be loaded.");

            for (int i = 0; i < _cachedStUnits.Count; ++i)
            {
                StUnit stUnit = _cachedStUnits[i];
                sb.AppendLine($"{i + 1} ID<{stUnit.StUnitID}> Type <{stUnit.StUnitType}>");
            }
            sb.AppendLine();
            sb.AppendLine("Enter 0 to create a new box.");

            Console.WriteLine(sb.ToString());
        }

        protected override MenuComponent UseInput(string input)
        {
            switch(input)
            {
                case "0":
                    DBStUnit dBStUnit = new(SqliteConnectionHelper.GetInstance());
                    StUnit stUnit = new() { StUnitType = "PICKBOX", FK_LocationID = _pickingMenuState.MobileUnitLocation.LocationID };
                    dBStUnit.Create(ref stUnit);
                    _pickingMenuState.LoadedStUnit = stUnit;
                    return Parent;
                default:
                    if (int.TryParse(input, out int result))
                    {
                        result -= 1;
                        if (result < 0 || result >= _cachedStUnits.Count)
                            break;
                        _pickingMenuState.LoadedStUnit = _cachedStUnits[result];
                        return Parent;
                    }
                    break;
            }
            return this;
        }
    }
}

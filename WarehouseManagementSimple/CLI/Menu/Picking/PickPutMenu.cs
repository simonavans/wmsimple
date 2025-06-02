using Core;
using DB;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Menu.Picking
{
    internal class PickPutMenu : MenuComponent
    {
        private readonly IPickingMenuState _pickingMenuState;
        IList<Location>? _cachedDestinations;
        public PickPutMenu(string menuName, MenuComponent parent, IPickingMenuState pickingMenuState, params MenuComponent[] menuItems)
            : base(menuName, menuItems)
        {
            _pickingMenuState = pickingMenuState;
            Parent = parent;
        }

        public override void ShowOptions()
        {
            StringBuilder sb = new();
            SqliteConnection conn = SqliteConnectionHelper.GetInstance();
            DBStUnitContent dbStUnitContent = new(conn);
            DBArticle dbArticle = new(conn);
            DBLocation dbLocation = new(conn);
            Article article = new();

            sb.AppendLine("The loaded box contains the following items:");
            foreach (var content in dbStUnitContent.GetAll().Where(sc => sc.FK_StUnitID == _pickingMenuState.LoadedStUnit.StUnitID))
            {
                article.ArticleID = content.FK_ArticleID;
                dbArticle.GetByArticleID(ref article);

                sb.AppendLine($"Partno <{article.Partno}> Revision <{article.Revision}> Quantity <{content.Quantity}>");
            }

            _cachedDestinations = dbLocation.GetAll().Where(l => l.Mha.StartsWith("OUTBOUND")).ToArray();

            sb.AppendLine();
            sb.AppendLine("Select a location to Put to:");
            for (int i = 0; i < _cachedDestinations.Count; ++i)
            {
                Location location = _cachedDestinations[i];
                sb.AppendLine($"{i + 1} Mha <{location.Mha}> Rack <{location.Rack}> HorCoor <{location.HorCoor}> VerCoor <{location.VerCoor}>");
            }

            Console.WriteLine(sb.ToString());
        }

        protected override MenuComponent UseInput(string input)
        {
            if (_cachedDestinations == null)
                throw new InvalidOperationException("_cachedDestinations cannot be NULL at this stage");

            DBStUnit dbStUnit = new(SqliteConnectionHelper.GetInstance());

            if (int.TryParse(input, out int inputInt))
            {
                inputInt -= 1;

                if (inputInt < 0 || inputInt >= _cachedDestinations.Count)
                    return this;

                Location location = _cachedDestinations[inputInt];
                _pickingMenuState.LoadedStUnit.FK_LocationID = location.LocationID;
                dbStUnit.Update(_pickingMenuState.LoadedStUnit);

                _pickingMenuState.LoadedStUnit = null;
                return Parent;
            }

            return this;
        }
    }
}

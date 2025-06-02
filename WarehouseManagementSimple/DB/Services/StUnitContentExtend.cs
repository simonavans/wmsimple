using Core;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Services
{
    public class StUnitContentExtend
    {
        private readonly SqliteConnection _connection;


        public StUnitContentExtend(SqliteConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StUnitContent> GetArticlePickStock(int articleID)
        {
            DBStUnit dbStunit = new(_connection);
            DBStUnitContent dbStunitContent = new(_connection);
            DBLocation dbLocation = new(_connection);
            HashSet<int> pickLocationIDs = dbLocation.GetAll().Where(l => l.Mha.StartsWith("PICKING")).Select(l => l.LocationID).ToHashSet();
            HashSet<int> pickStorageUnits = dbStunit.GetAll().Where(s => pickLocationIDs.Contains<int>(s.FK_LocationID)).Select(s => s.StUnitID).ToHashSet();

            return dbStunitContent.GetAll().Where(sc => sc.FK_OrderID <= 0 && sc.FK_ArticleID == articleID && pickStorageUnits.Contains<int>(sc.FK_StUnitID));
        }

        public int GetArticlePickStockCount(int articleID)
        {
            return GetArticlePickStock(articleID).Sum(s => s.Quantity);
        }
    }
}

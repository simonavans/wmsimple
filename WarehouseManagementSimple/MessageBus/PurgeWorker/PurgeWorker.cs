using Core;
using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBus.PurgeWorker;

public class PurgeWorker : Worker
{
    public readonly List<StUnit> _purgedStUnits = [];
    public readonly List<Order> _purgedOrders = [];

    public PurgeWorker(IMessageBus messageBus) : base(messageBus) { }

    public override Task DoWork(CancellationToken cancellationToken)
    {
        var dbConn = SqliteConnectionHelper.GetInstance();
        var dbStUnit = new DBStUnit(dbConn);
        var dbStUnitContent = new DBStUnitContent(dbConn);

        foreach (var stUnit in dbStUnit.GetAll())
        {
            var matchingContent = dbStUnitContent
                .GetAll()
                .FirstOrDefault(suc => suc.FK_StUnitID == stUnit.StUnitID);

            if (matchingContent == null)
                _purgedStUnits.Add(stUnit);
        }

        foreach (var stUnit in _purgedStUnits)
            dbStUnit.Delete(stUnit);

        var dbOrder = new DBOrder(dbConn);
        var dbOrderContent = new DBOrderContent(dbConn);

        foreach (var order in dbOrder.GetAll())
        {
            var matchingContent = dbOrderContent
                .GetAll()
                .FirstOrDefault(suc => suc.FK_OrderID == order.OrderID);

            if (matchingContent == null)
                _purgedOrders.Add(order);
        }

        foreach (var order in _purgedOrders)
            dbOrder.Delete(order);

        return Task.CompletedTask;
    }
}

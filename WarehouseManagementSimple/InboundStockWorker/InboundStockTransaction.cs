using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InboundStockWorker
{
    public class InboundStockTransaction : MessageBus.Transaction<InboundStockTransaction>
    {
        public int ArticleID { get; init; }
        public int LocationID { get; init; }
        public int Quantity { get; init; }

        public InboundStockTransaction(int article, int locationID, int quantity)
        {
            ArticleID = article;
            LocationID = locationID;
            Quantity = quantity;
        }
    }
}

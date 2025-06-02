using DB.Services;
using MessageBus;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace InboundStockWorker
{
    public class InboundStockWorker : MessageBus.Worker
    {
        public static readonly string inboundQueue = "InboundStock";
        public static readonly string logQueue = "Logging";

        public InboundStockWorker(IMessageBus messageBus) : base(messageBus)
        {
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (MessageBus.TryReadMessage(inboundQueue, out string message))
                {
                    InboundStockTransaction transaction = InboundStockTransaction.FromJson(message);

                    var SqliteConnection = DB.SqliteConnectionHelper.GetInstance();

                    // Validate article
                    ArticleExtend article = new ArticleExtend(transaction.ArticleID, SqliteConnection);
                    if (!article.IsExisting())
                    {
                        MessageBus.Write(inboundQueue, $"{typeof(InboundStockWorker)}: Article <{transaction.ArticleID}> does not exist!");
                        continue;
                    }

                    // Validate location
                    LocationExtend location = new LocationExtend(transaction.LocationID, SqliteConnection);
                    if (!location.IsExisting())
                    {
                        MessageBus.Write(inboundQueue, $"{typeof(InboundStockWorker)}: Location <{transaction.LocationID}> does not exist!");
                        continue;
                    }

                    // Check if the location is available
                    if (!location.IsAvailable())
                    {
                        MessageBus.Write(inboundQueue, $"{typeof(InboundStockWorker)}: Location <{transaction.LocationID}> is not available!");
                        continue;
                    }

                    // Create StUnit and StUnitContent
                    try
                    {
                        StUnitExtend stUnitExtend = new StUnitExtend(SqliteConnection, "PALLET", transaction.LocationID);
                        if (!stUnitExtend.AddContent(transaction.ArticleID, transaction.Quantity))
                        {
                            MessageBus.Write(inboundQueue, $"{typeof(InboundStockWorker)}: Can't add content to stunit!");
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBus.Write(inboundQueue, $"{typeof(InboundStockWorker)}: Can't create StUnit!");
                        continue;
                    }
                }

                await Task.Delay(100);
            }
        }
    }
}

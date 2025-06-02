using MessageBus;
using MessageBus.ExampleWorker;
using MessageBus.GenericMoveWorker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI
{
    internal static class WorkerSetup
    {
        public static List<IWorker> CreateWorkers(IMessageBus messageBus)
        {
            var workers = new List<IWorker>();
            //workers.Add(new ExampleWriteWorker(messageBus));
            //workers.Add(new ExampleReadWorker(messageBus));
            workers.Add(new InboundStockWorker.InboundStockWorker(messageBus));
            workers.Add(new GenericMoveWorker(messageBus, "INBOUND01", "PICKINGAREA"));
            return workers;
        }

        public static async Task RunWorkersAsync(List<IWorker> workers, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();
            foreach (var worker in workers)
            {
                tasks.Add(worker.DoWork(cancellationToken));
            }

            await Task.WhenAll(tasks);
        }
    }
}

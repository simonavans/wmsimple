using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBus.ExampleWorker
{
    public class ExampleReadWorker : Worker
    {
        private const string queueName = "Example queue";

        public ExampleReadWorker(IMessageBus messageBus) : base(messageBus) { }

        public override async Task DoWork(CancellationToken cancellationToken)
        {

            while (!cancellationToken.IsCancellationRequested)
            {
                if (MessageBus.TryReadMessage(queueName, out string message))
                {
                    Console.WriteLine($"Read from queue: <{queueName}> message: <{message}>");
                }

                await Task.Delay(500);
            }
        }
    }
}

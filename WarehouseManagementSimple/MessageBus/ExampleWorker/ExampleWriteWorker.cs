using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBus.ExampleWorker
{
    public class ExampleWriteWorker : Worker
    {
        private const string queueName = "Example queue";

        public ExampleWriteWorker(IMessageBus messageBus) : base(messageBus) { }

        public override async Task DoWork(CancellationToken cancellationToken)
        {

            while (!cancellationToken.IsCancellationRequested)
            {
                string message = "Hello worker!";

                if (!MessageBus.Write(queueName, message))
                {
                    Console.WriteLine($"Writing to queue <{queueName}> failed for message <{message}>");
                }

                await Task.Delay(500);
            }
        }
    }
}

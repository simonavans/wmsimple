using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBus.PickWorker
{
    public class PickWorker : Worker
    {
        public PickWorker(IMessageBus messageBus) : base(messageBus)
        {
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBus
{
    public abstract class Worker : IWorker
    {
        protected IMessageBus MessageBus { get; init; }

        protected Worker(IMessageBus messageBus)
        {
            this.MessageBus = messageBus;
        }

        public abstract Task DoWork(CancellationToken cancellationToken);
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBus
{
    public class InMemoryMessageBus : IMessageBus
    {
        private Dictionary<string, ConcurrentQueue<string>> messageQueues;

        private static InMemoryMessageBus instance;
        private static readonly object singletonLock = new object();

        private InMemoryMessageBus()
        {
            this.messageQueues = new Dictionary<string, ConcurrentQueue<string>>();
        }

        public static InMemoryMessageBus GetInstance()
        {
            if (instance == null)
            {
                lock (singletonLock) {
                    if (instance == null)
                    {
                        instance = new InMemoryMessageBus();
                    }
                }
            }
            return instance;
        }

        public bool TryReadMessage(string queueName, out string message)
        {
            message = string.Empty;
            queueName = queueName.Trim().ToLower();

            if (!this.messageQueues.ContainsKey(queueName))
                return false;

            if (this.messageQueues[queueName] == null)
                return false;

            if (!this.messageQueues[queueName].TryDequeue(out message))
                return false;

            return true;
        }

        public bool Write(string queueName, string payload)
        {
            queueName = queueName.Trim().ToLower();

            if (!this.messageQueues.ContainsKey(queueName))
            {
                this.messageQueues.Add(queueName, new ConcurrentQueue<string>());
            }

            if (this.messageQueues[queueName] != null)
            {
                this.messageQueues[queueName].Enqueue(payload);
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}

using MessageBus;
using CLI.Menu;
using DB;

namespace CLI
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            //Database Initialization
            DB.SqliteConnectionHelper.Init();
            new DBTestDataFactory(DB.SqliteConnectionHelper.GetInstance()).CreateTables(false);

            IMessageBus messageBus = InMemoryMessageBus.GetInstance();
            List<IWorker> workers = WorkerSetup.CreateWorkers(messageBus);
            Task workerTask = WorkerSetup.RunWorkersAsync(workers, cts.Token);

            // Create menu
            MenuComponent menuComponent = Menu.Menu.CreateMenu(cts, SqliteConnectionHelper.GetInstance(), messageBus);

            // Run the menu and workers async. Both will stop when the user cancels the operation with the token.
            await Menu.Menu.RunAsync(menuComponent, cts.Token);
            await workerTask;
        }
    }
}

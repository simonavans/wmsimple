using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CLI.Menu.SetMenus;
using Core;
using DB;
using InboundStockWorker;
using Microsoft.Data.Sqlite;

namespace CLI.Menu.InboundStock
{
    internal class InboundStockMenu : MenuComposite, IRequireArticle, IRequireLocationID, IRequireQuantity
    {
        private MessageBus.IMessageBus MessageBus { get; init; }
        private int articleID = -1;
        private int locationID = -1;
        private int quantity = -1;

        public InboundStockMenu(string menuName, MessageBus.IMessageBus messageBus, params MenuComponent[] menuItems) : base(menuName, menuItems)
        {
            MessageBus = messageBus;
        }

        public override void ShowOptions()
        {
            // Show current menu and options
            base.ShowOptions();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{MenuItems.Count} - Create inbound stock with following parameters:");
            sb.AppendLine($"ArticleID: {articleID}, LocationID {locationID}, Quantity {quantity}");

            Console.WriteLine(sb.ToString());
        }

        protected override MenuComponent UseInput(string input)
        {
            try
            {
                if (int.TryParse(input, out int id))
                {
                    if (id == MenuItems.Count)
                    {
                        InboundStockTransaction transaction = new(articleID, locationID, quantity);
                        string message = InboundStockTransaction.ToJson(transaction);

                        if (!MessageBus.Write(InboundStockWorker.InboundStockWorker.inboundQueue, message))
                            Console.WriteLine($"Could not write message to queue <{InboundStockWorker.InboundStockWorker.inboundQueue}>. Message: {message}");
                        else
                            Console.WriteLine($"Wrote message to queue <{InboundStockWorker.InboundStockWorker.inboundQueue}>. Message: {message}");

                        return this;
                    }
                    else
                    {
                        return base.UseInput(input);
                    }
                }
                else
                {
                    throw new ArgumentException("Input invalid");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this;
            }
        }

        public void SetArticle(int articleID)
        {
            this.articleID = articleID;
        }

        public void SetLocationId(int locationID)
        {
            this.locationID = locationID;
        }

        public void SetQuantity(int quantity)
        {
            this.quantity = quantity;
        }
    }
}

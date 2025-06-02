using Core;
using DB;
using DB.Services;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Menu.Picking
{
    internal class PickOrderMenu : MenuComponent
    {
        private readonly IPickingMenuState _pickingMenuState;
        private IList<Order>? _cachedOrders;
        private IList<OrderContent>? _cachedOrderContent;
        private Order? _selectedOrder = null;
        public PickOrderMenu(string menuName, MenuComponent parent, IPickingMenuState pickingMenuState, params MenuComponent[] menuItems)
            : base(menuName, menuItems)
        {
            _pickingMenuState = pickingMenuState;
            Parent = parent;
        }

        private void ShowOptionsSelectOrder()
        {
            StringBuilder sb = new();
            SqliteConnection conn = SqliteConnectionHelper.GetInstance();
            DBOrder dbOrder = new(conn);
            DBOrderContent dbOrderContent = new(conn);
            DBStUnitContent dbStUnitContent = new(conn);

            _cachedOrders = dbOrder.GetAll();
            sb.AppendLine("Select Order to start picking:");
            for (int i = 0; i < _cachedOrders.Count; ++i)
            {
                Order order     = _cachedOrders[i];
                int   totalQty  = dbOrderContent.GetAll().Where(oc => oc.FK_OrderID == order.OrderID).Sum(oc => oc.Quantity);
                int   pickedQty = dbStUnitContent.GetAll().Where(sc => sc.FK_OrderID == order.OrderID).Sum(sc => sc.Quantity);

                sb.AppendLine($"{i + 1} ID <{order.OrderID}> Customer Name <{order.CustomerName}> Picked <{pickedQty}/{totalQty}>");
            }

            Console.WriteLine(sb.ToString());
        }

        private void ShowOptionsPickOrder()
        {
            SqliteConnection conn = SqliteConnectionHelper.GetInstance();
            DBOrderContent     dbOrderContent  = new(conn);
            DBStUnitContent    dbStUnitContent = new(conn);
            DBArticle          dbArticle       = new(conn);
            StUnitContentExtend stUnitContentExtend = new(conn);
            Article         article = new();
            ConsoleColor originalColor = Console.ForegroundColor;

            var stUnitContentCache = dbStUnitContent.GetAll();
            _cachedOrderContent = dbOrderContent.GetAll().Where(dc => dc.FK_OrderID == _selectedOrder.OrderID).ToArray();

            Console.WriteLine($"The selected OrderID <{_selectedOrder.OrderID}> contains the following items:");
            for (int i = 0; i < _cachedOrderContent.Count; ++i)
            {
                OrderContent orderContent = _cachedOrderContent[i];
                article.ArticleID = orderContent.FK_ArticleID;
                dbArticle.GetByArticleID(ref article);
                int pickedQty = stUnitContentCache.Where(sc => sc.FK_ArticleID == orderContent.FK_ArticleID && sc.FK_OrderID == _selectedOrder.OrderID).Sum(sc => sc.Quantity);
                int availablePickStock = stUnitContentExtend.GetArticlePickStockCount(article.ArticleID);

                // Print pickline green if it's already picked
                if (pickedQty == orderContent.Quantity) Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{i + 1} Picked <{pickedQty}/{orderContent.Quantity}> Partno <{article.Partno}> Revision <{article.Revision}> AvailableStock <{availablePickStock}>");
                if (pickedQty == orderContent.Quantity) Console.ForegroundColor = originalColor;
            }

            Console.WriteLine($"Type in the number to pick the article.");
        }

        public override void ShowOptions()
        {
            if (_selectedOrder == null)
            {
                ShowOptionsSelectOrder();
            }
            else
            {
                ShowOptionsPickOrder();
            }
        }

        private void UseInputSelectOrder(string input)
        {
            if (_cachedOrders == null)
                throw new InvalidOperationException("_cachedOrders cannot be NULL at this stage.");

            if (!int.TryParse(input, out int result))
                return;

            result -= 1;
            if (result < 0 || result >= _cachedOrders.Count)
                return;

            _selectedOrder = _cachedOrders[result];
        }

        private void UseInputPickOrder(string input)
        {
            SqliteConnection conn = SqliteConnectionHelper.GetInstance();
            DBStUnitContent    dbStUnitContent = new(conn);
            if (_cachedOrderContent == null)
                throw new InvalidOperationException("_cachedOrderContent cannot be NULL at this stage.");

            if (!int.TryParse(input, out int inputInt))
                return;

            inputInt -= 1;
            if (inputInt < 0 || inputInt >= _cachedOrderContent.Count)
                return;

            Pick(_cachedOrderContent[inputInt]);
        }

        private void Pick(OrderContent orderContentToPick)
        {
            SqliteConnection conn = SqliteConnectionHelper.GetInstance();
            DBStUnitContent    dbStUnitContent = new(conn);
            StUnitContentExtend stUnitContentExtend = new(conn);
            ConsoleColor originalColor = Console.ForegroundColor;

            int pickedQty = dbStUnitContent.GetAll().Where(sc => sc.FK_ArticleID == orderContentToPick.FK_ArticleID && sc.FK_OrderID == _selectedOrder.OrderID).Sum(sc => sc.Quantity);
            int leftToPick = orderContentToPick.Quantity - pickedQty;
            int availablePickStock = stUnitContentExtend.GetArticlePickStockCount(orderContentToPick.FK_ArticleID);

            if (leftToPick <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Article already picked!");
                Console.ForegroundColor = originalColor;
                return;
            }
            if (leftToPick > availablePickStock)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not enough stock to pick!");
                Console.ForegroundColor = originalColor;
                return;
            }

            while (leftToPick > 0)
            {
                var stock = stUnitContentExtend.GetArticlePickStock(orderContentToPick.FK_ArticleID).First();
                int qtyToPick = Math.Min(leftToPick, stock.Quantity);

                StUnitContent pickStUnit = new();
                pickStUnit.FK_ArticleID = orderContentToPick.FK_ArticleID;
                pickStUnit.FK_OrderID = orderContentToPick.FK_OrderID;
                pickStUnit.FK_StUnitID = _pickingMenuState.LoadedStUnit.StUnitID;
                pickStUnit.Quantity = qtyToPick;

                if (!dbStUnitContent.Create(ref pickStUnit))
                {
                    throw new InvalidOperationException("Could not create new StUnitContent.");
                }

                stock.Quantity -= qtyToPick;
                if (stock.Quantity <= 0)
                {
                    //Quantity is empty, delete the stock record
                    dbStUnitContent.Delete(stock);
                }
                else
                {
                    dbStUnitContent.Update(stock);
                }
                leftToPick -= qtyToPick;

                Console.WriteLine($"Picked {qtyToPick} items from StUnit <{stock.FK_StUnitID}>");
            }
        }

        protected override MenuComponent UseInput(string input)
        {
            if (_selectedOrder == null)
                UseInputSelectOrder(input);
            else
                UseInputPickOrder(input);

            return this;
        }
    }
}

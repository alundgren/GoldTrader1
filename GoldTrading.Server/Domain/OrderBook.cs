using System;
using System.Collections.Generic;
using System.Linq;

namespace GoldTrading.Server.Domain
{
    public class OrderBook
    {
        private List<Order> orders = new List<Order>();
        private Action<List<Trade>> onNewTrades;
        private Action<List<Order>> onNewOrders;
        private object lockObject = new object();
        private int nextOrderId = 1000;

        public OrderBook(Action<List<Trade>> onNewTrades, Action<List<Order>> onNewOrders)
        {
            this.onNewTrades = onNewTrades;
            this.onNewOrders = onNewOrders;
        }

        public void AddBuyOrder(decimal amount, int count, string userId)
        {
            AddOrder(true, amount, count, userId);
        }
        
        public void AddSellOrder(decimal amount, int count, string userId)
        {
            AddOrder(false, amount, count, userId);
        }

        public void AddOrder(bool isBuyOrder, decimal amount, int count, string userId)
        {
            lock (lockObject)
            {
                var trades = new List<Trade>();
                var newOrder = new Order
                {
                    OrderId = (nextOrderId++).ToString(),
                    UserId = userId,
                    Amount = amount,
                    Count = count,
                    RemainingCount = count,
                    IsBuyOrder = isBuyOrder
                };
                Func<decimal, decimal, bool> isLessThanOrEqual = (x, y) => isBuyOrder ? x <= y : y <= x;
                List<Order> oldOrdersToRemove = new List<Order>();                                
                foreach (var order in orders.Where(x => x.IsBuyOrder != isBuyOrder && x.UserId != userId && isLessThanOrEqual(x.Amount, amount) && x.RemainingCount > 0).OrderBy(x => isBuyOrder ? x.Amount : -x.Amount))
                {
                    var matchedCount = Math.Min(order.RemainingCount, newOrder.RemainingCount);                    
                    newOrder.RemainingCount -= matchedCount;
                    order.RemainingCount -= matchedCount;
                    trades.Add(new Trade
                    {
                        Amount = order.Amount,
                        Count = matchedCount,
                        BuyOrder = isBuyOrder ? newOrder : order,
                        SellOrder = isBuyOrder ? order : newOrder
                    });
                    if (order.RemainingCount == 0)
                        oldOrdersToRemove.Add(order);
                    if (newOrder.RemainingCount == 0)
                        break;
                }

                foreach (var order in oldOrdersToRemove)
                {
                    orders.Remove(order);
                }

                if (newOrder.RemainingCount > 0)
                {
                    orders.Add(newOrder);
                    onNewOrders?.Invoke(new List<Order> { newOrder });
                }
                if (trades.Any())
                {
                    onNewTrades?.Invoke(trades);
                }
            }            
        }
    }    
}

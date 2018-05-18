using GoldTrading.Server.Domain;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoldTrading.Server.ClientMessaging
{
    public class TradesHub : Hub
    {
        public static void PushNewTradesToClients(List<Trade> trades)
        {
            PushEventToClients("newTrades", trades);
        }

        public static void PushNewOrdersToClients(List<Order> orders)
        {
            PushEventToClients("newOrders", orders);
        }

        private static void PushEventToClients<T>(string eventName, T eventData)
        {
            //In the real world these would be filtered by userid which would be part of the client registration
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<TradesHub>();
            hubContext.Clients.All.onTradeEvent(new { eventName, eventData });
        }
    }
}
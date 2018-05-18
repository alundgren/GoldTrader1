using GoldTrading.Server.ClientMessaging;
using GoldTrading.Server.Domain;
using GoldTrading.Server.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GoldTrading.Server.Controllers
{
    [RoutePrefix("api/orderbook")]
    public class AddOrderController : ApiController
    {
        public static OrderBook OrderBook = new OrderBook(TradesHub.PushNewTradesToClients, TradesHub.PushNewOrdersToClients);

        [Route("add")]
        public IHttpActionResult Post([FromBody]AddOrderRequestModel request)
        {
            OrderBook.AddOrder(request.IsBuyOrder, request.Amount, request.Count, request.UserId);
            return Ok();
        }
    }
}

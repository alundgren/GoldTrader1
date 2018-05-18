using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GoldTrading.Server
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "AddOrderApi",
                routeTemplate: "api/orderbook/add",
                defaults: new { action = "post", controller = "AddOrder" }
            );
        }
    }
}

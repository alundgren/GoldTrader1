using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace GoldTrading.Client.Controllers
{
    public class HomeController : Controller
    {
        [Route()]
        public ActionResult Index(string userId)
        {
            var actualUserId = userId ?? "1";
            var serverBaseUrl = new Uri(WebConfigurationManager.AppSettings["ServerUrl"]);

            ViewBag.UserId = actualUserId;
            return View(new IndexModel
            {
                UserId = actualUserId,
                ServerBaseUrl = serverBaseUrl,
                ServerSignalrHubsUrl = new Uri(serverBaseUrl, "signalr/hubs"),
                GlobalJavaScriptSettings = JsonConvert.SerializeObject(new
                {
                    userId = actualUserId,
                    serverBaseUrl = serverBaseUrl.ToString(),
                    serverSignalrBaseUrl = new Uri(serverBaseUrl, "signalr").ToString()
                })
            });
        }
    }
}
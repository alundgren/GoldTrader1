using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoldTrading.Client
{
    public class IndexModel
    {
        public Uri ServerBaseUrl { get; set; }
        public Uri ServerSignalrHubsUrl { get; set; }
        public string UserId { get; internal set; }
        public string GlobalJavaScriptSettings { get; internal set; }
    }
}
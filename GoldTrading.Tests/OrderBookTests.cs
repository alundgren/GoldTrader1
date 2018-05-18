using System;
using System.Collections.Concurrent;
using System.Text;
using GoldTrading.Server.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoldTrading.Tests
{
    [TestClass]
    public class OrderBookTests
    {
        private void TestCase(Action<OrderBook> setupCase, params string[] expectedResults)
        {
            var tradesText = new StringBuilder();
            var b = new OrderBook(trades => trades.ForEach(x => tradesText.AppendLine(x.ToString())), _ => { });

            setupCase(b);

            Assert.AreEqual(string.Join(Environment.NewLine, expectedResults), tradesText.ToString().Trim());
        }

        /// <summary>
        /// Ordrarna ska skickas till en server
        /// där de läggs på en orderbok, eller matchas mot andra ordrar i orderboken med korsande pris
        /// och skapar en affär.Det vill säga, om en användare lägger in en köporder på $1350 och det
        /// ligger en säljorder i orderboken på $1300 matchas en affär på $1300.
        /// </summary>
        [TestMethod]
        public void SimpleExampleCase()
        {
            TestCase(b =>
                {
                    b.AddSellOrder(1300, 5, "user1");
                    b.AddBuyOrder(1350, 5, "user2");
                },
                "user1 sells 5 to user2 at 1300");
        }

        [TestMethod]
        public void BestPriceUsedForBuyer()
        {
            TestCase(b =>
            {
                b.AddSellOrder(1340, 5, "user3");
                b.AddSellOrder(1300, 5, "user1");
                b.AddBuyOrder(1350, 5, "user2");
            },
            "user1 sells 5 to user2 at 1300");
        }

        [TestMethod]
        public void BestPriceUserForSeller()
        {
            TestCase(b =>
            {
                b.AddBuyOrder(1340, 5, "user1");
                b.AddBuyOrder(1350, 5, "user2");
                b.AddSellOrder(1300, 5, "user3");
            },
            "user3 sells 5 to user2 at 1350");
        }
        
        [TestMethod]
        public void CanMatchMultipleOrders()
        {
            TestCase(b =>
            {
                b.AddBuyOrder(1301, 1, "user1");
                b.AddBuyOrder(1302, 1, "user2");
                b.AddSellOrder(1300, 3, "user3");
            },
            "user3 sells 1 to user2 at 1302",
            "user3 sells 1 to user1 at 1301");
        }

        [TestMethod]
        public void IsLeftInOrderBookAfterPartialMatch()
        {
            TestCase(b =>
            {
                b.AddBuyOrder(1301, 1, "user1");
                b.AddSellOrder(1300, 2, "user2");
                b.AddBuyOrder(1302, 1, "user3");
            },
            "user2 sells 1 to user1 at 1301",
            "user2 sells 1 to user3 at 1300");
        }
    }
    
}

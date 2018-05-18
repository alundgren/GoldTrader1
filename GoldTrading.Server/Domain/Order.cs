namespace GoldTrading.Server.Domain
{
    public class Order
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public bool IsBuyOrder { get; set; }
        public decimal Amount { get; set; }
        public int Count { get; set; }
        public int RemainingCount { get; set; }
    }
    
}

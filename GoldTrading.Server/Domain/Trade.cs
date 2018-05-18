namespace GoldTrading.Server.Domain
{ 
    public class Trade
    {
        public Order BuyOrder { get;  set; }
        public Order SellOrder { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return $"{SellOrder.UserId} sells {Count} to {BuyOrder.UserId} at {Amount}";
        }
    }
    
}

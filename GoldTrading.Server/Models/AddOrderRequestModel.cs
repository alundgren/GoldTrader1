namespace GoldTrading.Server.Models
{
    public class AddOrderRequestModel
    {
        public string UserId { get; set; }
        public bool IsBuyOrder { get; set; }
        public int Amount { get; set; }
        public int Count { get; set; }
    }
}

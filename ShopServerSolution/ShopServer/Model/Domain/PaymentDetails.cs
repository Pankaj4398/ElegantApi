namespace ShopServer.Model.Domain
{
    public class PaymentDetails
    {
        public Guid id { get; set; }
        public decimal amount { get; set; }
        public string provider { get; set; }
        public string status { get; set; }
        public DateTime created_at { get; set; }
        public DateTime modified_at { get; set; }
        
    }
}

namespace ElegentAPINMN.Models.Domain
{
    public class PaymentDetails
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
        public string Provider { get; set; }
        public string Status { get; set; }
        public DateTime? Created_At { get; set; }
        public DateTime? Modified_At { get; set; }
    }
}

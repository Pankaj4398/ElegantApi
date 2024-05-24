namespace ElegentAPINMN.Models.Domain
{
    public class OrderDetails
    {
        public Guid Id { get; set; }
        public string Total { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Modified_At { get; set; }
        public Guid UserId { get; set; }
        public Guid PaymentDetialsId { get; set; }

        public Users Users { get; set; }
        public PaymentDetails PaymentDetails { get; set; }
    }
}

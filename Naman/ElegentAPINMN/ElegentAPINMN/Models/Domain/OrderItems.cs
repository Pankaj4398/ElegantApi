namespace ElegentAPINMN.Models.Domain
{
    public class OrderItems
    {
        public Guid Id { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Modified_At { get; set; }
        public Guid ProductId {get; set; }
        public Guid OrderDetailsId { get; set; }

        public Product Product { get; set; }
        public OrderDetails OrderDetails { get; set; }
    }
}

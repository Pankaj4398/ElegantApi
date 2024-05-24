namespace ShopServer.Model.Domain
{
    public class OrderItems
    {
        public Guid id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime modified_at { get; set; }
        public Guid order_id { get; set; }
        public Guid product_id { get; set; }
        // Navigation property
        public Product product { get; set; }
        public OrderDetails orderDetails { get; set; }
    }
}

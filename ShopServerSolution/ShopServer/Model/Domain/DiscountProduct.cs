namespace ShopServer.Model.Domain
{
    public class DiscountProduct
    {
        public Guid id { get; set; }
        public Guid product_id { get; set; }
        public Guid discount_id { get; set; }
        // Navigation property
        public Product product { get; set; }
        public Discount discount { get; set; }
    }
}

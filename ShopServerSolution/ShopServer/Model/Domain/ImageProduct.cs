namespace ShopServer.Model.Domain
{
    public class ImageProduct
    {
        public Guid id { get; set; }
        public Guid product_id { get; set; }
        // Navigation property
        public Product product { get; set; }
    }
}

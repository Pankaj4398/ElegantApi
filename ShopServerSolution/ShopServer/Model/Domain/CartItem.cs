namespace ShopServer.Model.Domain
{
    public class CartItem
    {
        public Guid id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime modified_at { get; set; }
        public int quantity {  get; set; }
        public Guid session_id { get; set; }
        public Guid product_id { get; set; }
        // Navigation property
        public ShoppingSession shopSession { get; set; }
        public Product product { get; set; }
    }
}

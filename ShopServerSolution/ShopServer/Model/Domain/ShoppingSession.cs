namespace ShopServer.Model.Domain
{
    public class ShoppingSession
    {
        public Guid id { get; set; }
        public decimal total {  get; set; }
        public DateTime created_at { get; set; }
        public DateTime modified_at { get; set; }
        public Guid user_id { get; set; }
        // Navigation property
        public Users users {  get; set; }
    }
}

namespace ElegentAPINMN.Models.Domain
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Modified_At { get; set; }
        public Guid ShoppingSessionId { get; set; }
        public Guid Product_Id { get; set; }
        public ShoppingSession ShoppingSession { get; set; }
    }
}

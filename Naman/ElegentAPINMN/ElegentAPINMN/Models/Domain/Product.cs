namespace ElegentAPINMN.Models.Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public string sku { get; set; }
        public DateTime Modified_At { get; set; }

        public Guid DiscountId { get; set; }

        public Discount Discount { get; set; }
    }
}

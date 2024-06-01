namespace ElegentAPINMN.Models.Domain
{
    public class DiscountProduct
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid DiscountId { get; set; }

        public Discount Discount { get; set; }
        public Product Product { get; set; }
    }
}

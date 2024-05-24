namespace ShopServer.Model.Domain
{
    public class Discount
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string discription { get; set; }
        public decimal discount_percent { get; set; }
        public DateTime created_at { get; set; }
        public DateTime modified_at { get; set; }

    }
}

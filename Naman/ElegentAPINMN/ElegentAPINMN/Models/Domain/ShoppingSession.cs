namespace ElegentAPINMN.Models.Domain
{
    public class ShoppingSession
    {
        public Guid Id { get; set; }
        public int Total { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Modified_At { get; set; }
        public Guid UsersId { get; set; }
        public Users Users { get; set; }
    }
}

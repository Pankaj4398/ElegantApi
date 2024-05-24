namespace ElegentAPINMN.Models.Domain
{
    public class Users
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Telephone {  get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Modified_At { get;set; }
        public string Email {  get; set; }
    }
}

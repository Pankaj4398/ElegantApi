namespace ShopServer.Model.Domain
{
    public class Users
    {
        public Guid id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public DateTime created_at { get; set; }
        public DateTime modified_at { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set;}
        public string telephone { get; set;}
    }
}

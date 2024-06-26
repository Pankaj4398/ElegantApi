﻿namespace ShopServer.Model.Domain
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public string description { get; set; }
        public int sku { get; set; }
        public string category { get; set;}
        public DateTime created_at { get; set; }
        public DateTime modified_at { get; set; }
        public Guid discount_id { get; set; }
        // Navigation property
        public Discount Discount { get; set; }

    }
}

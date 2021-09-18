using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectPlus.Data
{
    public class ShopItem
    {
        [BsonId]
        public Guid Id { get; set; }
        public Guid ShopID { get; set; }
        public string Name { get; set; }
        public List<string> IntroImages { get; set; }
        public List<string> IntroVideos { get; set; }
        public Decimal Price { get; set; }
        public bool IsDiscount {get;set;}
        public int DiscountPercent { get; set; }
        public DateTime? DiscountUntil { get; set; }
        public string Description { get; set; }
        public int Access { get; set; }
        public int Rating { get; set; }
        public int NBuyers { get; set; }
        public int N0 { get; set; }
        public int QuantityInShop { get; set; }
    }
}

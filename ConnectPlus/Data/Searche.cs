using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectPlus.Data
{
    public class Searche
    {
        [BsonId]
        public Guid Id { get; set; }
        public Guid? ShopId { get; set; }
        public Guid? ItemId { get; set; }
        public string Name { get; set; }
        public bool IsItem { get; set; }
        public Decimal Price { get; set; }
        public int NBuyers { get; set; }
        public string IntroImage { get; set; }
        public int DiscountPercent { get; set; }
        public int NumberofShopInShoper { get; set; }
    }
}

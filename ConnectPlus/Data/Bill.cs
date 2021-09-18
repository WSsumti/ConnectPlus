using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectPlus.Data
{
    public class Bill
    {
        [BsonId]
        public Guid Id { get; set; }
        public string ShopName { get; set; }
        public Guid ShopID { get; set; }
        public string ItemName { get; set; }
        public int N0 { get; set; }
        public int Quantity { get; set; }
        public Decimal Income { get; set; }
        public bool Delivered { get; set; }
        public string Address { get; set; }
        public string Method { get; set; }
    }
}

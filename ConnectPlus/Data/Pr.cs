using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectPlus.Data
{
    public class Pr
    {
        [BsonId]
        public Guid Id { get; set; }
        public Guid ShopId { get; set; }
        public string Picture { get; set; }
        public string Content { get; set; }
        public bool IsPr { get; set; }
        public int? N0 { get; set; }
        public List<string> Comments { get; set; }
    }
}

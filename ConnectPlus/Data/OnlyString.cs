using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectPlus.Data
{
    public class OnlyString
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Keyword { get; set; }
    }
}

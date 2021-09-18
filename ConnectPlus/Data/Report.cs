using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectPlus.Data
{
    public class Report
    {
        [BsonId]
        public Guid Id { get; set; }
        public Guid ShopId { get; set; }
        public string ShopName { get; set; }
        public Decimal TotalIncome { get; set; }
    }
}

using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConnectPlus.Data
{
    public class ConcvertPackages
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Image IntroImages { get; set; }
        public string Price { get; set; }
        public bool IsDiscount { get; set; }
        public int DiscountPercent { get; set; }
        public DateTime? DiscountUntil { get; set; }
        public string Description { get; set; }
        public int Access { get; set; }
        public int Rating { get; set; }
        public int NBuyers { get; set; }
        public int N0 { get; set; }
    }
}

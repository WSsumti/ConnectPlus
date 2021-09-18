using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConnectPlus.Data
{
    public class Shoper
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Quote { get; set; }
        public string Avatar { get; set; }
        public string CoverPicture { get; set; }
        public Info Information { get; set; }
        public List<ShopItem> Packages { get; set; }
        public List<Guid> Posts { get; set; }
        public List<string> KeyRoom { get; set; }
    }
    public class Info
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Web { get; set; }
        public int nFollowers { get; set; }
        public int nLikers { get; set; }
        public List<User> Followers { get; set; }
        public List<User> Likers { get; set; }
        public int Income { get; set; }
    }
}

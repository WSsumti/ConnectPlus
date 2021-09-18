using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectPlus.Data
{
    public class RoomChat
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Key { get; set; }
        public ChatData User1 { get; set; }
        public ChatData User2 { get; set; }
    }
    public class ChatData
    {
        public string Username { get; set; }
        public List<ChatMessage> History { get; set; }
    }
}

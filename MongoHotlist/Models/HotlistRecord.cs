using System;

namespace MongoHotlist.Models
{
    public class HotlistRecord
    {
        public string Value { get; set; }
        
        public DateTime ExpiryDate { get; set; }
    }
}
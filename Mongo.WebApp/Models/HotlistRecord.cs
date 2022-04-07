using System;

namespace Mongo.WebApp.Models
{
    public class HotlistRecord
    {
        public string Value { get; set; }
        
        public DateTime ExpiryDate { get; set; }
    }
}
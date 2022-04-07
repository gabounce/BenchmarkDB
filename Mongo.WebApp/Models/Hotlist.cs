using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mongo.WebApp.Models
{
    public class Hotlist
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        
        public int TenantId { get; set; }

        public string Name { get; set; }

        public List<HotlistRecord> HotlistRecord { get; set; }
    }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace UrlShortener.Repository.Model
{
    public class UrlShorted
    {
        [BsonRequired()]
        [BsonId()]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string RealUrl { get; set; }
        public string Hash { get; set; }
        public DateTime ExpiresIn { get; set; }
        public int Clicks { get; set; }
    }
}

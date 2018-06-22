using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace UrlShortener.Repository.Model
{
    public class UrlShorted
    {
        [BsonId]
        public string Id { get; set; }
        public Uri RealUrl { get; set; }
        public string Hash { get; set; }
        public DateTime LastVisit { get; set; }
        public int Clicks { get; set; }
    }
}

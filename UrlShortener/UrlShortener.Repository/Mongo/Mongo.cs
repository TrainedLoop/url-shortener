using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace UrlShortener.Repository.Mongo
{
    public class Mongo
    {
        private IMongoDatabase Database { get; }

        public Mongo(string connectionString, string database)
        {
            var client = new MongoClient(connectionString);
            Database = client.GetDatabase(database);
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            var collectionName = typeof(T).Name;
            return Database.GetCollection<T>(collectionName);
        }
    }
}

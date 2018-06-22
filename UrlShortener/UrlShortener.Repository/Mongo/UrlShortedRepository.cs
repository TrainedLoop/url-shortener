using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using UrlShortener.Repository.Contract;
using UrlShortener.Repository.Model;

namespace UrlShortener.Repository.Mongo
{
    public class UrlShortedRepository : Mongo, IUrlShortedRepository
    {
        public IMongoCollection<UrlShorted> Collection => GetCollection<UrlShorted>();

        public UrlShortedRepository(string connectionString, string database) : base(connectionString, database)
        {
        }

        public async Task CreateAsync(UrlShorted urlShorted)
        {
            await Collection.InsertOneAsync(urlShorted);
        }

        public async Task<UrlShorted> GetHashAsync(string hash)
        {
            var filter = Builders<UrlShorted>.Filter.Eq(i => i.Hash, hash);
            var result = await Collection.FindAsync(filter);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<UrlShorted> GetUrlAsync(string url)
        {
            var filter = Builders<UrlShorted>.Filter.Eq(i => i.RealUrl, url);
            var result = await Collection.FindAsync(filter);
            return await result.FirstAsync();
        }


        public async Task<UrlShorted> CountClick(string hash)
        {
            var mimii = Builders<UrlShorted>.Filter.Eq(i => i.Hash, hash);
            var update = new BsonDocument("$inc", new BsonDocument { { "Clicks", 1 } });
            var result = await Collection.FindOneAndUpdateAsync(mimii, update);
            return result;
        }

        public async Task RemoveHash(string hash)
        {
            var filter = Builders<UrlShorted>.Filter.Eq(i => i.Hash, hash);
            await Collection.FindOneAndDeleteAsync(filter);
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Mongo.WebApp.Models;
using MongoDB.Driver;

namespace Mongo.WebApp.Services
{
    public class HotlistService
    {
        private readonly IMongoCollection<Hotlist> _hotlistCollection;
        private const string CollectionName = "Hotlists";

        public HotlistService(
            IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(
                databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);

            _hotlistCollection = mongoDatabase.GetCollection<Hotlist>(CollectionName);
        }

        public async Task<List<Hotlist>> GetAsync() =>
            await _hotlistCollection.Find(_ => true).ToListAsync();

        public async Task<Hotlist?> GetAsync(string id) =>
            await _hotlistCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Hotlist newBook) =>
            await _hotlistCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, Hotlist updatedBook) =>
            await _hotlistCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _hotlistCollection.DeleteOneAsync(x => x.Id == id);
    }
}
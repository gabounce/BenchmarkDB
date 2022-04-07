using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoHotlist.Models;
using RandomNameGeneratorNG;

namespace MongoHotlist
{
    public class HotlistService
    {
        private readonly IMongoCollection<Hotlist> _hotlistCollection;
        private readonly PersonNameGenerator _nameGenerator;
        private static readonly string ConnectionString = "mongodb://localhost:27017";
        private static readonly string DatabaseName = "testdb";
        private const string CollectionName = "Hotlists";

        public HotlistService(PersonNameGenerator nameGenerator)
        {
            _nameGenerator = nameGenerator;
            var mongoClient = new MongoClient(ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(DatabaseName);

            _hotlistCollection = mongoDatabase.GetCollection<Hotlist>(CollectionName);
        }

        public async Task<List<Hotlist>> GetAsync() =>
            await _hotlistCollection.Find(_ => true).ToListAsync();

        public async Task<Hotlist?> GetRecordAsync(string hotlistName, string hotlistRecordValue) =>
            await _hotlistCollection.Find(x => x.Name == hotlistName && x.HotlistRecord.Any(r => r.Value == hotlistRecordValue)).FirstOrDefaultAsync();

        public async Task CreateAsync(Hotlist hotlist) =>
            await _hotlistCollection.InsertOneAsync(hotlist);

        public async Task RemoveAsync(string hotlistName) =>
            await _hotlistCollection.DeleteOneAsync(x => x.Name == hotlistName);

        public async Task CreateRecordAsync(string hotlistName)
        {
            await _hotlistCollection.UpdateOneAsync(x => x.Name == hotlistName,
                new JsonUpdateDefinition<Hotlist>($"{{$push: {{ \"HotlistRecords\" : {{ \"Value\": \"{_nameGenerator.GenerateRandomFirstAndLastName()}\", \"ExpiryDate\" : ISODate(\"2022-04-03T00:00:00.000Z\")}} }} }}"));
        }
    }
}
using HildenCo.Core.Infrastructure.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatalogSvc.Infrastructure.Db
{
    public class MongoClient : IMongoClient
    {
        private readonly MongoDB.Driver.MongoClient client;
        private readonly IMongoDatabase db;
        private string col;

        public string Collection { get => col; set => col = value; }

        public MongoClient(MongoOptions o)
        {
            client = new MongoDB.Driver.MongoClient(o.ConnectionString);
            col = o.Collection;
            db = client.GetDatabase(o.Db);
        }

        public async Task<IList<T>> GetAll<T>()
        {
            var c = db.GetCollection<T>(col);
            return (await c.FindAsync(new BsonDocument())).ToList();
        }

        public async Task<IList<T>> Find<T>(string column, string value)
        {
            var c = db.GetCollection<T>(col);
            var filter = Builders<T>.Filter.Eq(column, value);
            return (await c.FindAsync<T>(filter)).ToList();
        }

        public async Task<IList<T>> Find<T>(string column, List<string> values)
        {
            var c = db.GetCollection<T>(col);
            var filter = Builders<T>.Filter.In(column, values);
            return (await c.FindAsync<T>(filter)).ToList();
        }

        public async Task Insert<T>(T item)
        {
            var c = db.GetCollection<T>(col);
            await c.InsertOneAsync(item);
        }
    }
}

using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CatalogSvc.Infrastructure
{
    public class MongoClient : IMongoClient
    {
        private readonly MongoDB.Driver.MongoClient client;
        private readonly IMongoDatabase db;
        private string col;

        public string Collection { get => col; set => col = value; }

        public MongoClient(string connStr, string db, string collection)
        {
            client = new MongoDB.Driver.MongoClient(connStr);
            col = collection;
            this.db = client.GetDatabase(db);
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

        public async Task Insert<T>(T item)
        {
            var c = db.GetCollection<T>(col);
            await c.InsertOneAsync(item);
        }

        private void Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}

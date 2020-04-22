using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogSvc.Infrastructure
{
    public class MongoClient : IMongoClient
    {
        private readonly MongoDB.Driver.MongoClient client;
        private readonly IMongoDatabase db;
        private readonly string col;

        public MongoClient(string connStr, string db, string collection)
        {
            client = new MongoDB.Driver.MongoClient(connStr);
            col = collection;
            this.db = client.GetDatabase(db);
        }

        public IList<T> GetAll<T>()
        {
            var c = db.GetCollection<T>(col);
            return c.Find(new BsonDocument()).ToList();
        }

        public void Insert<T>(T item)
        {
            var c = db.GetCollection<T>(col);
            c.InsertOne(item);
        }

        private void Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}

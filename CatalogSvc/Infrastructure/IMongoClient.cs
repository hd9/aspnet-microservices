using System.Collections.Generic;

namespace CatalogSvc.Infrastructure
{
    public interface IMongoClient
    {
        void Insert<T>(T item);
        IList<T> GetAll<T>();
    }
}
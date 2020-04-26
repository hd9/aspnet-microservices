using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatalogSvc.Infrastructure
{
    public interface IMongoClient
    {
        string Collection { get; set; }
        Task Insert<T>(T item);
        Task<IList<T>> GetAll<T>();
        Task<IList<T>> Find<T>(string column, string value);
    }
}
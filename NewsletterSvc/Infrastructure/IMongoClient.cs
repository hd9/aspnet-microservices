using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsletterSvc.Infrastructure
{
    public interface IMongoClient
    {
        Task Insert<T>(T item);
        IList<T> GetAll<T>();
    }
}
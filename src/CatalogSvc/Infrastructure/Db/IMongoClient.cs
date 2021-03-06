﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatalogSvc.Infrastructure.Db
{
    public interface IMongoClient
    {
        string Collection { get; set; }
        Task Insert<T>(T item);
        Task<IList<T>> GetAll<T>();
        Task<IList<T>> Find<T>(string column, string value);
        Task<IList<T>> Find<T>(string column, List<string> values);
    }
}
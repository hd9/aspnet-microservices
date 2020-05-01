using RecommendationSvc.Models;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationSvc.Repositories
{
    public class RecommendationRepository : IRecommendationRepository
    {
        private readonly string _connStr;
        //private readonly string insPmt = "INSERT INTO payment (account_id, created_at, last_modified, currency, amount, status) values (@account_id, sysdate(), sysdate(), @currency, @amount, @status)";
        //private readonly string insLog = "INSERT INTO log (pmt_id, created_at) values (@pmt_id, sysdate())";
        //private readonly string updPmt = "UPDATE payment (account_id, created_at, last_modified, currency, amount, status) values (@account_id, sysdate(), sysdate(), @currency, @amount, @status)";
        //private readonly string queryPmtById = "SELECT * FROM payment WHERE id = @id";
        //private readonly string queryPmtByAcctId = "SELECT * FROM payment WHERE account_id = @id";

        public RecommendationRepository(string connStr)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            _connStr = connStr;
        }

        public async Task<int> Insert(Recommendation recomm)
        {
            throw new NotImplementedException();
            // todo :: insert
            // todo :: log
        }

        public async Task<Recommendation> GetByProductId(string productId)
        {
            throw new NotImplementedException();
        }

        public async Task<Recommendation> GetByAccountId(string accountId)
        {
            throw new NotImplementedException();
        }
    }
}

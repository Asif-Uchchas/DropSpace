using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;
using System.Threading.Tasks;

namespace DropSpace.Services.Dapper
{
    public interface IDapper : IDisposable
    {
        DbConnection GetDbconnection();
        Task<List<T>> FromSql<T>(string? SqlQuery, CommandType commandType = CommandType.Text);
        Task<T> FromSqlFirstOrDefaultAsync<T>(string? SqlQuery, CommandType commandType = CommandType.Text);
        Task<T> FromSqlSingleOrDefaultAsync<T>(string? SqlQuery, CommandType commandType = CommandType.Text);
        T Get<T>(string? sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        List<T> GetAll<T>(string? sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        int Execute(string? sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T Insert<T>(string? sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T Update<T>(string? sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    }
}

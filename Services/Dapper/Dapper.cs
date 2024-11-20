using Dapper;
using DropSpace.Services.Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DropSpace.Services.Dapper
{
    public class Dapper : IDapper
    {
        private readonly IConfiguration _config;
        private string Connectionstring = "DefaultConnection";

        public Dapper(IConfiguration config)
        {
            _config = config;
        }
        public void Dispose()
        {

        }

        public async Task<List<T>> FromSql<T>(string SqlQuery, CommandType commandType = CommandType.Text)
        {
            IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            var result = await db.QueryAsync<T>(SqlQuery, commandType: commandType);
            return result.ToList();
        }

        public async Task<T> FromSqlFirstOrDefaultAsync<T>(string SqlQuery, CommandType commandType = CommandType.Text)
        {
            IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            var result = await db.QueryFirstOrDefaultAsync<T>(SqlQuery, commandType: commandType);
            return result;
        }

        public async Task<T> FromSqlSingleOrDefaultAsync<T>(string SqlQuery, CommandType commandType = CommandType.Text)
        {
            IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            var result = await db.QuerySingleOrDefaultAsync<T>(SqlQuery, commandType: commandType);
            return result;
        }

        public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
        }



        public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            return db.Query<T>(sp, parms, commandType: commandType).ToList();
        }

        public DbConnection GetDbconnection()
        {
            return new SqlConnection(_config.GetConnectionString(Connectionstring));
        }

        public T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }
    }
}

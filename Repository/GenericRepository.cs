﻿using DropSpace.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DropSpace.Context;
using DropSpace.Repository.Contracts;

namespace DropSpace.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DropSpaceDbContext _context;
        private readonly DbSet<T> _db;

        public GenericRepository(DropSpaceDbContext context)
        {
            _context = context;
            _db = _context.Set<T>();
        }

        public async Task Create(T entity)
        {
            await _db.AddAsync(entity);
        }

        

        public void Delete(T entity)
        {
            _db.Remove(entity);
        }

        public async Task<T> Find(Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null)
        {
            IQueryable<T> query = _db;
            if(includes != null)
            {
                query = includes(query);
            }

            return await query.FirstOrDefaultAsync(expression);

        }

        public async Task<IList<T>> FindAll(Expression<Func<T, bool>>? expression=null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes=null)
        {
            IQueryable<T> query = _db;

            if(expression != null)
            {
                query = query.Where(expression);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> isExists(Expression<Func<T, bool>>? expression = null)
        {
            IQueryable<T> query = _db;
            return await query.AnyAsync(expression);
        }

        public void Update(T entity)
        {
            _db.Update(entity);
        }
    }
}

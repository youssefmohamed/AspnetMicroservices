﻿using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Presistense;
using Ordering.Infrastructure.Presistense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IAsyncRepository<T> where T : class
    {
        protected readonly OrderContext _orderContext;
        public RepositoryBase(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync() 
        {
            return await _orderContext.Set<T>().ToListAsync();
        }
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _orderContext.Set<T>().Where(predicate).ToListAsync();
        }
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null
            , Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
            , string includes = null
            , bool disableTracking = true)
        {
            IQueryable<T> query = _orderContext.Set<T>();
            if(disableTracking) query.AsNoTracking();
            if(includes != null) query = query.Include(includes); 
            if(predicate != null) query = query.Where(predicate);
            if (orderBy != null)
                return await orderBy(query).ToListAsync();
            return await query.ToListAsync();
        }
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null
            , Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
            , List<Expression<Func<T, object>>> includes = null
            , bool disableTracking = true)
        {
            IQueryable<T> query = _orderContext.Set<T>();
            if (disableTracking)  query.AsNoTracking();
            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));
            if (predicate != null) query = query.Where(predicate);
            if (orderBy != null)
                return await orderBy(query).ToListAsync();
            return await query.ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _orderContext.Set<T>().FindAsync(id);
        }
        public async Task<T> AddAsync(T entity)
        {
            await _orderContext.Set<T>().AddAsync(entity);
            await _orderContext.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(T entity)
        {
            _orderContext.Entry<T>(entity).State = EntityState.Modified;
            await _orderContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(T entity)
        {
            _orderContext.Set<T>().Remove(entity);
            await _orderContext.SaveChangesAsync();
        }
    }
}

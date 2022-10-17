using BlazorDictionary.Api.Application.Interfaces.Repositories;
using BlazorDictionary.Api.Domain.Models;
using BlazorDictionary.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDictionary.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext _dbContext;


        protected DbSet<TEntity> entity => _dbContext.Set<TEntity>();

        public GenericRepository(DbContext dbContext)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        #region Insert Methods

        public virtual int Add(TEntity entity)
        {
            this.entity.Add(entity);
            return _dbContext.SaveChanges();
        }

        public virtual int Add(IEnumerable<TEntity> entities)
        {
            this.entity.AddRange(entity);
            return _dbContext.SaveChanges();
        }

        public virtual async Task<int> AddAsync(TEntity entity)
        {
            await this.entity.AddAsync(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<int> AddAsync(IEnumerable<TEntity> entities)
        {
            await this.entity.AddRangeAsync(entities);
            return await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region Update Methods

        public virtual int Update(TEntity entity)
        {
            this.entity.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            return _dbContext.SaveChanges();
        }

        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            this.entity.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region Delete Methods

        public virtual int Delete(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                this.entity.Attach(entity);
            }

            this.entity.Remove(entity);

            return _dbContext.SaveChanges();
        }

        public virtual int Delete(Guid id)
        {
            var entity = this.entity.Find(id);
            return Delete(entity);
        }

        public virtual async Task<int> DeleteAsync(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                this.entity.Attach(entity);
            }

            this.entity.Remove(entity);

            return await _dbContext.SaveChangesAsync();
        }

        public virtual Task<int> DeleteAsync(Guid id)
        {
            var entity = this.entity.Find(id);
            return DeleteAsync(entity);
        }

        public virtual bool DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {
            _dbContext.RemoveRange(entity.Where(predicate));
            return _dbContext.SaveChanges() > 0;
        }

        public virtual async Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate)
        {
            _dbContext.RemoveRange(entity.Where(predicate));
            return await _dbContext.SaveChangesAsync() > 0;
        }

        #endregion

        #region CreateOrUpdate Methods

        public virtual int AddOrUpdate(TEntity entity)
        {
            if (!this.entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
                _dbContext.Update(entity);

            return _dbContext.SaveChanges();
        }

        public virtual Task<int> AddOrUpdateAsync(TEntity entity)
        {
            if (!this.entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
                _dbContext.Update(entity);

            return _dbContext.SaveChangesAsync();
        }

        #endregion

        #region Get Methods

        public virtual IQueryable<TEntity> AsQueryable() => entity.AsQueryable();


        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = entity.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = entity.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return query;
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            TEntity found = await entity.FindAsync(id);

            if (found == null)
                return null;

            if (noTracking)
                _dbContext.Entry(found).State = EntityState.Detached;

            foreach (Expression<Func<TEntity, object>> include in includes)
            {
                _dbContext.Entry(found).Reference(include).Load();
            }

            return found;
        }

        public virtual async Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = entity;

            if (predicate != null)
                query = query.Where(predicate);

            foreach (Expression<Func<TEntity, object>> include in includes)
            {
                query = query.Include(include);
            }

            if (orderBy != null)
                query = orderBy(query);


            if (noTracking)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = entity;

            if (predicate != null)
                query = query.Where(predicate);

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return await query.SingleOrDefaultAsync();
        }

        public virtual async Task<List<TEntity>> GetAll(bool noTracking = true)
        {
            IQueryable<TEntity> query = entity;

            query = query.AsNoTracking();

            return await query.ToListAsync();
        }
        #endregion

        #region Bulk Methods

        public virtual async Task BulkAdd(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
                await Task.CompletedTask;

            await entity.AddRangeAsync(entities);

            await _dbContext.SaveChangesAsync();
        }

        public virtual Task BulkDelete(Expression<Func<TEntity, bool>> predicate)
        {
            var query = entity.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            entity.RemoveRange(query);

            return _dbContext.SaveChangesAsync();
        }

        public virtual Task BulkDelete(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
                return Task.CompletedTask;

            entity.RemoveRange(entities);

            return _dbContext.SaveChangesAsync();
        }

        public virtual Task BulkDeleteById(IEnumerable<Guid> ids)
        {
            if (ids != null && !ids.Any())
                return Task.CompletedTask;

            _dbContext.RemoveRange(entity.Where(i => ids.Contains(i.Id)));
            return _dbContext.SaveChangesAsync();
        }

        public virtual Task BulkUpdate(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
                return Task.CompletedTask;

            foreach (var entity in entities)
            {
                this.entity.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            }

            return _dbContext.SaveChangesAsync();
        }

        #endregion

        #region SaveChanges Methods

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        #endregion

        private IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, Expression<Func<TEntity, object>>[] includes)
        {
            if (includes != null)
            {
                foreach (var includeItem in includes)
                {
                    query = query.Include(includeItem);
                }
            }

            return query;
        }

        

    }
}

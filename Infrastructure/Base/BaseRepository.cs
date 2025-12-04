using Infrastructure.DatabaseContext;
using InterfaceAdapter.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Base
{
    public abstract class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity>
    where TEntity : class
    {
        protected readonly IDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        public BaseRepository(IDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null!,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!,
            string includeProperties = "", int PageIndex = 0, int PageSize = 0)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (PageIndex > 0 && PageSize > 0)
            {
                return query.Skip(PageSize * (PageIndex - 1)).Take(PageSize).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> filter = null!,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                   (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query;
        }

        public virtual TEntity GetByID(object? id)
        {
            return _dbSet.Find(id)!;
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entitiesToInsert)
        {
            _dbSet.AddRange(entitiesToInsert);
        }
        public async virtual Task InsertAsync(TEntity entity) => await _dbSet.AddAsync(entity);

        public async virtual Task InsertRangeAsync(IEnumerable<TEntity> entitiesToInsert) => await _dbSet.AddRangeAsync(entitiesToInsert);

        public virtual void TryDelete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id)!;

            if (entityToDelete != null)
                Delete(entityToDelete);
        }
        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entitiesToDelete)
        {
            _dbSet.RemoveRange(entitiesToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}

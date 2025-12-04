using System.Linq.Expressions;

namespace InterfaceAdapter.Repositories
{
    public interface IBaseRepository<TEntity>
    {
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null!,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!,
           string includeProperties = "", int PageIndex = 0, int PageSize = 0);

        public IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> filter = null!,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!, string includeProperties = "");

        public TEntity GetByID(object? id);

        public void Insert(TEntity entity);

        public void InsertRange(IEnumerable<TEntity> entitiesToInsert);

        public Task InsertAsync(TEntity entity);

        public Task InsertRangeAsync(IEnumerable<TEntity> entitiesToInsert);

        public void TryDelete(object id);

        public void Delete(TEntity entityToDelete);

        public void DeleteRange(IEnumerable<TEntity> entitiesToDelete);

        public void Update(TEntity entityToUpdate);
    }
}

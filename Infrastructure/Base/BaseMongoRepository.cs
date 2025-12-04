using Infrastructure.DatabaseContext;
using InterfaceAdapter.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Base
{
    public abstract class BaseMongoRepository<TEntity> : IBaseMongoRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoDbContext Context;
        protected IMongoCollection<TEntity> DbSet;

        protected BaseMongoRepository(IMongoDbContext context)
        {
            Context = context;
            DbSet = Context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual void Add(TEntity obj)
        {
            Context.AddCommand(() => DbSet.InsertOneAsync(obj));
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            return data.SingleOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var all = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        //public virtual void Update(TEntity obj)
        //{
        //    Context.AddCommand(() => DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.GetId()), obj));
        //}

        public virtual void Remove(Guid id)
        {
            Context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id)));
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}

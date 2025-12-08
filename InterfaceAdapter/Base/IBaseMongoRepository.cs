namespace InterfaceAdapter.Base
{
    public interface IBaseMongoRepository<TEntity>
    {
        void Add(TEntity obj);
        Task<TEntity> GetById(Guid id);
        Task<IEnumerable<TEntity>> GetAll();
        //void Update(TEntity obj);
        void Remove(Guid id);
    }
}

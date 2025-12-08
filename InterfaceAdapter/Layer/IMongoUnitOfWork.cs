
namespace InterfaceAdapter.Layer
{
    public interface IMongoUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
}

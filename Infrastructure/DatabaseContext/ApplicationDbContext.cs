using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Infrastructure.DatabaseContext
{
    public interface IDbContext
    {
        public DbSet<TEntity> Set<TEntity>() where TEntity : class;
        public EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }

    public interface IApplicationDbContext : IDbContext
    {
        public DbSet<Item>? Item { get; set; }

        public int SaveChanges();
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        public DatabaseFacade Database { get; }
        public void Dispose();
    }

    public partial class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Item>? Item { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}

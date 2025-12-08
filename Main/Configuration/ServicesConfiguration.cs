using Application;
using Application.Base;
using Infrastructure.Base;
using Infrastructure.DatabaseContext;
using Infrastructure.UnitOfWork;
using InterfaceAdapter.Layer;
using Main.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Main.Configuration
{
    public static class ServicesConfiguration
    {
        public static void AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddMediatR(typeof(ApplicationMediatEntryPoint).Assembly);
            services.AddSwaggerGen();
        }

        public static void AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // SqlServer
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>()!);

            // MongoDB
            services.AddScoped<IMongoDbContext, MongoDbContext>();
            services.AddScoped<IMongoUnitOfWork, MongoUnitOfWork>();
        }

        public static void AddCustomDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IControllerDependencies<>), typeof(ControllerDependencies<>));
            services.AddTransient<IBusinessHandlerDependencies, BusinessHandlerDependencies>();
            services.AddTransient<IRepositoryDependencies, RepositoryDependencies>();
        }
    }
}

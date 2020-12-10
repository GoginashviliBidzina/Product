using Microsoft.EntityFrameworkCore;
using Product.Infrastructure.DataBase;
using Product.Messaging.Configurations;
using Product.Messaging.Infrastructure;
using Microsoft.Extensions.Configuration;
using Product.Application.Infrastructure;
using Product.Infrastructure.Repositories;
using Product.Infrastructure.EvnetDispatching;
using Microsoft.Extensions.DependencyInjection;
using Product.Domain.ProductAggregate.Repository;
using Product.Domain.CategoryAggregate.Repository;

namespace Product.DI
{
    public class DependencyResolver
    {
        private IConfiguration _configuration { get; }

        public DependencyResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceCollection Resolve(IServiceCollection services)
        {
            services ??= new ServiceCollection();

            var connectionString = _configuration.GetConnectionString("ProductDbContext");

            services.AddScoped(x => new InternalDomainEventDispatcher(
                services.BuildServiceProvider(),
                typeof(Domain.ProductAggregate.Product).Assembly,
                typeof(Command).Assembly));

            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));

            services.Configure<AzureServiceBusConfig>(options => _configuration.GetSection(nameof(AzureServiceBusConfig)).Bind(options));

            services.AddScoped<CommandExecutor>();
            services.AddScoped<MessageSender>();
            services.AddScoped<QueryExecutor>();
            services.AddScoped<UnitOfWork>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            return services;
        }
    }
}

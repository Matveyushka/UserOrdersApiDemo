using Microsoft.Extensions.DependencyInjection;

using UserOrdersApiDemo.Domain;
using Microsoft.EntityFrameworkCore;

namespace UserOrdersApiDemo.Infrastructure
{
    public static class SqlServicesProvider
    {
        public static void AddSql(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(connectionString,
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            });

            var entitiesTypes = DomainInfoProvider.GetIEntitiesTypes();

            foreach (var entityType in entitiesTypes)
            {
                var iRepositoryType = typeof(IRepository<>)
                    .GetGenericTypeDefinition()
                    .MakeGenericType(entityType);

                var sqlRepositoryType = typeof(SqlRepository<>)
                    .GetGenericTypeDefinition()
                    .MakeGenericType(entityType);

                services.AddScoped(iRepositoryType, sqlRepositoryType);
            }
        }
    }
}
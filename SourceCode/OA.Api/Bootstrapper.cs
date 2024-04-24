using Microsoft.EntityFrameworkCore;
using OA.Api.Interfaces;
using OA.Api.Repositories;
using OA.Entities;

namespace OA.Api
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBidRepository, BidRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection AddSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OAContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("OAConnection") ?? throw new InvalidOperationException("Connection string 'OAConnection' not found."),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                    sqlOptions.MigrationsAssembly("OA.Api");
                });
            });

            return services;
        }
    }
}

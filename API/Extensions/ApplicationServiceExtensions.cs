using API.Data;
using API.Services;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions {


    public static class ApplicationServiceExtensions {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config) {
            // NEXT LINE: Lesson 12.
            services.AddDbContext<DataContext>(opt => {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
           

            /* NEXT LINE: Lesson 24. */
            services.AddCors();

            return services;
        }
    }
}
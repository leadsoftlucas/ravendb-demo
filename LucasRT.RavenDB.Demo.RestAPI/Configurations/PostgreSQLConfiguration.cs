using LucasRT.RavenDB.Demo.Application.Interfaces.Menus;
using LucasRT.RavenDB.Demo.Application.PostgreSQL_Services.Data;
using LucasRT.RavenDB.Demo.Application.PostgreSQL_Services.Menus;
using Microsoft.EntityFrameworkCore;

namespace LucasRT.RavenDB.Demo.RestAPI.Configurations
{
    public static class PostgreSQLConfiguration
    {
        public static void AddPostgreSQL(this IServiceCollection service, IConfiguration _Configuration)
        {
            service.AddDbContext<PostgreSQL>(
                options => options.UseNpgsql(_Configuration.GetConnectionString("PostgreSQL"))
                );
            service.AddScoped<IUnitOfWork, UnitOfWork>();

            service.AddScoped<IMenusService, MenusPostgreSQLService>();
        }
    }
}

using LucasRT.RavenDB.Demo.Domain.Entities.Guests;
using LucasRT.RavenDB.Demo.Domain.Entities.Menus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LucasRT.RavenDB.Demo.Application.PostgreSQL_Services.Data
{
    public class PostgreSQL : DbContext
    {
        private readonly IConfiguration _Configuration;

        public PostgreSQL()
        {
        }

        public PostgreSQL(DbContextOptions<PostgreSQL> options) : base(options)
        {
        }

        public PostgreSQL(IConfiguration configuration, DbContextOptions<PostgreSQL> options) : base(options)
        {
            _Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_Configuration.GetConnectionString("PostgreSQL"));
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgreSQL).Assembly);

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                    .Where(p => p.ClrType.IsEnum)))
                property.SetColumnType("varchar(50)");

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Modified)
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Beverage> Beverages { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<SocialNetwork> SocialNetworks { get; set; }
    }

    public class PostgreSQLDesignTimeFactory : IDesignTimeDbContextFactory<PostgreSQL>
    {
        public PostgreSQL CreateDbContext(string[] args)
        {
            // Caminho base para encontrar o appsettings.json (do projeto de startup, geralmente)
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../LucasRT.RavenDB.Demo.RestAPI");

            // Criação da configuração mAnnual (design-time)
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.Development.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("PostgreSQL");

            var optionsBuilder = new DbContextOptionsBuilder<PostgreSQL>();
            optionsBuilder.UseNpgsql(connectionString);

            return new PostgreSQL(optionsBuilder.Options);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FireplaceApi.Infrastructure.Entities.UserInformationEntities;
using FireplaceApi.Core.ValueObjects;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System;

namespace FireplaceApi.Infrastructure.Entities
{
    public class FireplaceApiContext : DbContext
    {
        public DbSet<AccessTokenEntity> AccessTokenEntities { get; set; }
        public DbSet<EmailEntity> EmailEntities { get; set; }
        public DbSet<GoogleUserEntity> GoogleUserEntities { get; set; }
        public DbSet<SessionEntity> SessionEntities { get; set; }
        public DbSet<UserEntity> UserEntities { get; set; }
        public DbSet<ErrorEntity> ErrorEntities { get; set; }
        public DbSet<FileEntity> FileEntities { get; set; }
        public DbSet<GlobalEntity> GlobalEntities { get; set; }

        public FireplaceApiContext(DbContextOptions<FireplaceApiContext> options) 
            : base(options)
        {
            //ChangeTracker.LazyLoadingEnabled = false;
            //ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public FireplaceApiContext(string connectionString)
            : base(CreateOptionsFromConnectionString(connectionString))
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.EnableSensitiveDataLogging();
            //optionsBuilder.EnableDetailedErrors();
            //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccessTokenEntityConfiguration());
            modelBuilder.ApplyConfiguration(new EmailEntityConfiguration());
            modelBuilder.ApplyConfiguration(new GoogleUserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SessionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ErrorEntityConfiguration());
            modelBuilder.ApplyConfiguration(new FileEntityConfiguration());
            modelBuilder.ApplyConfiguration(new GlobalEntityConfiguration());
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            var EditedEntities = ChangeTracker.Entries()
                .Where(E => E.State == EntityState.Modified)
                .ToList();

            EditedEntities.ForEach(E =>
            {
                E.Property("ModifiedDate").CurrentValue = DateTime.UtcNow;
            });

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public void DetachAllEntries()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                Entry(entry.Entity).State = EntityState.Detached;
            }
        }

        public static DbContextOptions<FireplaceApiContext> CreateOptionsFromConnectionString(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FireplaceApiContext>();
            optionsBuilder.UseNpgsql(connectionString);
            return optionsBuilder.Options;
        }
    }
}
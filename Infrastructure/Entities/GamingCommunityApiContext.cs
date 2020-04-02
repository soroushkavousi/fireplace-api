using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GamingCommunityApi.Infrastructure.Entities.UserInformationEntities;
using GamingCommunityApi.Core.ValueObjects;

namespace GamingCommunityApi.Infrastructure.Entities
{
    public class GamingCommunityApiContext : DbContext
    {
        public IConfiguration Configuration { get; }
        public DbSet<AccessTokenEntity> AccessTokenEntities { get; set; }
        public DbSet<EmailEntity> EmailEntities { get; set; }
        public DbSet<SessionEntity> SessionEntities { get; set; }
        public DbSet<UserEntity> UserEntities { get; set; }
        public DbSet<ErrorEntity> ErrorEntities { get; set; }
        public DbSet<FileEntity> FileEntities { get; set; }
        public DbSet<GlobalEntity> GlobalEntities { get; set; }

        public GamingCommunityApiContext(DbContextOptions<GamingCommunityApiContext> options, IConfiguration configuration) 
            : base(options)
        {
            Configuration = configuration;
            //ChangeTracker.LazyLoadingEnabled = false;
            //ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql(Configuration.GetConnectionString("MainDatabase"));
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
            modelBuilder.ApplyConfiguration(new SessionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ErrorEntityConfiguration());
            modelBuilder.ApplyConfiguration(new FileEntityConfiguration());
            modelBuilder.ApplyConfiguration(new GlobalEntityConfiguration());
        }

        public void DetachAllEntries()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                Entry(entry.Entity).State = EntityState.Detached;
            }
        }
    }
}
using FireplaceApi.Infrastructure.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.Entities;

public class ProjectDbContext : DbContext
{
    private readonly ILogger<ProjectDbContext> _logger;

    public DbSet<CommentEntity> CommentEntities { get; set; }
    public DbSet<CommentVoteEntity> CommentVoteEntities { get; set; }

    public DbSet<CommunityEntity> CommunityEntities { get; set; }
    public DbSet<CommunityMembershipEntity> CommunityMembershipEntities { get; set; }

    public DbSet<PostEntity> PostEntities { get; set; }
    public DbSet<PostVoteEntity> PostVoteEntities { get; set; }

    public DbSet<EmailEntity> EmailEntities { get; set; }
    public DbSet<GoogleUserEntity> GoogleUserEntities { get; set; }
    public DbSet<SessionEntity> SessionEntities { get; set; }
    public DbSet<UserEntity> UserEntities { get; set; }

    public DbSet<ConfigsEntity> ConfigsEntities { get; set; }
    public DbSet<ErrorEntity> ErrorEntities { get; set; }
    public DbSet<FileEntity> FileEntities { get; set; }

    public DbSet<RequestTraceEntity> RequestTraceEntities { get; set; }

    public ProjectDbContext(ILogger<ProjectDbContext> logger, DbContextOptions<ProjectDbContext> options)
        : base(options)
    {
        _logger = logger;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public ProjectDbContext(string connectionString)
        : base(CreateOptionsFromConnectionString(connectionString))
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasCollation(Constants.CaseInsensitiveCollationName,
            locale: "en-u-ks-primary", provider: "icu", deterministic: false);
#pragma warning disable CS0618 // Type or member is obsolete
        modelBuilder.UseDefaultColumnCollation(Constants.CaseInsensitiveCollationName);
#pragma warning restore CS0618 // Type or member is obsolete

        modelBuilder.ApplyConfiguration(new CommentEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CommentVoteEntityConfiguration());

        modelBuilder.ApplyConfiguration(new CommunityEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CommunityMembershipEntityConfiguration());

        modelBuilder.ApplyConfiguration(new PostEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PostVoteEntityConfiguration());

        modelBuilder.ApplyConfiguration(new EmailEntityConfiguration());
        modelBuilder.ApplyConfiguration(new GoogleUserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new SessionEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());

        modelBuilder.ApplyConfiguration(new ConfigsEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ErrorEntityConfiguration());
        modelBuilder.ApplyConfiguration(new FileEntityConfiguration());

        modelBuilder.ApplyConfiguration(new RequestTraceEntityConfiguration());
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

    public static DbContextOptions<ProjectDbContext> CreateOptionsFromConnectionString(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return optionsBuilder.Options;
    }

    private void EnableDeepLogging(DbContextOptionsBuilder optionsBuilder)
    {
        if (_logger != null)
            optionsBuilder.LogTo((log) => _logger.LogAppInformation(log));
    }
}
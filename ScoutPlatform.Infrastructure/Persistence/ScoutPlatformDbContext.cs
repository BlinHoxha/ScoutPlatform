using Microsoft.EntityFrameworkCore;
using ScoutPlatform.Infrastructure.Persistence.Entities;

namespace ScoutPlatform.Infrastructure.Persistence;

public sealed class ScoutPlatformDbContext : DbContext
{
    public ScoutPlatformDbContext(DbContextOptions<ScoutPlatformDbContext> options)
        : base(options)
    {
    }

    public DbSet<PlayerEntity> Players => Set<PlayerEntity>();
    public DbSet<PlayerMetricEntity> PlayerMetrics => Set<PlayerMetricEntity>();
    public DbSet<MetricDefinitionEntity> MetricDefinitions => Set<MetricDefinitionEntity>();
    public DbSet<TeamProfileEntity> TeamProfiles => Set<TeamProfileEntity>();
    public DbSet<TeamProfileWeightEntity> TeamProfileWeights => Set<TeamProfileWeightEntity>();
    public DbSet<SuitabilityScoreEntity> SuitabilityScores => Set<SuitabilityScoreEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlayerEntity>(entity =>
        {
            entity.ToTable("Players");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.FullName).HasMaxLength(200).IsRequired();
            entity.Property(item => item.PrimaryPosition).HasMaxLength(20).IsRequired();
            entity.Property(item => item.CurrentClub).HasMaxLength(200);
            entity.Property(item => item.MarketValueEur).HasPrecision(18, 2);
        });

        modelBuilder.Entity<PlayerMetricEntity>(entity =>
        {
            entity.ToTable("PlayerMetrics");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.MetricKey).HasMaxLength(80).IsRequired();
            entity.Property(item => item.Value).HasPrecision(18, 6);
            entity.Property(item => item.Minutes).HasPrecision(18, 2);
            entity.HasIndex(item => new { item.PlayerId, item.SeasonId, item.MetricKey });
            entity.HasOne(item => item.Player)
                .WithMany(player => player.Metrics)
                .HasForeignKey(item => item.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MetricDefinitionEntity>(entity =>
        {
            entity.ToTable("MetricDefinitions");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Key).HasMaxLength(80).IsRequired();
            entity.Property(item => item.Name).HasMaxLength(200).IsRequired();
            entity.Property(item => item.Unit).HasMaxLength(40);
            entity.Property(item => item.Description).HasMaxLength(500);
            entity.Property(item => item.Group).HasMaxLength(80);
            entity.Property(item => item.NormalizationStrategy).HasMaxLength(40).IsRequired();
            entity.Property(item => item.MinExpected).HasPrecision(18, 6);
            entity.Property(item => item.MaxExpected).HasPrecision(18, 6);
            entity.HasIndex(item => item.Key).IsUnique();
        });

        modelBuilder.Entity<TeamProfileEntity>(entity =>
        {
            entity.ToTable("TeamProfiles");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Name).HasMaxLength(200).IsRequired();
            entity.Property(item => item.Style).HasMaxLength(120).IsRequired();
            entity.Property(item => item.TargetPosition).HasMaxLength(20).IsRequired();
            entity.Property(item => item.BudgetMaxEur).HasPrecision(18, 2);
            entity.HasIndex(item => new { item.OrganizationId, item.Name }).IsUnique();
        });

        modelBuilder.Entity<TeamProfileWeightEntity>(entity =>
        {
            entity.ToTable("TeamProfileWeights");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.MetricKey).HasMaxLength(80).IsRequired();
            entity.Property(item => item.Weight).HasPrecision(18, 6);
            entity.Property(item => item.MinValue).HasPrecision(18, 6);
            entity.Property(item => item.MaxValue).HasPrecision(18, 6);
            entity.HasIndex(item => new { item.TeamProfileId, item.MetricKey }).IsUnique();
            entity.HasOne(item => item.TeamProfile)
                .WithMany(profile => profile.Weights)
                .HasForeignKey(item => item.TeamProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SuitabilityScoreEntity>(entity =>
        {
            entity.ToTable("SuitabilityScores");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Score).HasPrecision(18, 6);
            entity.Property(item => item.BreakdownJson).HasColumnType("jsonb");
            entity.HasIndex(item => new { item.TeamProfileId, item.PlayerId, item.ScoreVersion });
        });

        Seed(modelBuilder);
    }

    private static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MetricDefinitionEntity>().HasData(
            new MetricDefinitionEntity
            {
                Id = Guid.Parse("9f37f1d1-6433-4f4b-b57d-47b503c9d101"),
                Key = "xg_per90",
                Name = "xG Per 90",
                Unit = "xG",
                HigherIsBetter = true,
                Group = "Attacking",
                MinExpected = 0.05m,
                MaxExpected = 0.60m,
                Description = "Expected goals per 90 minutes"
            },
            new MetricDefinitionEntity
            {
                Id = Guid.Parse("9f37f1d1-6433-4f4b-b57d-47b503c9d102"),
                Key = "pressures_per90",
                Name = "Pressures Per 90",
                Unit = "count",
                HigherIsBetter = true,
                Group = "Defending",
                MinExpected = 5m,
                MaxExpected = 25m,
                Description = "Pressuring actions per 90 minutes"
            },
            new MetricDefinitionEntity
            {
                Id = Guid.Parse("9f37f1d1-6433-4f4b-b57d-47b503c9d103"),
                Key = "minutes",
                Name = "Minutes Played",
                Unit = "minutes",
                HigherIsBetter = true,
                Group = "Availability",
                MinExpected = 0m,
                MaxExpected = 3200m,
                Description = "Minutes played in season"
            });
    }
}

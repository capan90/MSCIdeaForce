using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Problem> Problems => Set<Problem>();
    public DbSet<Signal> Signals => Set<Signal>();
    public DbSet<Opportunity> Opportunities => Set<Opportunity>();
    public DbSet<Validation> Validations => Set<Validation>();
    public DbSet<ProblemAnalysis> ProblemAnalyses => Set<ProblemAnalysis>();
    public DbSet<Research> Researches => Set<Research>();
    public DbSet<Solution> Solutions => Set<Solution>();
    public DbSet<MVPPlan> MVPPlans => Set<MVPPlan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Problem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Description).IsRequired();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<Signal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Description).IsRequired();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<Opportunity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => !e.IsDeleted);
            entity.OwnsOne(e => e.OpportunityScore, score =>
            {
                score.Property(s => s.ProblemSeverity).HasColumnName("ProblemSeverity");
                score.Property(s => s.MarketSize).HasColumnName("MarketSize");
                score.Property(s => s.FounderFit).HasColumnName("FounderFit");
                score.Property(s => s.RiskScore).HasColumnName("RiskScore");
                score.Property(s => s.TechnicalFeasibility).HasColumnName("TechnicalFeasibility");
            });
        });

        modelBuilder.Entity<Validation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<ProblemAnalysis>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<Research>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<Solution>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<MVPPlan>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });
    }
}

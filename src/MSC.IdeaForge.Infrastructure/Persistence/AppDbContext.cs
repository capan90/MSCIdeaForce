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
    public DbSet<KnowledgeEntry> KnowledgeEntries => Set<KnowledgeEntry>();
    public DbSet<ImportedSignal> ImportedSignals => Set<ImportedSignal>();
    public DbSet<Decision> Decisions => Set<Decision>();
    public DbSet<TrendAnalysis> TrendAnalyses => Set<TrendAnalysis>();
    public DbSet<RevenueAnalysis> RevenueAnalyses => Set<RevenueAnalysis>();
    public DbSet<FounderProfile> FounderProfiles => Set<FounderProfile>();
    public DbSet<CompetitorAnalysis> CompetitorAnalyses => Set<CompetitorAnalysis>();
    public DbSet<FieldNote> FieldNotes => Set<FieldNote>();
    public DbSet<InvestorBrief> InvestorBriefs => Set<InvestorBrief>();
    public DbSet<ProblemNote> ProblemNotes => Set<ProblemNote>();
    public DbSet<BusinessCanvas> BusinessCanvases => Set<BusinessCanvas>();
    public DbSet<ChecklistItem> ChecklistItems => Set<ChecklistItem>();

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

        modelBuilder.Entity<KnowledgeEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<ImportedSignal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<Decision>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<TrendAnalysis>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TrendName).IsRequired().HasMaxLength(200);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<RevenueAnalysis>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<FounderProfile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<CompetitorAnalysis>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<FieldNote>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Content).IsRequired();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<InvestorBrief>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ExecutiveSummary).IsRequired();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<ProblemNote>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<BusinessCanvas>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<ChecklistItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });
    }
}

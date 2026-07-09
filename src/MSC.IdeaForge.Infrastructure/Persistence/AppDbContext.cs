using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Problem> Problems => Set<Problem>();
    public DbSet<Signal> Signals => Set<Signal>();

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
    }
}

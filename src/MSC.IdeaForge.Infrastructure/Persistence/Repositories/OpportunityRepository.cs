using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// Fırsat veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class OpportunityRepository : IOpportunityRepository
{
    private readonly AppDbContext _context;

    public OpportunityRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Belirtilen problem ID'sine ait tekil fırsat kaydını bulur.
    /// </summary>
    public async Task<Opportunity?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        return await _context.Opportunities
            .FirstOrDefaultAsync(o => o.ProblemId == problemId, cancellationToken);
    }

    /// <summary>
    /// Tüm fırsat değerlendirmelerini listeler.
    /// </summary>
    public async Task<IEnumerable<Opportunity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Opportunities.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Yeni bir fırsat kaydını veritabanına ekler.
    /// </summary>
    public async Task AddAsync(Opportunity opportunity, CancellationToken cancellationToken = default)
    {
        await _context.Opportunities.AddAsync(opportunity, cancellationToken);
    }

    /// <summary>
    /// Fırsat kaydını günceller.
    /// </summary>
    public Task UpdateAsync(Opportunity opportunity, CancellationToken cancellationToken = default)
    {
        _context.Opportunities.Update(opportunity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Değişiklikleri veritabanına kaydeder.
    /// </summary>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

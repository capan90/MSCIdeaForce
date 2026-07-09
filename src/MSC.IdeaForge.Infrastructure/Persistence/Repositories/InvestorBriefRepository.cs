using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// Yatırımcı briefing veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class InvestorBriefRepository : IInvestorBriefRepository
{
    private readonly AppDbContext _context;

    public InvestorBriefRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Belirtilen problem ID'sine ait yatırımcı briefing kaydını bulur.
    /// </summary>
    public async Task<InvestorBrief?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        return await _context.InvestorBriefs
            .FirstOrDefaultAsync(b => b.ProblemId == problemId, cancellationToken);
    }

    /// <summary>
    /// Yeni bir yatırımcı briefing kaydı ekler.
    /// </summary>
    public async Task AddAsync(InvestorBrief investorBrief, CancellationToken cancellationToken = default)
    {
        await _context.InvestorBriefs.AddAsync(investorBrief, cancellationToken);
    }

    /// <summary>
    /// Yatırımcı briefing kaydını günceller.
    /// </summary>
    public Task UpdateAsync(InvestorBrief investorBrief, CancellationToken cancellationToken = default)
    {
        _context.InvestorBriefs.Update(investorBrief);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Değişiklikleri kaydeder.
    /// </summary>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

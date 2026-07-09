using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// MVP Planı veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class MVPPlanRepository : IMVPPlanRepository
{
    private readonly AppDbContext _context;

    public MVPPlanRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Belirtilen problem ID'sine ait tekil MVP plan kaydını getirir.
    /// </summary>
    public async Task<MVPPlan?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        return await _context.MVPPlans
            .FirstOrDefaultAsync(m => m.ProblemId == problemId, cancellationToken);
    }

    /// <summary>
    /// Yeni bir MVP planını veritabanına ekler.
    /// </summary>
    public async Task AddAsync(MVPPlan mvpPlan, CancellationToken cancellationToken = default)
    {
        await _context.MVPPlans.AddAsync(mvpPlan, cancellationToken);
    }

    /// <summary>
    /// MVP plan kaydını günceller.
    /// </summary>
    public Task UpdateAsync(MVPPlan mvpPlan, CancellationToken cancellationToken = default)
    {
        _context.MVPPlans.Update(mvpPlan);
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

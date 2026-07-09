using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// Aksiyon planı veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class ActionPlanRepository : IActionPlanRepository
{
    private readonly AppDbContext _context;

    public ActionPlanRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Belirtilen problem ID'sine ait aksiyon planı kaydını bulur.
    /// </summary>
    public async Task<ActionPlan?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        return await _context.ActionPlans
            .FirstOrDefaultAsync(a => a.ProblemId == problemId, cancellationToken);
    }

    /// <summary>
    /// Yeni bir aksiyon planı kaydı ekler.
    /// </summary>
    public async Task AddAsync(ActionPlan actionPlan, CancellationToken cancellationToken = default)
    {
        await _context.ActionPlans.AddAsync(actionPlan, cancellationToken);
    }

    /// <summary>
    /// Aksiyon planı kaydını günceller.
    /// </summary>
    public Task UpdateAsync(ActionPlan actionPlan, CancellationToken cancellationToken = default)
    {
        _context.ActionPlans.Update(actionPlan);
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

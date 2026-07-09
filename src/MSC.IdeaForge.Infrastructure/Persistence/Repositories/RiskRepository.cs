using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// Risk veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class RiskRepository : IRiskRepository
{
    private readonly AppDbContext _context;

    public RiskRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Belirtilen probleme ait riskleri (skora göre azalan) getirir.
    /// </summary>
    public async Task<List<RiskItem>> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        return await _context.RiskItems
            .Where(r => r.ProblemId == problemId)
            .OrderByDescending(r => r.RiskScore)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Yeni bir risk kaydı ekler.
    /// </summary>
    public async Task AddAsync(RiskItem risk, CancellationToken cancellationToken = default)
    {
        await _context.RiskItems.AddAsync(risk, cancellationToken);
    }

    /// <summary>
    /// Risk kaydını günceller.
    /// </summary>
    public Task UpdateAsync(RiskItem risk, CancellationToken cancellationToken = default)
    {
        _context.RiskItems.Update(risk);
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

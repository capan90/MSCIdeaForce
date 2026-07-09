using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// Gelir analizi veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class RevenueAnalysisRepository : IRevenueAnalysisRepository
{
    private readonly AppDbContext _context;

    public RevenueAnalysisRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Belirtilen problem ID'sine ait gelir analizi kaydını bulur.
    /// </summary>
    public async Task<RevenueAnalysis?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        return await _context.RevenueAnalyses
            .FirstOrDefaultAsync(r => r.ProblemId == problemId, cancellationToken);
    }

    /// <summary>
    /// Yeni bir gelir analizi kaydı ekler.
    /// </summary>
    public async Task AddAsync(RevenueAnalysis revenueAnalysis, CancellationToken cancellationToken = default)
    {
        await _context.RevenueAnalyses.AddAsync(revenueAnalysis, cancellationToken);
    }

    /// <summary>
    /// Gelir analizi kaydını günceller.
    /// </summary>
    public Task UpdateAsync(RevenueAnalysis revenueAnalysis, CancellationToken cancellationToken = default)
    {
        _context.RevenueAnalyses.Update(revenueAnalysis);
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

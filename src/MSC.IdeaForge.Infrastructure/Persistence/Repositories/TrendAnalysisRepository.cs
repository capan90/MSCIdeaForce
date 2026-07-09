using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// Trend analizi veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class TrendAnalysisRepository : ITrendAnalysisRepository
{
    private readonly AppDbContext _context;

    public TrendAnalysisRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Belirtilen trend başlığına ait en güncel analiz kaydını bulur.
    /// </summary>
    public async Task<TrendAnalysis?> GetByTrendNameAsync(string trendName, CancellationToken cancellationToken = default)
    {
        return await _context.TrendAnalyses
            .OrderByDescending(t => t.AnalyzedAt)
            .FirstOrDefaultAsync(t => t.TrendName == trendName, cancellationToken);
    }

    /// <summary>
    /// Kaydedilmiş tüm trend analizlerini getirir.
    /// </summary>
    public async Task<List<TrendAnalysis>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TrendAnalyses
            .OrderByDescending(t => t.AnalyzedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Yeni bir trend analizi kaydı ekler.
    /// </summary>
    public async Task AddAsync(TrendAnalysis trendAnalysis, CancellationToken cancellationToken = default)
    {
        await _context.TrendAnalyses.AddAsync(trendAnalysis, cancellationToken);
    }

    /// <summary>
    /// Trend analizi kaydını günceller.
    /// </summary>
    public Task UpdateAsync(TrendAnalysis trendAnalysis, CancellationToken cancellationToken = default)
    {
        _context.TrendAnalyses.Update(trendAnalysis);
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

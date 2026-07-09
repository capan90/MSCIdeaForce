using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// Rakip analizi veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class CompetitorAnalysisRepository : ICompetitorAnalysisRepository
{
    private readonly AppDbContext _context;

    public CompetitorAnalysisRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Belirtilen problem ID'sine ait rakip analizi kaydını bulur.
    /// </summary>
    public async Task<CompetitorAnalysis?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        return await _context.CompetitorAnalyses
            .FirstOrDefaultAsync(c => c.ProblemId == problemId, cancellationToken);
    }

    /// <summary>
    /// Yeni bir rakip analizi kaydı ekler.
    /// </summary>
    public async Task AddAsync(CompetitorAnalysis competitorAnalysis, CancellationToken cancellationToken = default)
    {
        await _context.CompetitorAnalyses.AddAsync(competitorAnalysis, cancellationToken);
    }

    /// <summary>
    /// Rakip analizi kaydını günceller.
    /// </summary>
    public Task UpdateAsync(CompetitorAnalysis competitorAnalysis, CancellationToken cancellationToken = default)
    {
        _context.CompetitorAnalyses.Update(competitorAnalysis);
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

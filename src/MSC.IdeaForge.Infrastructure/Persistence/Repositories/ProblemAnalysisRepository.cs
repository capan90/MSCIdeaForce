using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// Yapay zeka analiz veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class ProblemAnalysisRepository : IProblemAnalysisRepository
{
    private readonly AppDbContext _context;

    public ProblemAnalysisRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Belirtilen problem ID'sine ait yapay zeka analiz kaydını bulur.
    /// </summary>
    public async Task<ProblemAnalysis?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        return await _context.ProblemAnalyses
            .FirstOrDefaultAsync(p => p.ProblemId == problemId, cancellationToken);
    }

    /// <summary>
    /// Yeni bir yapay zeka analiz kaydı ekler.
    /// </summary>
    public async Task AddAsync(ProblemAnalysis problemAnalysis, CancellationToken cancellationToken = default)
    {
        await _context.ProblemAnalyses.AddAsync(problemAnalysis, cancellationToken);
    }

    /// <summary>
    /// Yapay zeka analiz kaydını günceller.
    /// </summary>
    public Task UpdateAsync(ProblemAnalysis problemAnalysis, CancellationToken cancellationToken = default)
    {
        _context.ProblemAnalyses.Update(problemAnalysis);
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

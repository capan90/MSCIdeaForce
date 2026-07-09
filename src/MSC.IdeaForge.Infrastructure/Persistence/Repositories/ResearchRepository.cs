using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// Araştırma veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class ResearchRepository : IResearchRepository
{
    private readonly AppDbContext _context;

    public ResearchRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Belirtilen problem ID'sine ait araştırma kaydını bulur.
    /// </summary>
    public async Task<Research?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        return await _context.Researches
            .FirstOrDefaultAsync(r => r.ProblemId == problemId, cancellationToken);
    }

    /// <summary>
    /// Yeni bir araştırma kaydı ekler.
    /// </summary>
    public async Task AddAsync(Research research, CancellationToken cancellationToken = default)
    {
        await _context.Researches.AddAsync(research, cancellationToken);
    }

    /// <summary>
    /// Araştırma kaydını günceller.
    /// </summary>
    public Task UpdateAsync(Research research, CancellationToken cancellationToken = default)
    {
        _context.Researches.Update(research);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Değişiklikleri veritabanına yansıtır.
    /// </summary>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

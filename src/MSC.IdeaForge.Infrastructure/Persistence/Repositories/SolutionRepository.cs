using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// Çözüm veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class SolutionRepository : ISolutionRepository
{
    private readonly AppDbContext _context;

    public SolutionRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Belirtilen probleme ait tüm çözüm önerilerini listeler.
    /// </summary>
    public async Task<IEnumerable<Solution>> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        return await _context.Solutions
            .Where(s => s.ProblemId == problemId)
            .OrderByDescending(s => s.IsRecommended)
            .ThenBy(s => s.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Yeni bir çözüm önerisini veritabanına ekler.
    /// </summary>
    public async Task AddAsync(Solution solution, CancellationToken cancellationToken = default)
    {
        await _context.Solutions.AddAsync(solution, cancellationToken);
    }

    /// <summary>
    /// Çözüm önerisini günceller.
    /// </summary>
    public Task UpdateAsync(Solution solution, CancellationToken cancellationToken = default)
    {
        _context.Solutions.Update(solution);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Çözüm önerisini (soft-delete ile) siler.
    /// </summary>
    public Task DeleteAsync(Solution solution, CancellationToken cancellationToken = default)
    {
        solution.MarkAsDeleted(); // BaseEntity üzerindeki MarkAsDeleted metodunu çağırıyoruz.
        _context.Solutions.Update(solution);
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

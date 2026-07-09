using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// Sinyal veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class SignalRepository : ISignalRepository
{
    private readonly AppDbContext _context;

    public SignalRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Belirtilen probleme ait tüm sinyalleri azalan oluşturulma tarihine göre listeler.
    /// </summary>
    public async Task<IEnumerable<Signal>> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        return await _context.Signals
            .Where(s => s.ProblemId == problemId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Toplam sinyal sayısını döner.
    /// </summary>
    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Signals.CountAsync(cancellationToken);
    }

    /// <summary>
    /// Yeni bir sinyali veritabanı bağlamına ekler.
    /// </summary>
    public async Task AddAsync(Signal signal, CancellationToken cancellationToken = default)
    {
        await _context.Signals.AddAsync(signal, cancellationToken);
    }

    /// <summary>
    /// Değişiklikleri veritabanına kaydeder.
    /// </summary>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

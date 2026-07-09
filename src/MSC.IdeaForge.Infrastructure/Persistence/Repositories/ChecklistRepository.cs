using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// Kontrol listesi veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class ChecklistRepository : IChecklistRepository
{
    private readonly AppDbContext _context;

    public ChecklistRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Belirtilen probleme ait kontrol listesi öğelerini (önceliğe göre) getirir.
    /// </summary>
    public async Task<List<ChecklistItem>> GetByProblemIdAsync(Guid? problemId, CancellationToken cancellationToken = default)
    {
        return await _context.ChecklistItems
            .Where(c => c.ProblemId == problemId)
            .OrderByDescending(c => c.Priority)
            .ThenBy(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Belirtilen ID'ye ait öğeyi bulur.
    /// </summary>
    public async Task<ChecklistItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ChecklistItems.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    /// <summary>
    /// Yeni bir kontrol listesi öğesi ekler.
    /// </summary>
    public async Task AddAsync(ChecklistItem item, CancellationToken cancellationToken = default)
    {
        await _context.ChecklistItems.AddAsync(item, cancellationToken);
    }

    /// <summary>
    /// Öğeyi günceller.
    /// </summary>
    public Task UpdateAsync(ChecklistItem item, CancellationToken cancellationToken = default)
    {
        _context.ChecklistItems.Update(item);
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

using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// Problem notu veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class ProblemNoteRepository : IProblemNoteRepository
{
    private readonly AppDbContext _context;

    public ProblemNoteRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Belirtilen probleme ait notları (yeniden eskiye) getirir.
    /// </summary>
    public async Task<List<ProblemNote>> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        return await _context.ProblemNotes
            .Where(n => n.ProblemId == problemId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Belirtilen ID'ye ait notu bulur.
    /// </summary>
    public async Task<ProblemNote?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ProblemNotes.FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
    }

    /// <summary>
    /// Yeni bir not ekler.
    /// </summary>
    public async Task AddAsync(ProblemNote note, CancellationToken cancellationToken = default)
    {
        await _context.ProblemNotes.AddAsync(note, cancellationToken);
    }

    /// <summary>
    /// Not kaydını günceller.
    /// </summary>
    public Task UpdateAsync(ProblemNote note, CancellationToken cancellationToken = default)
    {
        _context.ProblemNotes.Update(note);
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

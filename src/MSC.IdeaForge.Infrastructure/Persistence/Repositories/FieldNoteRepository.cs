using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// Saha notu veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class FieldNoteRepository : IFieldNoteRepository
{
    private readonly AppDbContext _context;

    public FieldNoteRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Tüm saha notlarını gözlem tarihine göre (yeniden eskiye) getirir.
    /// </summary>
    public async Task<List<FieldNote>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.FieldNotes
            .OrderByDescending(f => f.ObservationDate)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Yeni bir saha notu ekler.
    /// </summary>
    public async Task AddAsync(FieldNote fieldNote, CancellationToken cancellationToken = default)
    {
        await _context.FieldNotes.AddAsync(fieldNote, cancellationToken);
    }

    /// <summary>
    /// Değişiklikleri kaydeder.
    /// </summary>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

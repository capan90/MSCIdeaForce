using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// Kurucu profili veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class FounderProfileRepository : IFounderProfileRepository
{
    private readonly AppDbContext _context;

    public FounderProfileRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// En güncel kurucu profilini getirir.
    /// </summary>
    public async Task<FounderProfile?> GetAsync(CancellationToken cancellationToken = default)
    {
        return await _context.FounderProfiles
            .OrderByDescending(f => f.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Yeni bir kurucu profili ekler.
    /// </summary>
    public async Task AddAsync(FounderProfile founderProfile, CancellationToken cancellationToken = default)
    {
        await _context.FounderProfiles.AddAsync(founderProfile, cancellationToken);
    }

    /// <summary>
    /// Kurucu profilini günceller.
    /// </summary>
    public Task UpdateAsync(FounderProfile founderProfile, CancellationToken cancellationToken = default)
    {
        _context.FounderProfiles.Update(founderProfile);
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

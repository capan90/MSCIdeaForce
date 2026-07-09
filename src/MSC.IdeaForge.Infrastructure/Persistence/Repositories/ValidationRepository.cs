using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// Doğrulama veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class ValidationRepository : IValidationRepository
{
    private readonly AppDbContext _context;

    public ValidationRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Belirtilen problem ID'sine ait doğrulama kaydını getirir.
    /// </summary>
    public async Task<Validation?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        return await _context.Validations
            .FirstOrDefaultAsync(v => v.ProblemId == problemId, cancellationToken);
    }

    /// <summary>
    /// Yeni bir doğrulama kaydı ekler.
    /// </summary>
    public async Task AddAsync(Validation validation, CancellationToken cancellationToken = default)
    {
        await _context.Validations.AddAsync(validation, cancellationToken);
    }

    /// <summary>
    /// Doğrulama kaydını günceller.
    /// </summary>
    public Task UpdateAsync(Validation validation, CancellationToken cancellationToken = default)
    {
        _context.Validations.Update(validation);
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

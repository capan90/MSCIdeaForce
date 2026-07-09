using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// Business Model Canvas veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class BusinessCanvasRepository : IBusinessCanvasRepository
{
    private readonly AppDbContext _context;

    public BusinessCanvasRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Belirtilen problem ID'sine ait canvas kaydını bulur.
    /// </summary>
    public async Task<BusinessCanvas?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        return await _context.BusinessCanvases
            .FirstOrDefaultAsync(c => c.ProblemId == problemId, cancellationToken);
    }

    /// <summary>
    /// Yeni bir canvas kaydı ekler.
    /// </summary>
    public async Task AddAsync(BusinessCanvas canvas, CancellationToken cancellationToken = default)
    {
        await _context.BusinessCanvases.AddAsync(canvas, cancellationToken);
    }

    /// <summary>
    /// Canvas kaydını günceller.
    /// </summary>
    public Task UpdateAsync(BusinessCanvas canvas, CancellationToken cancellationToken = default)
    {
        _context.BusinessCanvases.Update(canvas);
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

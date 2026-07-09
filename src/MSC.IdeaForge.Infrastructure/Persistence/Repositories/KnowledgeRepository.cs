using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Enums;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// Bilgi bankası veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class KnowledgeRepository : IKnowledgeRepository
{
    private readonly AppDbContext _context;

    public KnowledgeRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Tüm bilgi bankası girdilerini eklenme tarihine göre azalan şekilde getirir.
    /// </summary>
    public async Task<IEnumerable<KnowledgeEntry>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.KnowledgeEntries
            .OrderByDescending(k => k.LearnedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Belirtilen kategoriye ait girdileri getirir.
    /// </summary>
    public async Task<IEnumerable<KnowledgeEntry>> GetByCategoryAsync(KnowledgeCategory category, CancellationToken cancellationToken = default)
    {
        return await _context.KnowledgeEntries
            .Where(k => k.Category == category)
            .OrderByDescending(k => k.LearnedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Belirtilen probleme ait girdileri getirir.
    /// </summary>
    public async Task<IEnumerable<KnowledgeEntry>> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        return await _context.KnowledgeEntries
            .Where(k => k.ProblemId == problemId)
            .OrderByDescending(k => k.LearnedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Yeni girdiyi veritabanına ekler.
    /// </summary>
    public async Task AddAsync(KnowledgeEntry entry, CancellationToken cancellationToken = default)
    {
        await _context.KnowledgeEntries.AddAsync(entry, cancellationToken);
    }

    /// <summary>
    /// Girdiyi günceller.
    /// </summary>
    public Task UpdateAsync(KnowledgeEntry entry, CancellationToken cancellationToken = default)
    {
        _context.KnowledgeEntries.Update(entry);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Girdiyi (soft-delete ile) siler.
    /// </summary>
    public Task DeleteAsync(KnowledgeEntry entry, CancellationToken cancellationToken = default)
    {
        entry.MarkAsDeleted();
        _context.KnowledgeEntries.Update(entry);
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

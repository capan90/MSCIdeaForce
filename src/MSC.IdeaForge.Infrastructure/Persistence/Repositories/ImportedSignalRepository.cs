using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

/// <summary>
/// İçe aktarılan ham sinyallerin veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class ImportedSignalRepository : IImportedSignalRepository
{
    private readonly AppDbContext _context;

    public ImportedSignalRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Henüz işlenmemiş (problemle ilişkilendirilmemiş) sinyalleri içe aktarılma tarihine göre azalan getirir.
    /// </summary>
    public async Task<IEnumerable<ImportedSignal>> GetUnprocessedAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ImportedSignals
            .Where(s => !s.IsProcessed)
            .OrderByDescending(s => s.ImportedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Tüm içe aktarılmış sinyalleri getirir.
    /// </summary>
    public async Task<IEnumerable<ImportedSignal>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ImportedSignals
            .OrderByDescending(s => s.ImportedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Yeni sinyali veritabanına ekler.
    /// </summary>
    public async Task AddAsync(ImportedSignal signal, CancellationToken cancellationToken = default)
    {
        await _context.ImportedSignals.AddAsync(signal, cancellationToken);
    }

    /// <summary>
    /// Sinyali günceller.
    /// </summary>
    public Task UpdateAsync(ImportedSignal signal, CancellationToken cancellationToken = default)
    {
        _context.ImportedSignals.Update(signal);
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

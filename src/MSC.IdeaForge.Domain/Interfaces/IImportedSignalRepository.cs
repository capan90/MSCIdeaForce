using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// İçe aktarılan ham sinyallerin veritabanı erişim arayüzü.
/// </summary>
public interface IImportedSignalRepository
{
    /// <summary>
    /// Henüz işlenmemiş (herhangi bir probleme bağlanmamış) sinyalleri getirir.
    /// </summary>
    Task<IEnumerable<ImportedSignal>> GetUnprocessedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Tüm içe aktarılmış sinyalleri getirir.
    /// </summary>
    Task<IEnumerable<ImportedSignal>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir içe aktarılmış sinyal kaydı ekler.
    /// </summary>
    Task AddAsync(ImportedSignal signal, CancellationToken cancellationToken = default);

    /// <summary>
    /// İçe aktarılmış sinyal kaydını günceller.
    /// </summary>
    Task UpdateAsync(ImportedSignal signal, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri veritabanına kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

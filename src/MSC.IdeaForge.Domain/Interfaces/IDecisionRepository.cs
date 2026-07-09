using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Alınan kararların veritabanı erişim arayüzü.
/// </summary>
public interface IDecisionRepository
{
    /// <summary>
    /// Tüm karar günlüklerini getirir.
    /// </summary>
    Task<IEnumerable<Decision>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirli bir probleme ait alınan kararları getirir.
    /// </summary>
    Task<IEnumerable<Decision>> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir karar kaydı ekler.
    /// </summary>
    Task AddAsync(Decision decision, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri veritabanına kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

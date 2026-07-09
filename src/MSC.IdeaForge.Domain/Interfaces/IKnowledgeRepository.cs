using System;
using System.Collections.Generic;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Enums;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Bilgi bankası verilerine erişim arayüzü.
/// </summary>
public interface IKnowledgeRepository
{
    /// <summary>
    /// Tüm aktif bilgi bankası girdilerini getirir.
    /// </summary>
    Task<IEnumerable<KnowledgeEntry>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen kategoriye ait bilgi bankası girdilerini getirir.
    /// </summary>
    Task<IEnumerable<KnowledgeEntry>> GetByCategoryAsync(KnowledgeCategory category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen problem ID'si ile ilişkilendirilmiş bilgi bankası girdilerini getirir.
    /// </summary>
    Task<IEnumerable<KnowledgeEntry>> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir bilgi bankası girdisi ekler.
    /// </summary>
    Task AddAsync(KnowledgeEntry entry, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bilgi bankası girdisini günceller.
    /// </summary>
    Task UpdateAsync(KnowledgeEntry entry, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bilgi bankası girdisini siler (soft delete).
    /// </summary>
    Task DeleteAsync(KnowledgeEntry entry, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri veritabanına kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

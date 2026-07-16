using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

public interface IAIProviderSettingRepository
{
    Task<AIProviderSetting?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AIProviderSetting>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AIProviderSetting?> GetDefaultAsync(CancellationToken cancellationToken = default);
    Task AddAsync(AIProviderSetting setting, CancellationToken cancellationToken = default);
    Task UpdateAsync(AIProviderSetting setting, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

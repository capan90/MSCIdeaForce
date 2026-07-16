using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Enums;

namespace MSC.IdeaForge.Domain.Interfaces;

public interface IAIPromptRepository
{
    Task<AIPrompt?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AIPrompt>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AIPrompt?> GetActiveByTypeAsync(PromptType promptType, CancellationToken cancellationToken = default);
    Task<IEnumerable<AIPrompt>> GetHistoryByTypeAsync(PromptType promptType, CancellationToken cancellationToken = default);
    Task AddAsync(AIPrompt prompt, CancellationToken cancellationToken = default);
    Task UpdateAsync(AIPrompt prompt, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

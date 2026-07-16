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

public class AIPromptRepository : IAIPromptRepository
{
    private readonly AppDbContext _context;

    public AIPromptRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AIPrompt?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.AIPrompts.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<IEnumerable<AIPrompt>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.AIPrompts.OrderByDescending(p => p.CreatedAt).ToListAsync(cancellationToken);

    public async Task<AIPrompt?> GetActiveByTypeAsync(PromptType promptType, CancellationToken cancellationToken = default)
        => await _context.AIPrompts.FirstOrDefaultAsync(p => p.PromptType == promptType && p.IsActive, cancellationToken);

    public async Task<IEnumerable<AIPrompt>> GetHistoryByTypeAsync(PromptType promptType, CancellationToken cancellationToken = default)
        => await _context.AIPrompts
            .Where(p => p.PromptType == promptType)
            .OrderByDescending(p => p.Version)
            .Take(3)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(AIPrompt prompt, CancellationToken cancellationToken = default)
        => await _context.AIPrompts.AddAsync(prompt, cancellationToken);

    public Task UpdateAsync(AIPrompt prompt, CancellationToken cancellationToken = default)
    {
        _context.AIPrompts.Update(prompt);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}

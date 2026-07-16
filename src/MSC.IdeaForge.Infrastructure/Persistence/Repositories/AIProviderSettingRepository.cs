using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

public class AIProviderSettingRepository : IAIProviderSettingRepository
{
    private readonly AppDbContext _context;

    public AIProviderSettingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AIProviderSetting?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.AIProviderSettings.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task<IEnumerable<AIProviderSetting>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.AIProviderSettings.OrderBy(s => s.ProviderName).ToListAsync(cancellationToken);

    public async Task<AIProviderSetting?> GetDefaultAsync(CancellationToken cancellationToken = default)
        => await _context.AIProviderSettings.FirstOrDefaultAsync(s => s.IsDefault && s.IsActive, cancellationToken);

    public async Task AddAsync(AIProviderSetting setting, CancellationToken cancellationToken = default)
        => await _context.AIProviderSettings.AddAsync(setting, cancellationToken);

    public Task UpdateAsync(AIProviderSetting setting, CancellationToken cancellationToken = default)
    {
        _context.AIProviderSettings.Update(setting);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}

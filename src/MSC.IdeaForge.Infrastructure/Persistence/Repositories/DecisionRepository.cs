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
/// Alınan kararların veritabanı işlemlerini gerçekleştiren depo sınıfı.
/// </summary>
public class DecisionRepository : IDecisionRepository
{
    private readonly AppDbContext _context;

    public DecisionRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Tüm karar günlüklerini karar tarihine göre azalan sırada getirir.
    /// </summary>
    public async Task<IEnumerable<Decision>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Decisions
            .OrderByDescending(d => d.DecisionDate)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Belirli bir probleme ait kararları karar tarihine göre azalan sırada getirir.
    /// </summary>
    public async Task<IEnumerable<Decision>> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        return await _context.Decisions
            .Where(d => d.ProblemId == problemId)
            .OrderByDescending(d => d.DecisionDate)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Yeni bir karar kaydını veritabanına ekler.
    /// </summary>
    public async Task AddAsync(Decision decision, CancellationToken cancellationToken = default)
    {
        await _context.Decisions.AddAsync(decision, cancellationToken);
    }

    /// <summary>
    /// Değişiklikleri kaydeder.
    /// </summary>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

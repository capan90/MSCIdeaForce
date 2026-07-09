using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

public class RSSFeedRepository : IRSSFeedRepository
{
    private readonly AppDbContext _context;

    public RSSFeedRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RSSFeed?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.RSSFeeds.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

    public async Task<IEnumerable<RSSFeed>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.RSSFeeds.OrderByDescending(f => f.CreatedAt).ToListAsync(cancellationToken);

    public async Task AddAsync(RSSFeed feed, CancellationToken cancellationToken = default)
        => await _context.RSSFeeds.AddAsync(feed, cancellationToken);

    public Task UpdateAsync(RSSFeed feed, CancellationToken cancellationToken = default)
    {
        _context.RSSFeeds.Update(feed);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}

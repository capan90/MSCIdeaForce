using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

public interface IRSSFeedRepository
{
    Task<RSSFeed?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<RSSFeed>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(RSSFeed feed, CancellationToken cancellationToken = default);
    Task UpdateAsync(RSSFeed feed, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

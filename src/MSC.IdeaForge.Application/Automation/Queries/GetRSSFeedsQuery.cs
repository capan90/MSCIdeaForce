using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Application.Automation.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Automation.Queries;

public class GetRSSFeedsQuery
{
}

public class GetRSSFeedsHandler
{
    private readonly IRSSFeedRepository _repository;

    public GetRSSFeedsHandler(IRSSFeedRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RSSFeedDto>> HandleAsync(GetRSSFeedsQuery query, CancellationToken cancellationToken = default)
    {
        var feeds = await _repository.GetAllAsync(cancellationToken);
        return feeds.Select(f => new RSSFeedDto
        {
            Id = f.Id,
            Url = f.Url,
            Name = f.Name,
            IsActive = f.IsActive,
            LastScannedAt = f.LastScannedAt,
            LastSignalCount = f.LastSignalCount
        });
    }
}

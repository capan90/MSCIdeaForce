using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Application.AI.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.AI.Queries;

public class GetAIProviderSettingsQuery
{
}

public class GetAIProviderSettingsHandler
{
    private readonly IAIProviderSettingRepository _repository;

    public GetAIProviderSettingsHandler(IAIProviderSettingRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AIProviderSettingDto>> HandleAsync(GetAIProviderSettingsQuery query, CancellationToken cancellationToken = default)
    {
        var settings = await _repository.GetAllAsync(cancellationToken);
        return settings.Select(s => new AIProviderSettingDto
        {
            Id = s.Id,
            ProviderName = s.ProviderName,
            ApiKey = s.ApiKey,
            Model = s.Model,
            IsActive = s.IsActive,
            IsDefault = s.IsDefault
        });
    }
}

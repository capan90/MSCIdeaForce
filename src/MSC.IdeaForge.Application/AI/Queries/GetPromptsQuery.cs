using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Application.AI.DTOs;
using MSC.IdeaForge.Domain.Enums;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.AI.Queries;

public class GetPromptsQuery
{
    public PromptType? PromptType { get; set; }
}

public class GetPromptsHandler
{
    private readonly IAIPromptRepository _repository;

    public GetPromptsHandler(IAIPromptRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PromptDto>> HandleAsync(GetPromptsQuery query, CancellationToken cancellationToken = default)
    {
        var prompts = await _repository.GetAllAsync(cancellationToken);
        if (query.PromptType.HasValue)
        {
            prompts = prompts.Where(p => p.PromptType == query.PromptType.Value);
        }

        return prompts.Select(p => new PromptDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            PromptType = p.PromptType,
            Content = p.Content,
            IsActive = p.IsActive,
            Version = p.Version,
            CreatedAt = p.CreatedAt
        });
    }
}

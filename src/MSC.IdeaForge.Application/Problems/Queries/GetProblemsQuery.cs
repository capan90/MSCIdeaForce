using MSC.IdeaForge.Application.Problems.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Problems.Queries;

public class GetProblemsHandler
{
    private readonly IProblemRepository _repository;

    public GetProblemsHandler(IProblemRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProblemDto>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var problems = await _repository.GetAllAsync(cancellationToken);
        return problems.Select(p => new ProblemDto
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            Status = p.Status.ToString(),
            Priority = p.Priority.ToString(),
            Sector = p.Sector,
            Tags = p.Tags,
            Source = p.Source,
            CreatedAt = p.CreatedAt,
            IdeaStatus = (p.IdeaStatus ?? Domain.Enums.IdeaStatus.Raw).ToString(),
            IdeaStatusHistory = p.IdeaStatusHistory
        });
    }
}

using MSC.IdeaForge.Application.Solutions.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Solutions.Queries;

/// <summary>
/// Probleme ait çözüm önerilerini listelemek için kullanılan sorgu işleyicisi.
/// </summary>
public class GetSolutionsByProblemHandler
{
    private readonly ISolutionRepository _repository;

    public GetSolutionsByProblemHandler(ISolutionRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Probleme ait tüm çözümleri DTO listesi olarak döner.
    /// </summary>
    public async Task<IEnumerable<SolutionDto>> HandleAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        var solutions = await _repository.GetByProblemIdAsync(problemId, cancellationToken);

        return System.Linq.Enumerable.Select(solutions, s => new SolutionDto
        {
            Id = s.Id,
            ProblemId = s.ProblemId,
            SolutionType = s.SolutionType.ToString(),
            Summary = s.Summary,
            Complexity = s.Complexity.ToString(),
            EstimatedDurationMonths = s.EstimatedDurationMonths,
            EstimatedCost = s.EstimatedCost,
            Pros = s.Pros,
            Cons = s.Cons,
            IsRecommended = s.IsRecommended,
            CreatedAt = s.CreatedAt
        });
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Application.Decisions.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Decisions.Queries;

/// <summary>
/// Belirli bir probleme ait kararları getiren sorgu işleyicisi.
/// </summary>
public class GetDecisionsByProblemHandler
{
    private readonly IDecisionRepository _repository;
    private readonly IProblemRepository _problemRepository;

    public GetDecisionsByProblemHandler(IDecisionRepository repository, IProblemRepository problemRepository)
    {
        _repository = repository;
        _problemRepository = problemRepository;
    }

    /// <summary>
    /// Problem ID'sine göre kararları listeler.
    /// </summary>
    public async Task<IEnumerable<DecisionDto>> HandleAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        var decisions = await _repository.GetByProblemIdAsync(problemId, cancellationToken);
        var problem = await _problemRepository.GetByIdAsync(problemId, cancellationToken);
        var problemTitle = problem?.Title ?? "Bilinmeyen Problem";

        return decisions.Select(d => new DecisionDto
        {
            Id = d.Id,
            ProblemId = d.ProblemId,
            ProblemTitle = problemTitle,
            DecisionType = d.DecisionType.ToString(),
            Reason = d.Reason,
            DecisionDate = d.DecisionDate
        });
    }
}

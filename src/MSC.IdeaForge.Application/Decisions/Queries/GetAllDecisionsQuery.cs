using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Application.Decisions.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Decisions.Queries;

/// <summary>
/// Tüm kararları listeleyen sorgu işleyicisi.
/// </summary>
public class GetAllDecisionsHandler
{
    private readonly IDecisionRepository _repository;
    private readonly IProblemRepository _problemRepository;

    public GetAllDecisionsHandler(IDecisionRepository repository, IProblemRepository problemRepository)
    {
        _repository = repository;
        _problemRepository = problemRepository;
    }

    /// <summary>
    /// Veritabanındaki tüm kararları problem adlarıyla birlikte getirir.
    /// </summary>
    public async Task<IEnumerable<DecisionDto>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var decisions = await _repository.GetAllAsync(cancellationToken);
        var problems = await _problemRepository.GetAllAsync(cancellationToken);
        var problemMap = problems.ToDictionary(p => p.Id, p => p.Title);

        return decisions.Select(d => new DecisionDto
        {
            Id = d.Id,
            ProblemId = d.ProblemId,
            ProblemTitle = problemMap.TryGetValue(d.ProblemId, out var title) ? title : "Bilinmeyen Problem",
            DecisionType = d.DecisionType.ToString(),
            Reason = d.Reason,
            DecisionDate = d.DecisionDate
        });
    }
}

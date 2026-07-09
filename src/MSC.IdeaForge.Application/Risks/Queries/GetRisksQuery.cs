using MSC.IdeaForge.Application.Risks.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Risks.Queries;

/// <summary>
/// Belirtilen probleme ait riskleri getiren sorgu işleyicisi.
/// </summary>
public class GetRisksHandler
{
    private readonly IRiskRepository _repository;

    public GetRisksHandler(IRiskRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Probleme ait riskleri DTO listesi olarak döner.
    /// </summary>
    public async Task<List<RiskDto>> HandleAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        var risks = await _repository.GetByProblemIdAsync(problemId, cancellationToken);
        return risks.Select(r => new RiskDto
        {
            Id = r.Id,
            ProblemId = r.ProblemId,
            RiskName = r.RiskName,
            Category = r.Category,
            Probability = r.Probability,
            Impact = r.Impact,
            RiskScore = r.RiskScore,
            MitigationStrategy = r.MitigationStrategy,
            Owner = r.Owner,
            Status = r.Status
        }).ToList();
    }
}

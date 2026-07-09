using MSC.IdeaForge.Application.Opportunities.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Opportunities.Queries;

/// <summary>
/// Belirtilen probleme ait fırsat değerlendirme verilerini getiren sorgu işleyicisi.
/// </summary>
public class GetOpportunityByProblemHandler
{
    private readonly IOpportunityRepository _repository;

    public GetOpportunityByProblemHandler(IOpportunityRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Probleme ait fırsat kaydını DTO formatında döner.
    /// </summary>
    public async Task<OpportunityDto?> HandleAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        var opportunity = await _repository.GetByProblemIdAsync(problemId, cancellationToken);
        if (opportunity is null)
        {
            return null;
        }

        return new OpportunityDto
        {
            Id = opportunity.Id,
            ProblemId = opportunity.ProblemId,
            ProblemSeverity = opportunity.OpportunityScore.ProblemSeverity,
            MarketSize = opportunity.OpportunityScore.MarketSize,
            FounderFit = opportunity.OpportunityScore.FounderFit,
            RiskScore = opportunity.OpportunityScore.RiskScore,
            TechnicalFeasibility = opportunity.OpportunityScore.TechnicalFeasibility,
            TotalScore = opportunity.OpportunityScore.TotalScore,
            Notes = opportunity.Notes,
            Status = opportunity.Status.ToString(),
            CreatedAt = opportunity.CreatedAt
        };
    }
}

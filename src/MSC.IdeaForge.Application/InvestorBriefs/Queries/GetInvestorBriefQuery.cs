using MSC.IdeaForge.Application.InvestorBriefs.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.InvestorBriefs.Queries;

/// <summary>
/// Belirtilen probleme ait yatırımcı briefing verisini getiren sorgu işleyicisi.
/// </summary>
public class GetInvestorBriefHandler
{
    private readonly IInvestorBriefRepository _repository;

    public GetInvestorBriefHandler(IInvestorBriefRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Probleme ait yatırımcı briefing kaydını DTO formatında döner. Kayıt yoksa null döner.
    /// </summary>
    public async Task<InvestorBriefDto?> HandleAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        var brief = await _repository.GetByProblemIdAsync(problemId, cancellationToken);
        if (brief is null)
        {
            return null;
        }

        return new InvestorBriefDto
        {
            Id = brief.Id,
            ProblemId = brief.ProblemId,
            ExecutiveSummary = brief.ExecutiveSummary,
            ProblemStatement = brief.ProblemStatement,
            MarketOpportunity = brief.MarketOpportunity,
            Solution = brief.Solution,
            BusinessModel = brief.BusinessModel,
            CompetitiveAdvantage = brief.CompetitiveAdvantage,
            AskAmount = brief.AskAmount,
            UseOfFunds = brief.UseOfFunds,
            Traction = brief.Traction,
            GeneratedAt = brief.GeneratedAt
        };
    }
}

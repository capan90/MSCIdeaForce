using MSC.IdeaForge.Application.Researches.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Researches.Queries;

/// <summary>
/// Probleme ait araştırma verilerini çeken sorgu işleyicisi.
/// </summary>
public class GetResearchByProblemHandler
{
    private readonly IResearchRepository _repository;

    public GetResearchByProblemHandler(IResearchRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Probleme ait araştırma kaydını ResearchDto nesnesine dönüştürerek döner.
    /// </summary>
    public async Task<ResearchDto?> HandleAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        var research = await _repository.GetByProblemIdAsync(problemId, cancellationToken);
        if (research is null)
        {
            return null;
        }

        return new ResearchDto
        {
            Id = research.Id,
            ProblemId = research.ProblemId,
            MarketAnalysis = research.MarketAnalysis,
            CompetitorSummary = research.CompetitorSummary,
            TechnologyNotes = research.TechnologyNotes,
            TrendNotes = research.TrendNotes,
            Sources = research.Sources,
            ResearchedAt = research.ResearchedAt
        };
    }
}

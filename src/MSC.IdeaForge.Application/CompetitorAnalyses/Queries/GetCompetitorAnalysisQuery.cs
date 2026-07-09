using System.Text.Json;
using MSC.IdeaForge.Application.CompetitorAnalyses.DTOs;
using MSC.IdeaForge.Domain.Interfaces;
using MSC.IdeaForge.Domain.ValueObjects;

namespace MSC.IdeaForge.Application.CompetitorAnalyses.Queries;

/// <summary>
/// Belirtilen probleme ait rakip analizi verilerini getiren sorgu işleyicisi.
/// </summary>
public class GetCompetitorAnalysisHandler
{
    private readonly ICompetitorAnalysisRepository _repository;

    public GetCompetitorAnalysisHandler(ICompetitorAnalysisRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Probleme ait rakip analizi kaydını DTO formatında döner. Kayıt yoksa null döner.
    /// </summary>
    public async Task<CompetitorAnalysisDto?> HandleAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        var analysis = await _repository.GetByProblemIdAsync(problemId, cancellationToken);
        if (analysis is null)
        {
            return null;
        }

        // JSON olarak saklanan rakip listesini çözümlüyoruz
        List<Competitor> competitors;
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            competitors = JsonSerializer.Deserialize<List<Competitor>>(analysis.CompetitorsJson, options) ?? new();
        }
        catch
        {
            competitors = new();
        }

        return new CompetitorAnalysisDto
        {
            Id = analysis.Id,
            ProblemId = analysis.ProblemId,
            Competitors = competitors,
            AnalyzedAt = analysis.AnalyzedAt
        };
    }
}

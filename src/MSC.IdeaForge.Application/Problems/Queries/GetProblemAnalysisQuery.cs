using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Problems.Queries;

/// <summary>
/// Belirtilen probleme ait yapay zeka analiz verilerini getiren sorgu işleyicisi.
/// </summary>
public class GetProblemAnalysisHandler
{
    private readonly IProblemAnalysisRepository _repository;

    public GetProblemAnalysisHandler(IProblemAnalysisRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Probleme ait yapay zeka analizi kaydını döner.
    /// </summary>
    public async Task<ProblemAnalysis?> HandleAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByProblemIdAsync(problemId, cancellationToken);
    }
}

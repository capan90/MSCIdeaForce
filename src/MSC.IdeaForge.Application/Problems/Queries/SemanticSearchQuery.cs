using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Application.Problems.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Problems.Queries;

public class SemanticSearchQuery
{
    public string Query { get; set; } = string.Empty;
}

public class SemanticSearchHandler
{
    private readonly IProblemRepository _problemRepository;
    private readonly IAIProvider _aiProvider;

    public SemanticSearchHandler(IProblemRepository problemRepository, IAIProvider aiProvider)
    {
        _problemRepository = problemRepository;
        _aiProvider = aiProvider;
    }

    public async Task<List<SemanticSearchResultDto>> HandleAsync(SemanticSearchQuery query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query.Query))
        {
            return new List<SemanticSearchResultDto>();
        }

        // Tüm aktif problemleri çekelim
        var problems = (await _problemRepository.GetAllAsync(cancellationToken)).ToList();

        // AI ile semantik analizi tetikleyelim
        var rawResults = await _aiProvider.FindSimilarProblemsAsync(query.Query, problems);

        // Sonuçları DTO'ya map edelim ve benzerlik skoruna göre azalan sırada dönelim
        var results = new List<SemanticSearchResultDto>();
        foreach (var r in rawResults)
        {
            var matchedProblem = problems.FirstOrDefault(p => p.Id == r.ProblemId);
            if (matchedProblem != null)
            {
                results.Add(new SemanticSearchResultDto
                {
                    Id = matchedProblem.Id,
                    Title = matchedProblem.Title,
                    Description = matchedProblem.Description,
                    Tags = matchedProblem.Tags ?? string.Empty,
                    Status = matchedProblem.Status.ToString(),
                    SimilarityScore = r.SimilarityScore,
                    Reason = r.Reason
                });
            }
        }

        return results.OrderByDescending(r => r.SimilarityScore).ToList();
    }
}

public class SemanticSearchResultDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public double SimilarityScore { get; set; }
    public string Reason { get; set; } = string.Empty;
}

using MSC.IdeaForge.Domain.Interfaces;
using MSC.IdeaForge.Domain.ValueObjects;

namespace MSC.IdeaForge.Application.Opportunities.Commands;

/// <summary>
/// Yapay zeka ile fırsat skorlama komut nesnesi.
/// </summary>
public class AIScoreOpportunityCommand
{
    public Guid ProblemId { get; set; }
}

/// <summary>
/// Yapay zeka ile otomatik fırsat skorlamasını işleyen sınıf.
/// </summary>
public class AIScoreOpportunityHandler
{
    private readonly IProblemRepository _problemRepository;
    private readonly IAIProvider _aiProvider;

    public AIScoreOpportunityHandler(IProblemRepository problemRepository, IAIProvider aiProvider)
    {
        _problemRepository = problemRepository;
        _aiProvider = aiProvider;
    }

    /// <summary>
    /// Problem bilgilerini veritabanından çeker ve IAIProvider aracılığıyla fırsat puanlaması sonucunu döner.
    /// </summary>
    public async Task<OpportunityScoreResult> HandleAsync(AIScoreOpportunityCommand command, CancellationToken cancellationToken = default)
    {
        var problem = await _problemRepository.GetByIdAsync(command.ProblemId, cancellationToken);
        if (problem is null)
        {
            throw new InvalidOperationException("Otomatik puanlama yapılacak problem bulunamadı.");
        }

        // Yapay zeka sağlayıcısını çağırıyoruz.
        var result = await _aiProvider.ScoreOpportunityAsync(problem.Title, problem.Description, problem.Sector);
        return result;
    }
}

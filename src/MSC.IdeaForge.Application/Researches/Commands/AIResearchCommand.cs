using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Researches.Commands;

/// <summary>
/// Yapay zeka kullanarak problem hakkında araştırma yapılması komut nesnesi.
/// </summary>
public class AIResearchCommand
{
    public Guid ProblemId { get; set; }
}

/// <summary>
/// Yapay zeka ile araştırma komutunu işleyen sınıf.
/// </summary>
public class AIResearchHandler
{
    private readonly IProblemRepository _problemRepository;
    private readonly IResearchRepository _researchRepository;
    private readonly IAIProvider _aiProvider;

    public AIResearchHandler(
        IProblemRepository problemRepository,
        IResearchRepository researchRepository,
        IAIProvider aiProvider)
    {
        _problemRepository = problemRepository;
        _researchRepository = researchRepository;
        _aiProvider = aiProvider;
    }

    /// <summary>
    /// Problemi Gemini AI yardımıyla araştırır ve elde edilen verileri kalıcı olarak kaydeder.
    /// </summary>
    public async Task HandleAsync(AIResearchCommand command, CancellationToken cancellationToken = default)
    {
        var problem = await _problemRepository.GetByIdAsync(command.ProblemId, cancellationToken);
        if (problem is null)
        {
            throw new KeyNotFoundException($"ID'si {command.ProblemId} olan problem bulunamadı.");
        }

        // Yapay zeka araştırmasını tetikliyoruz
        var aiResult = await _aiProvider.ResearchProblemAsync(
            problem.Title,
            problem.Description,
            problem.Sector
        );

        var research = await _researchRepository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        if (research is null)
        {
            research = Research.Create(
                command.ProblemId,
                aiResult.MarketAnalysis,
                aiResult.CompetitorSummary,
                aiResult.TechnologyNotes,
                aiResult.TrendNotes,
                aiResult.Sources
            );
            await _researchRepository.AddAsync(research, cancellationToken);
        }
        else
        {
            research.Update(
                aiResult.MarketAnalysis,
                aiResult.CompetitorSummary,
                aiResult.TechnologyNotes,
                aiResult.TrendNotes,
                aiResult.Sources
            );
            await _researchRepository.UpdateAsync(research, cancellationToken);
        }

        await _researchRepository.SaveChangesAsync(cancellationToken);
    }
}

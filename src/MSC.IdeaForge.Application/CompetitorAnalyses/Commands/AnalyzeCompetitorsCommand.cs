using System.Text.Json;
using MSC.IdeaForge.Application.CompetitorAnalyses.DTOs;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.CompetitorAnalyses.Commands;

/// <summary>
/// Bir problem için yapay zeka ile rakip analizi yapılması komut nesnesi.
/// </summary>
public class AnalyzeCompetitorsCommand
{
    public Guid ProblemId { get; set; }
}

/// <summary>
/// Yapay zeka ile rakip analizi komutunu işleyen sınıf.
/// </summary>
public class AnalyzeCompetitorsHandler
{
    private readonly IProblemRepository _problemRepository;
    private readonly ICompetitorAnalysisRepository _competitorRepository;
    private readonly IAIProvider _aiProvider;

    public AnalyzeCompetitorsHandler(
        IProblemRepository problemRepository,
        ICompetitorAnalysisRepository competitorRepository,
        IAIProvider aiProvider)
    {
        _problemRepository = problemRepository;
        _competitorRepository = competitorRepository;
        _aiProvider = aiProvider;
    }

    /// <summary>
    /// Problemi Gemini AI yardımıyla rakip analizi açısından değerlendirir ve sonucu kalıcı olarak kaydeder.
    /// </summary>
    public async Task<CompetitorAnalysisDto> HandleAsync(AnalyzeCompetitorsCommand command, CancellationToken cancellationToken = default)
    {
        var problem = await _problemRepository.GetByIdAsync(command.ProblemId, cancellationToken);
        if (problem is null)
        {
            throw new KeyNotFoundException($"ID'si {command.ProblemId} olan problem bulunamadı.");
        }

        // Yapay zeka rakip analizini tetikliyoruz
        var competitors = await _aiProvider.AnalyzeCompetitorsAsync(problem.Title, problem.Description, problem.Sector);
        var json = JsonSerializer.Serialize(competitors);

        // Mevcut kayıt varsa güncelliyor, yoksa yenisini oluşturuyoruz
        var analysis = await _competitorRepository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        if (analysis is null)
        {
            analysis = CompetitorAnalysis.Create(command.ProblemId, json);
            await _competitorRepository.AddAsync(analysis, cancellationToken);
        }
        else
        {
            analysis.Update(json);
            await _competitorRepository.UpdateAsync(analysis, cancellationToken);
        }

        await _competitorRepository.SaveChangesAsync(cancellationToken);

        return new CompetitorAnalysisDto
        {
            Id = analysis.Id,
            ProblemId = analysis.ProblemId,
            Competitors = competitors,
            AnalyzedAt = analysis.AnalyzedAt
        };
    }
}

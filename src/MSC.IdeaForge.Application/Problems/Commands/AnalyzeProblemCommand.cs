using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;
using MSC.IdeaForge.Domain.ValueObjects;

namespace MSC.IdeaForge.Application.Problems.Commands;

/// <summary>
/// Problem yapay zeka analizi komut nesnesi.
/// </summary>
public class AnalyzeProblemCommand
{
    public Guid ProblemId { get; set; }
}

/// <summary>
/// Problem yapay zeka analiz komutunu işleyen sınıf.
/// </summary>
public class AnalyzeProblemHandler
{
    private readonly IProblemRepository _repository;
    private readonly IProblemAnalysisRepository _analysisRepository;
    private readonly IAIProvider _aiProvider;

    public AnalyzeProblemHandler(IProblemRepository repository, IProblemAnalysisRepository analysisRepository, IAIProvider aiProvider)
    {
        _repository = repository;
        _analysisRepository = analysisRepository;
        _aiProvider = aiProvider;
    }

    /// <summary>
    /// Problemi veri tabanından getirir, yapay zekaya analiz ettirir, sonucu veritabanına kaydeder ve döner.
    /// </summary>
    /// <param name="problemId">Analiz edilecek problemin benzersiz kimliği</param>
    /// <param name="cancellationToken">İptal belirteci</param>
    /// <returns>Analiz sonuçlarını içeren ProblemAnalysisResult nesnesini döner.</returns>
    /// <exception cref="KeyNotFoundException">Problem bulunamazsa fırlatılır.</exception>
    public async Task<ProblemAnalysisResult> HandleAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        // Veritabanından problemi sorguluyoruz.
        var problem = await _repository.GetByIdAsync(problemId, cancellationToken);
        if (problem is null)
        {
            throw new KeyNotFoundException($"ID'si {problemId} olan problem bulunamadı.");
        }

        // AI sağlayıcısı üzerinden analiz işlemini gerçekleştiriyoruz.
        var analysisResult = await _aiProvider.AnalyzeProblemAsync(
            problem.Title,
            problem.Description,
            problem.Sector
        );

        // Sonucu veritabanına kaydediyoruz veya güncelliyoruz.
        var problemAnalysis = await _analysisRepository.GetByProblemIdAsync(problemId, cancellationToken);
        if (problemAnalysis is null)
        {
            problemAnalysis = ProblemAnalysis.Create(
                problemId,
                analysisResult.Summary,
                analysisResult.SuggestedCategory,
                analysisResult.SuggestedTags,
                analysisResult.RiskLevel,
                analysisResult.SolutionTypeSuggestion,
                analysisResult.ConfidenceScore
            );
            await _analysisRepository.AddAsync(problemAnalysis, cancellationToken);
        }
        else
        {
            problemAnalysis.Update(
                analysisResult.Summary,
                analysisResult.SuggestedCategory,
                analysisResult.SuggestedTags,
                analysisResult.RiskLevel,
                analysisResult.SolutionTypeSuggestion,
                analysisResult.ConfidenceScore
            );
            await _analysisRepository.UpdateAsync(problemAnalysis, cancellationToken);
        }

        await _analysisRepository.SaveChangesAsync(cancellationToken);

        return analysisResult;
    }
}

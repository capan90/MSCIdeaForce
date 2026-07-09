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
    private readonly IAIProvider _aiProvider;

    public AnalyzeProblemHandler(IProblemRepository repository, IAIProvider aiProvider)
    {
        _repository = repository;
        _aiProvider = aiProvider;
    }

    /// <summary>
    /// Problemi veri tabanından getirir, yapay zekaya analiz ettirir ve sonucu döner.
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

        return analysisResult;
    }
}

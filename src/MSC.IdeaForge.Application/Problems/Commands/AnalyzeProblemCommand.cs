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
        Console.WriteLine("AnalyzeProblemHandler başladı");

        MSC.IdeaForge.Domain.Entities.Problem? problem = null;
        try
        {
            Console.WriteLine("Problem veritabanından getiriliyor...");
            problem = await _repository.GetByIdAsync(problemId, cancellationToken);
            Console.WriteLine($"Problem veritabanından getirildi. Bulundu mu: {problem != null}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Problem getirme hatası: {ex.Message}");
            throw;
        }

        if (problem is null)
        {
            Console.WriteLine($"Hata: ID'si {problemId} olan problem bulunamadı.");
            throw new KeyNotFoundException($"ID'si {problemId} olan problem bulunamadı.");
        }

        ProblemAnalysisResult? analysisResult = null;
        try
        {
            Console.WriteLine("IAIProvider.AnalyzeProblemAsync çağrılıyor...");
            analysisResult = await _aiProvider.AnalyzeProblemAsync(
                problem.Title,
                problem.Description,
                problem.Sector
            );
            Console.WriteLine("IAIProvider.AnalyzeProblemAsync yanıtı alındı.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"IAIProvider.AnalyzeProblemAsync hatası: {ex.Message}");
            throw;
        }

        MSC.IdeaForge.Domain.Entities.ProblemAnalysis? problemAnalysis = null;
        try
        {
            Console.WriteLine("Problem analizi veritabanından sorgulanıyor...");
            problemAnalysis = await _analysisRepository.GetByProblemIdAsync(problemId, cancellationToken);
            Console.WriteLine($"Problem analizi sorgulandı. Var mıydı: {problemAnalysis != null}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Problem analizi sorgulama hatası: {ex.Message}");
            throw;
        }

        try
        {
            if (problemAnalysis is null)
            {
                Console.WriteLine("Yeni ProblemAnalysis oluşturuluyor...");
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
                Console.WriteLine("Yeni ProblemAnalysis eklendi.");
            }
            else
            {
                Console.WriteLine("Var olan ProblemAnalysis güncelleniyor...");
                problemAnalysis.Update(
                    analysisResult.Summary,
                    analysisResult.SuggestedCategory,
                    analysisResult.SuggestedTags,
                    analysisResult.RiskLevel,
                    analysisResult.SolutionTypeSuggestion,
                    analysisResult.ConfidenceScore
                );
                await _analysisRepository.UpdateAsync(problemAnalysis, cancellationToken);
                Console.WriteLine("Var olan ProblemAnalysis güncellendi.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ProblemAnalysis ekleme/güncelleme hatası: {ex.Message}");
            throw;
        }

        try
        {
            Console.WriteLine("Değişiklikler kaydediliyor...");
            await _analysisRepository.SaveChangesAsync(cancellationToken);
            Console.WriteLine("Değişiklikler kaydedildi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Değişiklikleri kaydetme hatası: {ex.Message}");
            throw;
        }

        return analysisResult;
    }
}

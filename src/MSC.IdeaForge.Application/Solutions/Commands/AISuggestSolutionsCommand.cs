using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Solutions.Commands;

/// <summary>
/// Yapay zekadan çözüm önerileri üretilmesini tetikleyen komut nesnesi.
/// </summary>
public class AISuggestSolutionsCommand
{
    public Guid ProblemId { get; set; }
}

/// <summary>
/// Yapay zeka ile çözüm önerisi üretme komutunu işleyen sınıf.
/// </summary>
public class AISuggestSolutionsHandler
{
    private readonly IProblemRepository _problemRepository;
    private readonly ISolutionRepository _solutionRepository;
    private readonly IAIProvider _aiProvider;

    public AISuggestSolutionsHandler(
        IProblemRepository problemRepository,
        ISolutionRepository solutionRepository,
        IAIProvider aiProvider)
    {
        _problemRepository = problemRepository;
        _solutionRepository = solutionRepository;
        _aiProvider = aiProvider;
    }

    /// <summary>
    /// Problemi analiz eder, 3 çözüm alternatifi üretir, eski çözümleri silip yenilerini veritabanına kaydeder.
    /// </summary>
    public async Task HandleAsync(AISuggestSolutionsCommand command, CancellationToken cancellationToken = default)
    {
        var problem = await _problemRepository.GetByIdAsync(command.ProblemId, cancellationToken);
        if (problem is null)
        {
            throw new KeyNotFoundException($"ID'si {command.ProblemId} olan problem bulunamadı.");
        }

        // AI ile önerileri alıyoruz
        var suggestions = await _aiProvider.SuggestSolutionsAsync(
            problem.Title,
            problem.Description,
            problem.Sector
        );

        // Eski çözümleri temizliyoruz (yeniden üretildiğinde temiz liste olması için)
        var existingSolutions = await _solutionRepository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        foreach (var oldSolution in existingSolutions)
        {
            await _solutionRepository.DeleteAsync(oldSolution, cancellationToken);
        }

        // Yeni çözüm önerilerini ekliyoruz
        foreach (var suggestion in suggestions)
        {
            var solution = Solution.Create(
                command.ProblemId,
                suggestion.SolutionType,
                suggestion.Summary,
                suggestion.Complexity,
                suggestion.EstimatedDurationMonths,
                suggestion.EstimatedCost,
                suggestion.Pros,
                suggestion.Cons,
                suggestion.IsRecommended
            );
            await _solutionRepository.AddAsync(solution, cancellationToken);
        }

        await _solutionRepository.SaveChangesAsync(cancellationToken);
    }
}

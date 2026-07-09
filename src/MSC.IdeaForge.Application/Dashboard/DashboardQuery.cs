using MSC.IdeaForge.Domain.Enums;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Dashboard;

/// <summary>
/// Dashboard verilerini getiren sorgu nesnesi.
/// </summary>
public class DashboardQuery
{
}

/// <summary>
/// Dashboard sorgusunu işleyen sınıf.
/// </summary>
public class DashboardQueryHandler
{
    private readonly IProblemRepository _problemRepository;
    private readonly ISignalRepository _signalRepository;
    private readonly IOpportunityRepository _opportunityRepository;
    private readonly IValidationRepository _validationRepository;
    private readonly IProblemAnalysisRepository _problemAnalysisRepository;

    public DashboardQueryHandler(
        IProblemRepository problemRepository,
        ISignalRepository signalRepository,
        IOpportunityRepository opportunityRepository,
        IValidationRepository validationRepository,
        IProblemAnalysisRepository problemAnalysisRepository)
    {
        _problemRepository = problemRepository;
        _signalRepository = signalRepository;
        _opportunityRepository = opportunityRepository;
        _validationRepository = validationRepository;
        _problemAnalysisRepository = problemAnalysisRepository;
    }

    /// <summary>
    /// Dashboard için gerekli olan tüm istatistik ve listeleri tek bir veritabanı turunda birleştirerek getirir.
    /// </summary>
    public async Task<DashboardDto> HandleAsync(DashboardQuery query, CancellationToken cancellationToken = default)
    {
        // Tüm ana listeleri paralel veya sırayla çekiyoruz.
        var problems = (await _problemRepository.GetAllAsync(cancellationToken)).ToList();
        var opportunities = (await _opportunityRepository.GetAllAsync(cancellationToken)).ToList();
        var validations = (await _validationRepository.GetAllAsync(cancellationToken)).ToList();
        var totalSignalsCount = await _signalRepository.GetCountAsync(cancellationToken);
        var totalAiAnalysisCount = await _problemAnalysisRepository.GetCountAsync(cancellationToken);

        // İstatistik hesaplamaları
        var totalProblemsCount = problems.Count;
        var analyzedProblemsCount = problems.Count(p => p.Status == ProblemStatus.Analyzed);
        var validatedProblemsCount = problems.Count(p => p.Status == ProblemStatus.Validated);

        // Ortalama fırsat skoru (fırsat kaydı olmayan durumda 0)
        var averageOpportunityScore = opportunities.Any()
            ? Math.Round(opportunities.Average(o => o.OpportunityScore.TotalScore), 1)
            : 0.0;

        // En yüksek fırsat skoruna sahip ilk 5 problem
        var topOpportunityProblems = opportunities
            .OrderByDescending(o => o.OpportunityScore.TotalScore)
            .Take(5)
            .Select(o => {
                var prob = problems.FirstOrDefault(p => p.Id == o.ProblemId);
                return new OpportunityProblemDto
                {
                    Id = o.ProblemId,
                    Title = prob?.Title ?? "Bilinmeyen Problem",
                    OpportunityScore = o.OpportunityScore.TotalScore,
                    Status = prob?.Status.ToString() ?? "Draft"
                };
            })
            .ToList();

        // Son eklenen 5 problem
        var recentProblems = problems
            .OrderByDescending(p => p.CreatedAt)
            .Take(5)
            .Select(p => new RecentProblemDto
            {
                Id = p.Id,
                Title = p.Title,
                Priority = p.Priority.ToString(),
                Status = p.Status.ToString(),
                CreatedAt = p.CreatedAt
            })
            .ToList();

        // Doğrulama bekleyen problemler (Doğrulama kaydı olmayan veya durumu NotStarted olanlar)
        var nonPendingValidationProblemIds = validations
            .Where(v => v.Status != ValidationStatus.NotStarted)
            .Select(v => v.ProblemId)
            .ToHashSet();

        var validationPendingProblems = problems
            .Where(p => !nonPendingValidationProblemIds.Contains(p.Id))
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ValidationPendingProblemDto
            {
                Id = p.Id,
                Title = p.Title,
                Priority = p.Priority.ToString(),
                CreatedAt = p.CreatedAt
            })
            .ToList();

        return new DashboardDto
        {
            TotalProblemsCount = totalProblemsCount,
            AnalyzedProblemsCount = analyzedProblemsCount,
            ValidatedProblemsCount = validatedProblemsCount,
            TotalSignalsCount = totalSignalsCount,
            TotalAiAnalysisCount = totalAiAnalysisCount,
            AverageOpportunityScore = averageOpportunityScore,
            TopOpportunityProblems = topOpportunityProblems,
            RecentProblems = recentProblems,
            ValidationPendingProblems = validationPendingProblems
        };
    }
}

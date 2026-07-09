using MSC.IdeaForge.Application.Revenues.DTOs;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Revenues.Commands;

/// <summary>
/// Bir problem için yapay zeka ile gelir modeli analizi yapılması komut nesnesi.
/// </summary>
public class AnalyzeRevenueCommand
{
    public Guid ProblemId { get; set; }

    // Opsiyonel: Analizde dikkate alınacak çözüm tipi (varsa)
    public string? SolutionType { get; set; }
}

/// <summary>
/// Yapay zeka ile gelir analizi komutunu işleyen sınıf.
/// </summary>
public class AnalyzeRevenueHandler
{
    private readonly IProblemRepository _problemRepository;
    private readonly IRevenueAnalysisRepository _revenueRepository;
    private readonly IAIProvider _aiProvider;

    public AnalyzeRevenueHandler(
        IProblemRepository problemRepository,
        IRevenueAnalysisRepository revenueRepository,
        IAIProvider aiProvider)
    {
        _problemRepository = problemRepository;
        _revenueRepository = revenueRepository;
        _aiProvider = aiProvider;
    }

    /// <summary>
    /// Problemi Gemini AI yardımıyla gelir modeli açısından analiz eder ve sonucu kalıcı olarak kaydeder.
    /// </summary>
    public async Task<RevenueAnalysisDto> HandleAsync(AnalyzeRevenueCommand command, CancellationToken cancellationToken = default)
    {
        var problem = await _problemRepository.GetByIdAsync(command.ProblemId, cancellationToken);
        if (problem is null)
        {
            throw new KeyNotFoundException($"ID'si {command.ProblemId} olan problem bulunamadı.");
        }

        // Yapay zeka gelir analizini tetikliyoruz
        var aiResult = await _aiProvider.AnalyzeRevenueAsync(
            problem.Title,
            problem.Description,
            problem.Sector,
            command.SolutionType
        );

        // Mevcut kayıt varsa güncelliyor, yoksa yenisini oluşturuyoruz
        var revenue = await _revenueRepository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        if (revenue is null)
        {
            revenue = RevenueAnalysis.Create(
                command.ProblemId,
                aiResult.RevenueModel,
                aiResult.MonthlyRevenueEstimate,
                aiResult.AnnualRevenueEstimate,
                aiResult.PricingStrategy,
                aiResult.TargetCustomer,
                aiResult.SalesChannel,
                aiResult.Scalability,
                aiResult.Risks
            );
            await _revenueRepository.AddAsync(revenue, cancellationToken);
        }
        else
        {
            revenue.Update(
                aiResult.RevenueModel,
                aiResult.MonthlyRevenueEstimate,
                aiResult.AnnualRevenueEstimate,
                aiResult.PricingStrategy,
                aiResult.TargetCustomer,
                aiResult.SalesChannel,
                aiResult.Scalability,
                aiResult.Risks
            );
            await _revenueRepository.UpdateAsync(revenue, cancellationToken);
        }

        await _revenueRepository.SaveChangesAsync(cancellationToken);

        return new RevenueAnalysisDto
        {
            Id = revenue.Id,
            ProblemId = revenue.ProblemId,
            RevenueModel = revenue.RevenueModel,
            MonthlyRevenueEstimate = revenue.MonthlyRevenueEstimate,
            AnnualRevenueEstimate = revenue.AnnualRevenueEstimate,
            PricingStrategy = revenue.PricingStrategy,
            TargetCustomer = revenue.TargetCustomer,
            SalesChannel = revenue.SalesChannel,
            Scalability = revenue.Scalability,
            Risks = revenue.Risks,
            AnalyzedAt = revenue.AnalyzedAt
        };
    }
}

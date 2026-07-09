using MSC.IdeaForge.Application.Revenues.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Revenues.Queries;

/// <summary>
/// Belirtilen probleme ait gelir analizi verilerini getiren sorgu işleyicisi.
/// </summary>
public class GetRevenueAnalysisHandler
{
    private readonly IRevenueAnalysisRepository _repository;

    public GetRevenueAnalysisHandler(IRevenueAnalysisRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Probleme ait gelir analizi kaydını DTO formatında döner.
    /// </summary>
    public async Task<RevenueAnalysisDto?> HandleAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        var revenue = await _repository.GetByProblemIdAsync(problemId, cancellationToken);
        if (revenue is null)
        {
            return null;
        }

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

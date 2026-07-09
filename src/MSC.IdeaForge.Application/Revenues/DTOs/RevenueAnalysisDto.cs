using System;

namespace MSC.IdeaForge.Application.Revenues.DTOs;

/// <summary>
/// Gelir analizi verilerini taşımak için kullanılan DTO.
/// </summary>
public class RevenueAnalysisDto
{
    public Guid Id { get; set; }
    public Guid ProblemId { get; set; }
    public string RevenueModel { get; set; } = string.Empty;
    public string MonthlyRevenueEstimate { get; set; } = string.Empty;
    public string AnnualRevenueEstimate { get; set; } = string.Empty;
    public string PricingStrategy { get; set; } = string.Empty;
    public string TargetCustomer { get; set; } = string.Empty;
    public string SalesChannel { get; set; } = string.Empty;
    public string Scalability { get; set; } = string.Empty;
    public string Risks { get; set; } = string.Empty;
    public DateTime AnalyzedAt { get; set; }
}

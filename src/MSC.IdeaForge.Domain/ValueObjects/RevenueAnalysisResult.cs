namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Yapay zeka tarafından yapılan gelir modeli analizi sonuçlarını temsil eden değer nesnesi (Value Object).
/// </summary>
public class RevenueAnalysisResult
{
    public string RevenueModel { get; set; } = string.Empty;
    public string MonthlyRevenueEstimate { get; set; } = string.Empty;
    public string AnnualRevenueEstimate { get; set; } = string.Empty;
    public string PricingStrategy { get; set; } = string.Empty;
    public string TargetCustomer { get; set; } = string.Empty;
    public string SalesChannel { get; set; } = string.Empty;
    public string Scalability { get; set; } = string.Empty;
    public string Risks { get; set; } = string.Empty;
}

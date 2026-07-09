using System.Collections.Generic;

namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Yapay zeka tarafından üretilen fiyatlandırma stratejisi sonuçlarını temsil eden değer nesnesi.
/// </summary>
public class PricingStrategyResult
{
    public string RecommendedModel { get; set; } = string.Empty;
    public List<PricingTier> PricingTiers { get; set; } = new();
    public string FreeTrialStrategy { get; set; } = string.Empty;
    public string DiscountStrategy { get; set; } = string.Empty;
    public string CompetitorPricing { get; set; } = string.Empty;
    public string PsychologicalPricingTips { get; set; } = string.Empty;
    public string RevenueProjection { get; set; } = string.Empty;
}

/// <summary>
/// Tek bir fiyatlandırma kademesini (tier) temsil eden değer nesnesi.
/// </summary>
public class PricingTier
{
    public string TierName { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
    public string Features { get; set; } = string.Empty;
    public string TargetSegment { get; set; } = string.Empty;
}

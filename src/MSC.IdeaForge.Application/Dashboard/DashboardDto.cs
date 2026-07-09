using System;
using System.Collections.Generic;

namespace MSC.IdeaForge.Application.Dashboard;

/// <summary>
/// Dashboard sayfasında gösterilecek tüm özet verileri barındıran veri transfer nesnesi (DTO).
/// </summary>
public class DashboardDto
{
    public int TotalProblemsCount { get; set; }
    public int AnalyzedProblemsCount { get; set; }
    public int ValidatedProblemsCount { get; set; }
    public int TotalSignalsCount { get; set; }
    public int TotalAiAnalysisCount { get; set; }
    public double AverageOpportunityScore { get; set; }
    public int UnscoredProblemsCount { get; set; }
    public List<OpportunityProblemDto> TopOpportunityProblems { get; set; } = new();
    public List<RecentProblemDto> RecentProblems { get; set; } = new();
    public List<ValidationPendingProblemDto> ValidationPendingProblems { get; set; } = new();
}

/// <summary>
/// En yüksek fırsat skoruna sahip problemler için basitleştirilmiş DTO.
/// </summary>
public class OpportunityProblemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public double OpportunityScore { get; set; }
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// Son eklenen problemler için basitleştirilmiş DTO.
/// </summary>
public class RecentProblemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Doğrulama bekleyen problemler için basitleştirilmiş DTO.
/// </summary>
public class ValidationPendingProblemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

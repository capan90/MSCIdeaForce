using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Application.Problems.Commands;
using MSC.IdeaForge.Application.Researches.Commands;
using MSC.IdeaForge.Application.Opportunities.Commands;
using MSC.IdeaForge.Application.Solutions.Commands;
using MSC.IdeaForge.Application.MVPPlans.Commands;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Agents;

/// <summary>
/// Tüm yapay zeka analiz adımlarını sırayla tetikleyen ve yöneten koordinatör sınıfı.
/// </summary>
public class AgentCoordinator
{
    private readonly AnalyzeProblemHandler _analyzeProblemHandler;
    private readonly AIResearchHandler _aiResearchHandler;
    private readonly AIScoreOpportunityHandler _aiScoreOpportunityHandler;
    private readonly ScoreOpportunityHandler _scoreOpportunityHandler;
    private readonly AISuggestSolutionsHandler _aiSuggestSolutionsHandler;
    private readonly ISolutionRepository _solutionRepository;
    private readonly GenerateMVPPlanHandler _generateMVPPlanHandler;

    public AgentCoordinator(
        AnalyzeProblemHandler analyzeProblemHandler,
        AIResearchHandler aiResearchHandler,
        AIScoreOpportunityHandler aiScoreOpportunityHandler,
        ScoreOpportunityHandler scoreOpportunityHandler,
        AISuggestSolutionsHandler aiSuggestSolutionsHandler,
        ISolutionRepository solutionRepository,
        GenerateMVPPlanHandler generateMVPPlanHandler)
    {
        _analyzeProblemHandler = analyzeProblemHandler;
        _aiResearchHandler = aiResearchHandler;
        _aiScoreOpportunityHandler = aiScoreOpportunityHandler;
        _scoreOpportunityHandler = scoreOpportunityHandler;
        _aiSuggestSolutionsHandler = aiSuggestSolutionsHandler;
        _solutionRepository = solutionRepository;
        _generateMVPPlanHandler = generateMVPPlanHandler;
    }

    /// <summary>
    /// Belirtilen probleme ait tam analizi sırayla başlatır.
    /// Her adım tamamlandığında veya hata aldığında onProgress callback'ini çağırır.
    /// </summary>
    public async Task<List<AgentProgress>> RunFullAnalysisAsync(Guid problemId, Action<List<AgentProgress>>? onProgress = null, CancellationToken cancellationToken = default)
    {
        var steps = new List<AgentProgress>
        {
            new() { Step = "Problem Analizi", Status = AgentStepStatus.Pending },
            new() { Step = "Araştırma", Status = AgentStepStatus.Pending },
            new() { Step = "Fırsat Skorlama", Status = AgentStepStatus.Pending },
            new() { Step = "Çözüm Önerisi", Status = AgentStepStatus.Pending },
            new() { Step = "MVP Planı", Status = AgentStepStatus.Pending }
        };

        void ReportProgress(int stepIndex, AgentStepStatus status, string? errorMessage = null)
        {
            steps[stepIndex].Status = status;
            steps[stepIndex].ErrorMessage = errorMessage;
            if (status == AgentStepStatus.Completed || status == AgentStepStatus.Failed)
            {
                steps[stepIndex].CompletedAt = DateTime.UtcNow;
            }
            onProgress?.Invoke(steps);
        }

        // --- 1. PROBLEM ANALİZİ ---
        ReportProgress(0, AgentStepStatus.Running);
        try
        {
            await _analyzeProblemHandler.HandleAsync(problemId, cancellationToken);
            ReportProgress(0, AgentStepStatus.Completed);
        }
        catch (Exception ex)
        {
            ReportProgress(0, AgentStepStatus.Failed, ex.Message);
        }

        // --- 2. ARAŞTIRMA ---
        ReportProgress(1, AgentStepStatus.Running);
        try
        {
            await _aiResearchHandler.HandleAsync(new AIResearchCommand { ProblemId = problemId }, cancellationToken);
            ReportProgress(1, AgentStepStatus.Completed);
        }
        catch (Exception ex)
        {
            ReportProgress(1, AgentStepStatus.Failed, ex.Message);
        }

        // --- 3. FIRSAT SKORLAMA ---
        ReportProgress(2, AgentStepStatus.Running);
        try
        {
            var aiScore = await _aiScoreOpportunityHandler.HandleAsync(new AIScoreOpportunityCommand { ProblemId = problemId }, cancellationToken);
            var notes = $"Yapay zeka otomatik puanlama gerekçeleri:\n- Şiddet: {aiScore.ProblemSeverityReason}\n- Pazar: {aiScore.MarketSizeReason}\n- Kurucu: {aiScore.FounderFitReason}\n- Risk: {aiScore.RiskScoreReason}\n- Teknik: {aiScore.TechnicalFeasibilityReason}";
            
            await _scoreOpportunityHandler.HandleAsync(new ScoreOpportunityCommand
            {
                ProblemId = problemId,
                ProblemSeverity = aiScore.ProblemSeverity,
                MarketSize = aiScore.MarketSize,
                FounderFit = aiScore.FounderFit,
                RiskScore = aiScore.RiskScore,
                TechnicalFeasibility = aiScore.TechnicalFeasibility,
                Notes = notes
            }, cancellationToken);

            ReportProgress(2, AgentStepStatus.Completed);
        }
        catch (Exception ex)
        {
            ReportProgress(2, AgentStepStatus.Failed, ex.Message);
        }

        // --- 4. ÇÖZÜM ÖNERİLERİ ---
        ReportProgress(3, AgentStepStatus.Running);
        string? recommendedSolutionType = null;
        try
        {
            await _aiSuggestSolutionsHandler.HandleAsync(new AISuggestSolutionsCommand { ProblemId = problemId }, cancellationToken);
            
            // Sonraki MVP adımında kullanabilmek için recommended olan çözümün adını buluyoruz
            var solutions = await _solutionRepository.GetByProblemIdAsync(problemId, cancellationToken);
            recommendedSolutionType = solutions.FirstOrDefault(s => s.IsRecommended)?.SolutionType.ToString()
                                     ?? solutions.FirstOrDefault()?.SolutionType.ToString();

            ReportProgress(3, AgentStepStatus.Completed);
        }
        catch (Exception ex)
        {
            ReportProgress(3, AgentStepStatus.Failed, ex.Message);
        }

        // --- 5. MVP PLANI ---
        ReportProgress(4, AgentStepStatus.Running);
        try
        {
            await _generateMVPPlanHandler.HandleAsync(new GenerateMVPPlanCommand 
            { 
                ProblemId = problemId,
                SolutionType = recommendedSolutionType
            }, cancellationToken);
            ReportProgress(4, AgentStepStatus.Completed);
        }
        catch (Exception ex)
        {
            ReportProgress(4, AgentStepStatus.Failed, ex.Message);
        }

        return steps;
    }
}

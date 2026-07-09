using System;
using System.Text.Json;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.MVPPlans.Commands;

/// <summary>
/// Yapay zeka ile otomatik MVP planı oluşturulması komut nesnesi.
/// </summary>
public class GenerateMVPPlanCommand
{
    public Guid ProblemId { get; set; }
    public string? SolutionType { get; set; }
}

/// <summary>
/// Yapay zeka ile MVP planı oluşturma komut işleyicisi.
/// </summary>
public class GenerateMVPPlanHandler
{
    private readonly IProblemRepository _problemRepository;
    private readonly IMVPPlanRepository _mvpPlanRepository;
    private readonly IAIProvider _aiProvider;

    public GenerateMVPPlanHandler(
        IProblemRepository problemRepository,
        IMVPPlanRepository mvpPlanRepository,
        IAIProvider aiProvider)
    {
        _problemRepository = problemRepository;
        _mvpPlanRepository = mvpPlanRepository;
        _aiProvider = aiProvider;
    }

    /// <summary>
    /// Problemi ve çözüm tipini analiz eder, yapay zekaya bir MVP planı hazırlatır ve kalıcı olarak veritabanına kaydeder.
    /// </summary>
    public async Task HandleAsync(GenerateMVPPlanCommand command, CancellationToken cancellationToken = default)
    {
        var problem = await _problemRepository.GetByIdAsync(command.ProblemId, cancellationToken);
        if (problem is null)
        {
            throw new KeyNotFoundException($"ID'si {command.ProblemId} olan problem bulunamadı.");
        }

        // Yapay zeka ile MVP planı üretiyoruz
        var aiResult = await _aiProvider.GenerateMVPPlanAsync(
            problem.Title,
            problem.Description,
            problem.Sector,
            command.SolutionType
        );

        var userStoriesJson = JsonSerializer.Serialize(aiResult.UserStories);
        var sprintPlanJson = JsonSerializer.Serialize(aiResult.SprintPlan);

        // Varsa güncelle, yoksa ekle
        var mvpPlan = await _mvpPlanRepository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        if (mvpPlan is null)
        {
            mvpPlan = MVPPlan.Create(
                command.ProblemId,
                aiResult.Scope,
                userStoriesJson,
                sprintPlanJson,
                aiResult.TechStack,
                aiResult.Risks,
                aiResult.FirstSalesPlan
            );
            await _mvpPlanRepository.AddAsync(mvpPlan, cancellationToken);
        }
        else
        {
            mvpPlan.Update(
                aiResult.Scope,
                userStoriesJson,
                sprintPlanJson,
                aiResult.TechStack,
                aiResult.Risks,
                aiResult.FirstSalesPlan
            );
            await _mvpPlanRepository.UpdateAsync(mvpPlan, cancellationToken);
        }

        await _mvpPlanRepository.SaveChangesAsync(cancellationToken);
    }
}

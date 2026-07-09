using System.Linq;
using System.Text.Json;
using MSC.IdeaForge.Application.ActionPlans.DTOs;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;
using MSC.IdeaForge.Domain.ValueObjects;

namespace MSC.IdeaForge.Application.ActionPlans.Commands;

/// <summary>
/// Bir problem için yapay zeka ile 30-60-90 günlük aksiyon planı üretilmesi komut nesnesi.
/// </summary>
public class GenerateActionPlanCommand
{
    public Guid ProblemId { get; set; }
}

/// <summary>
/// Aksiyon planı üretme komutunu işleyen sınıf. Üretilen plan kalıcı olarak kaydedilir.
/// </summary>
public class GenerateActionPlanHandler
{
    private readonly IProblemRepository _problemRepository;
    private readonly ISolutionRepository _solutionRepository;
    private readonly IActionPlanRepository _actionPlanRepository;
    private readonly IAIProvider _aiProvider;

    public GenerateActionPlanHandler(
        IProblemRepository problemRepository,
        ISolutionRepository solutionRepository,
        IActionPlanRepository actionPlanRepository,
        IAIProvider aiProvider)
    {
        _problemRepository = problemRepository;
        _solutionRepository = solutionRepository;
        _actionPlanRepository = actionPlanRepository;
        _aiProvider = aiProvider;
    }

    /// <summary>
    /// Problemi yapay zekaya göndererek aksiyon planı üretir, JSON olarak kaydeder ve DTO döner.
    /// </summary>
    public async Task<ActionPlanDto> HandleAsync(GenerateActionPlanCommand command, CancellationToken cancellationToken = default)
    {
        var problem = await _problemRepository.GetByIdAsync(command.ProblemId, cancellationToken);
        if (problem is null)
        {
            throw new KeyNotFoundException($"ID'si {command.ProblemId} olan problem bulunamadı.");
        }

        // Önerilen çözüm tipini (varsa) plana bağlam olarak veriyoruz
        var solutions = await _solutionRepository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        var solutionType = solutions.FirstOrDefault(s => s.IsRecommended)?.SolutionType.ToString()
                           ?? solutions.FirstOrDefault()?.SolutionType.ToString();

        var aiResult = await _aiProvider.GenerateActionPlanAsync(problem.Title, problem.Description, problem.Sector, solutionType);
        var planJson = JsonSerializer.Serialize(aiResult);

        var actionPlan = await _actionPlanRepository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        if (actionPlan is null)
        {
            actionPlan = ActionPlan.Create(command.ProblemId, planJson);
            await _actionPlanRepository.AddAsync(actionPlan, cancellationToken);
        }
        else
        {
            actionPlan.Update(planJson);
            await _actionPlanRepository.UpdateAsync(actionPlan, cancellationToken);
        }

        await _actionPlanRepository.SaveChangesAsync(cancellationToken);

        return new ActionPlanDto
        {
            Id = actionPlan.Id,
            ProblemId = actionPlan.ProblemId,
            Day30 = aiResult.Day30,
            Day60 = aiResult.Day60,
            Day90 = aiResult.Day90,
            SuccessMetrics = aiResult.SuccessMetrics,
            QuickWins = aiResult.QuickWins,
            GeneratedAt = actionPlan.GeneratedAt
        };
    }
}

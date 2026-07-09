using System.Text.Json;
using MSC.IdeaForge.Application.ActionPlans.DTOs;
using MSC.IdeaForge.Domain.Interfaces;
using MSC.IdeaForge.Domain.ValueObjects;

namespace MSC.IdeaForge.Application.ActionPlans.Queries;

/// <summary>
/// Belirtilen probleme ait aksiyon planını getiren sorgu işleyicisi.
/// </summary>
public class GetActionPlanHandler
{
    private readonly IActionPlanRepository _repository;

    public GetActionPlanHandler(IActionPlanRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Probleme ait aksiyon planı kaydını (JSON'dan çözümleyerek) DTO olarak döner. Kayıt yoksa null döner.
    /// </summary>
    public async Task<ActionPlanDto?> HandleAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        var actionPlan = await _repository.GetByProblemIdAsync(problemId, cancellationToken);
        if (actionPlan is null)
        {
            return null;
        }

        ActionPlanResult content;
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            content = JsonSerializer.Deserialize<ActionPlanResult>(actionPlan.PlanJson, options) ?? new();
        }
        catch
        {
            content = new();
        }

        return new ActionPlanDto
        {
            Id = actionPlan.Id,
            ProblemId = actionPlan.ProblemId,
            Day30 = content.Day30,
            Day60 = content.Day60,
            Day90 = content.Day90,
            SuccessMetrics = content.SuccessMetrics,
            QuickWins = content.QuickWins,
            GeneratedAt = actionPlan.GeneratedAt
        };
    }
}

using System;
using System.Collections.Generic;
using System.Text.Json;
using MSC.IdeaForge.Application.MVPPlans.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.MVPPlans.Queries;

/// <summary>
/// Probleme ait MVP planını DTO olarak getiren sorgu işleyicisi.
/// </summary>
public class GetMVPPlanByProblemHandler
{
    private readonly IMVPPlanRepository _repository;

    public GetMVPPlanByProblemHandler(IMVPPlanRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Probleme ait MVP Planını bulup DTO nesnesine dönüştürerek döner.
    /// </summary>
    public async Task<MVPPlanDto?> HandleAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        var mvpPlan = await _repository.GetByProblemIdAsync(problemId, cancellationToken);
        if (mvpPlan is null)
        {
            return null;
        }

        List<string> userStories = new();
        List<string> sprintPlan = new();

        try
        {
            if (!string.IsNullOrEmpty(mvpPlan.UserStories))
            {
                userStories = JsonSerializer.Deserialize<List<string>>(mvpPlan.UserStories) ?? new();
            }
        }
        catch
        {
            // Parse hatası durumunda boş liste bırakıyoruz
        }

        try
        {
            if (!string.IsNullOrEmpty(mvpPlan.SprintPlan))
            {
                sprintPlan = JsonSerializer.Deserialize<List<string>>(mvpPlan.SprintPlan) ?? new();
            }
        }
        catch
        {
            // Parse hatası durumunda boş liste bırakıyoruz
        }

        return new MVPPlanDto
        {
            Id = mvpPlan.Id,
            ProblemId = mvpPlan.ProblemId,
            Scope = mvpPlan.Scope,
            UserStories = userStories,
            SprintPlan = sprintPlan,
            TechStack = mvpPlan.TechStack,
            Risks = mvpPlan.Risks,
            FirstSalesPlan = mvpPlan.FirstSalesPlan,
            CreatedAt = mvpPlan.CreatedAt,
            UpdatedAt = mvpPlan.UpdatedAt
        };
    }
}

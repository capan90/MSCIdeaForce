using System;
using System.Collections.Generic;
using System.Text.Json;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.MVPPlans.Commands;

/// <summary>
/// MVP planı kaydetme komut nesnesi.
/// </summary>
public class SaveMVPPlanCommand
{
    public Guid ProblemId { get; set; }
    public string? Scope { get; set; }
    public List<string> UserStories { get; set; } = new();
    public List<string> SprintPlan { get; set; } = new();
    public string? TechStack { get; set; }
    public string? Risks { get; set; }
    public string? FirstSalesPlan { get; set; }
}

/// <summary>
/// MVP planı kaydetme komut işleyicisi.
/// </summary>
public class SaveMVPPlanHandler
{
    private readonly IMVPPlanRepository _repository;

    public SaveMVPPlanHandler(IMVPPlanRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Varsa mevcut MVP planını günceller, yoksa yenisini oluşturup veritabanına yansıtır.
    /// </summary>
    public async Task HandleAsync(SaveMVPPlanCommand command, CancellationToken cancellationToken = default)
    {
        var userStoriesJson = JsonSerializer.Serialize(command.UserStories);
        var sprintPlanJson = JsonSerializer.Serialize(command.SprintPlan);

        var mvpPlan = await _repository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        if (mvpPlan is null)
        {
            mvpPlan = MVPPlan.Create(
                command.ProblemId,
                command.Scope,
                userStoriesJson,
                sprintPlanJson,
                command.TechStack,
                command.Risks,
                command.FirstSalesPlan
            );
            await _repository.AddAsync(mvpPlan, cancellationToken);
        }
        else
        {
            mvpPlan.Update(
                command.Scope,
                userStoriesJson,
                sprintPlanJson,
                command.TechStack,
                command.Risks,
                command.FirstSalesPlan
            );
            await _repository.UpdateAsync(mvpPlan, cancellationToken);
        }

        await _repository.SaveChangesAsync(cancellationToken);
    }
}

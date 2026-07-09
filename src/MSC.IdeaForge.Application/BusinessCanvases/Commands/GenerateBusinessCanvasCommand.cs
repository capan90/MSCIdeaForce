using System.Linq;
using MSC.IdeaForge.Application.BusinessCanvases.DTOs;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.BusinessCanvases.Commands;

/// <summary>
/// Bir problem için yapay zeka ile Business Model Canvas üretilmesi komut nesnesi.
/// </summary>
public class GenerateBusinessCanvasCommand
{
    public Guid ProblemId { get; set; }
}

/// <summary>
/// Business Model Canvas üretme komutunu işleyen sınıf.
/// </summary>
public class GenerateBusinessCanvasHandler
{
    private readonly IProblemRepository _problemRepository;
    private readonly ISolutionRepository _solutionRepository;
    private readonly IBusinessCanvasRepository _canvasRepository;
    private readonly IAIProvider _aiProvider;

    public GenerateBusinessCanvasHandler(
        IProblemRepository problemRepository,
        ISolutionRepository solutionRepository,
        IBusinessCanvasRepository canvasRepository,
        IAIProvider aiProvider)
    {
        _problemRepository = problemRepository;
        _solutionRepository = solutionRepository;
        _canvasRepository = canvasRepository;
        _aiProvider = aiProvider;
    }

    /// <summary>
    /// Problemi yapay zekaya göndererek Business Model Canvas üretir ve kaydeder.
    /// </summary>
    public async Task<BusinessCanvasDto> HandleAsync(GenerateBusinessCanvasCommand command, CancellationToken cancellationToken = default)
    {
        var problem = await _problemRepository.GetByIdAsync(command.ProblemId, cancellationToken);
        if (problem is null)
        {
            throw new KeyNotFoundException($"ID'si {command.ProblemId} olan problem bulunamadı.");
        }

        // Önerilen çözüm tipini (varsa) canvas üretimine bağlam olarak veriyoruz
        var solutions = await _solutionRepository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        var solutionType = solutions.FirstOrDefault(s => s.IsRecommended)?.SolutionType.ToString()
                           ?? solutions.FirstOrDefault()?.SolutionType.ToString();

        var aiResult = await _aiProvider.GenerateBusinessCanvasAsync(problem.Title, problem.Description, problem.Sector, solutionType);

        var canvas = await _canvasRepository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        if (canvas is null)
        {
            canvas = BusinessCanvas.Create(
                command.ProblemId,
                aiResult.KeyPartners, aiResult.KeyActivities, aiResult.KeyResources,
                aiResult.ValuePropositions, aiResult.CustomerRelationships, aiResult.Channels,
                aiResult.CustomerSegments, aiResult.CostStructure, aiResult.RevenueStreams);
            await _canvasRepository.AddAsync(canvas, cancellationToken);
        }
        else
        {
            canvas.Update(
                aiResult.KeyPartners, aiResult.KeyActivities, aiResult.KeyResources,
                aiResult.ValuePropositions, aiResult.CustomerRelationships, aiResult.Channels,
                aiResult.CustomerSegments, aiResult.CostStructure, aiResult.RevenueStreams);
            await _canvasRepository.UpdateAsync(canvas, cancellationToken);
        }

        await _canvasRepository.SaveChangesAsync(cancellationToken);
        return BusinessCanvasMapper.ToDto(canvas);
    }
}

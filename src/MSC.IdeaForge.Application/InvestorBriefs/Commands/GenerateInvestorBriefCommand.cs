using System.Linq;
using MSC.IdeaForge.Application.InvestorBriefs.DTOs;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.InvestorBriefs.Commands;

/// <summary>
/// Bir problem için yapay zeka ile yatırımcı briefing üretilmesi komut nesnesi.
/// </summary>
public class GenerateInvestorBriefCommand
{
    public Guid ProblemId { get; set; }
}

/// <summary>
/// Yatırımcı briefing üretme komutunu işleyen sınıf.
/// Problem, fırsat skoru, gelir modeli ve önerilen çözüm tipini toplayıp yapay zekaya iletir.
/// </summary>
public class GenerateInvestorBriefHandler
{
    private readonly IProblemRepository _problemRepository;
    private readonly IOpportunityRepository _opportunityRepository;
    private readonly IRevenueAnalysisRepository _revenueRepository;
    private readonly ISolutionRepository _solutionRepository;
    private readonly IInvestorBriefRepository _briefRepository;
    private readonly IAIProvider _aiProvider;

    public GenerateInvestorBriefHandler(
        IProblemRepository problemRepository,
        IOpportunityRepository opportunityRepository,
        IRevenueAnalysisRepository revenueRepository,
        ISolutionRepository solutionRepository,
        IInvestorBriefRepository briefRepository,
        IAIProvider aiProvider)
    {
        _problemRepository = problemRepository;
        _opportunityRepository = opportunityRepository;
        _revenueRepository = revenueRepository;
        _solutionRepository = solutionRepository;
        _briefRepository = briefRepository;
        _aiProvider = aiProvider;
    }

    /// <summary>
    /// Problemi ve mevcut analiz verilerini toplayıp yapay zekadan yatırımcı briefing üretir ve kaydeder.
    /// </summary>
    public async Task<InvestorBriefDto> HandleAsync(GenerateInvestorBriefCommand command, CancellationToken cancellationToken = default)
    {
        var problem = await _problemRepository.GetByIdAsync(command.ProblemId, cancellationToken);
        if (problem is null)
        {
            throw new KeyNotFoundException($"ID'si {command.ProblemId} olan problem bulunamadı.");
        }

        // Briefing'i zenginleştirmek için mevcut analiz verilerini topluyoruz
        var opportunity = await _opportunityRepository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        var revenue = await _revenueRepository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        var solutions = await _solutionRepository.GetByProblemIdAsync(command.ProblemId, cancellationToken);

        var solutionType = solutions.FirstOrDefault(s => s.IsRecommended)?.SolutionType.ToString()
                           ?? solutions.FirstOrDefault()?.SolutionType.ToString();
        var revenueModel = revenue?.RevenueModel;
        double? opportunityScore = opportunity?.OpportunityScore.TotalScore;

        var aiResult = await _aiProvider.GenerateInvestorBriefAsync(
            problem.Title,
            problem.Description,
            problem.Sector,
            solutionType,
            revenueModel,
            opportunityScore
        );

        // Mevcut kayıt varsa güncelliyor, yoksa yenisini oluşturuyoruz
        var brief = await _briefRepository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        if (brief is null)
        {
            brief = InvestorBrief.Create(
                command.ProblemId,
                aiResult.ExecutiveSummary,
                aiResult.ProblemStatement,
                aiResult.MarketOpportunity,
                aiResult.Solution,
                aiResult.BusinessModel,
                aiResult.CompetitiveAdvantage,
                aiResult.AskAmount,
                aiResult.UseOfFunds,
                aiResult.Traction
            );
            await _briefRepository.AddAsync(brief, cancellationToken);
        }
        else
        {
            brief.Update(
                aiResult.ExecutiveSummary,
                aiResult.ProblemStatement,
                aiResult.MarketOpportunity,
                aiResult.Solution,
                aiResult.BusinessModel,
                aiResult.CompetitiveAdvantage,
                aiResult.AskAmount,
                aiResult.UseOfFunds,
                aiResult.Traction
            );
            await _briefRepository.UpdateAsync(brief, cancellationToken);
        }

        await _briefRepository.SaveChangesAsync(cancellationToken);

        return ToDto(brief);
    }

    private static InvestorBriefDto ToDto(InvestorBrief brief) => new()
    {
        Id = brief.Id,
        ProblemId = brief.ProblemId,
        ExecutiveSummary = brief.ExecutiveSummary,
        ProblemStatement = brief.ProblemStatement,
        MarketOpportunity = brief.MarketOpportunity,
        Solution = brief.Solution,
        BusinessModel = brief.BusinessModel,
        CompetitiveAdvantage = brief.CompetitiveAdvantage,
        AskAmount = brief.AskAmount,
        UseOfFunds = brief.UseOfFunds,
        Traction = brief.Traction,
        GeneratedAt = brief.GeneratedAt
    };
}

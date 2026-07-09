using Microsoft.Extensions.DependencyInjection;
using MSC.IdeaForge.Application.Problems.Commands;
using MSC.IdeaForge.Application.Problems.Queries;
using MSC.IdeaForge.Application.Signals.Commands;
using MSC.IdeaForge.Application.Signals.Queries;
using MSC.IdeaForge.Application.Opportunities.Commands;
using MSC.IdeaForge.Application.Opportunities.Queries;
using MSC.IdeaForge.Application.Validations.Commands;
using MSC.IdeaForge.Application.Validations.Queries;
using MSC.IdeaForge.Application.Dashboard;
using MSC.IdeaForge.Application.Researches.Commands;
using MSC.IdeaForge.Application.Researches.Queries;
using MSC.IdeaForge.Application.Solutions.Commands;
using MSC.IdeaForge.Application.Solutions.Queries;
using MSC.IdeaForge.Application.MVPPlans.Commands;
using MSC.IdeaForge.Application.MVPPlans.Queries;
using MSC.IdeaForge.Application.Knowledge.Commands;
using MSC.IdeaForge.Application.Knowledge.Queries;
using MSC.IdeaForge.Application.Agents;
using MSC.IdeaForge.Application.Signals.Services;
using MSC.IdeaForge.Application.Decisions.Commands;
using MSC.IdeaForge.Application.Decisions.Queries;

namespace MSC.IdeaForge.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateProblemHandler>();
        services.AddScoped<GetProblemsHandler>();
        // Problem detayı sorgu işleyicisini bağımlılık enjeksiyonuna ekliyoruz
        services.AddScoped<GetProblemByIdHandler>();
        // Silme ve durum güncelleme komut işleyicilerini ekliyoruz
        services.AddScoped<DeleteProblemHandler>();
        services.AddScoped<UpdateProblemStatusHandler>();
        services.AddScoped<UpdateProblemHandler>();

        // Sinyal ekleme ve sorgulama işleyicilerini ekliyoruz
        services.AddScoped<CreateSignalHandler>();
        services.AddScoped<GetSignalsByProblemHandler>();

        // Problem yapay zeka analizi komut işleyicisini ekliyoruz
        services.AddScoped<AnalyzeProblemHandler>();
        services.AddScoped<GetProblemAnalysisHandler>();

        // Fırsat puanlama ve sorgulama işleyicilerini ekliyoruz
        services.AddScoped<ScoreOpportunityHandler>();
        services.AddScoped<GetOpportunityByProblemHandler>();
        services.AddScoped<AIScoreOpportunityHandler>();

        // Doğrulama işleyicilerini ekliyoruz
        services.AddScoped<SaveValidationHandler>();
        services.AddScoped<GetValidationByProblemHandler>();

        // Dashboard sorgulama işleyicisini ekliyoruz
        services.AddScoped<DashboardQueryHandler>();

        // Araştırma işleyicilerini ekliyoruz
        services.AddScoped<SaveResearchHandler>();
        services.AddScoped<AIResearchHandler>();
        services.AddScoped<GetResearchByProblemHandler>();

        // Çözüm önerisi işleyicilerini ekliyoruz
        services.AddScoped<SaveSolutionHandler>();
        services.AddScoped<DeleteSolutionHandler>();
        services.AddScoped<AISuggestSolutionsHandler>();
        services.AddScoped<GetSolutionsByProblemHandler>();

        // MVP Planı işleyicilerini ekliyoruz
        services.AddScoped<SaveMVPPlanHandler>();
        services.AddScoped<GenerateMVPPlanHandler>();
        services.AddScoped<GetMVPPlanByProblemHandler>();

        // Bilgi Bankası işleyicilerini ekliyoruz
        services.AddScoped<CreateKnowledgeHandler>();
        services.AddScoped<DeleteKnowledgeHandler>();
        services.AddScoped<GetAllKnowledgeHandler>();
        services.AddScoped<UpdateKnowledgeHandler>();

        // AI Ajan Koordinatörü
        services.AddScoped<AgentCoordinator>();

        // Sinyal İçe Aktarım işleyicileri
        services.AddScoped<ImportRSSHandler>();
        services.AddScoped<ProcessImportedSignalHandler>();
        services.AddScoped<GetImportedSignalsHandler>();
        services.AddScoped<RSSImporter>();

        // Karar Günlüğü işleyicileri
        services.AddScoped<CreateDecisionHandler>();
        services.AddScoped<GetDecisionsByProblemHandler>();
        services.AddScoped<GetAllDecisionsHandler>();

        return services;
    }
}

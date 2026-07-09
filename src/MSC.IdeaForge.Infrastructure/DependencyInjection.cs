using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSC.IdeaForge.Domain.Interfaces;
using MSC.IdeaForge.Infrastructure.AI;
using MSC.IdeaForge.Infrastructure.Persistence;
using MSC.IdeaForge.Infrastructure.Persistence.Repositories;

namespace MSC.IdeaForge.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProblemRepository, ProblemRepository>();
        services.AddScoped<ISignalRepository, SignalRepository>();
        services.AddScoped<IOpportunityRepository, OpportunityRepository>();
        services.AddScoped<IValidationRepository, ValidationRepository>();
        services.AddScoped<IProblemAnalysisRepository, ProblemAnalysisRepository>();
        services.AddScoped<IResearchRepository, ResearchRepository>();
        services.AddScoped<ISolutionRepository, SolutionRepository>();
        services.AddScoped<IMVPPlanRepository, MVPPlanRepository>();
        services.AddScoped<IKnowledgeRepository, KnowledgeRepository>();

        // HttpClient ve GeminiAIProvider bağımlılıklarını ekliyoruz
        services.AddScoped<HttpClient>();
        services.AddScoped<IAIProvider, GeminiAIProvider>();

        return services;
    }
}

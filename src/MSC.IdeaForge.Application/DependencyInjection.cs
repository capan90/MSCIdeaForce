using Microsoft.Extensions.DependencyInjection;
using MSC.IdeaForge.Application.Problems.Commands;
using MSC.IdeaForge.Application.Problems.Queries;

namespace MSC.IdeaForge.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateProblemHandler>();
        services.AddScoped<GetProblemsHandler>();
        // Problem detayı sorgu işleyicisini bağımlılık enjeksiyonuna ekliyoruz
        services.AddScoped<GetProblemByIdHandler>();
        return services;
    }
}

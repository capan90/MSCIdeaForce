using Microsoft.Extensions.DependencyInjection;
using MSC.IdeaForge.Application.Problems.Commands;
using MSC.IdeaForge.Application.Problems.Queries;
using MSC.IdeaForge.Application.Signals.Commands;
using MSC.IdeaForge.Application.Signals.Queries;

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

        // Sinyal ekleme ve sorgulama işleyicilerini ekliyoruz
        services.AddScoped<CreateSignalHandler>();
        services.AddScoped<GetSignalsByProblemHandler>();

        // Problem yapay zeka analizi komut işleyicisini ekliyoruz
        services.AddScoped<AnalyzeProblemHandler>();

        return services;
    }
}

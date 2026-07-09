using System;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Enums;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Signals.Commands;

/// <summary>
/// İçe aktarılmış ham sinyali işlemden geçirme komut nesnesi.
/// </summary>
public class ProcessImportedSignalCommand
{
    public Guid ImportedSignalId { get; set; }
    public Guid? ProblemId { get; set; } // null ise yeni bir problem oluşturur
}

/// <summary>
/// Ham sinyali yeni veya mevcut bir probleme bağlayan/dönüştüren komut işleyicisi.
/// </summary>
public class ProcessImportedSignalHandler
{
    private readonly IImportedSignalRepository _importedSignalRepository;
    private readonly IProblemRepository _problemRepository;
    private readonly ISignalRepository _signalRepository;
    private readonly IProblemAnalysisRepository _analysisRepository;
    private readonly IAIProvider _aiProvider;

    public ProcessImportedSignalHandler(
        IImportedSignalRepository importedSignalRepository,
        IProblemRepository problemRepository,
        ISignalRepository signalRepository,
        IProblemAnalysisRepository analysisRepository,
        IAIProvider aiProvider)
    {
        _importedSignalRepository = importedSignalRepository;
        _problemRepository = problemRepository;
        _signalRepository = signalRepository;
        _analysisRepository = analysisRepository;
        _aiProvider = aiProvider;
    }

    /// <summary>
    /// Ham sinyali analiz eder, seçime göre yeni problem oluşturur veya mevcut probleme kaynak sinyal olarak bağlar.
    /// </summary>
    public async Task HandleAsync(ProcessImportedSignalCommand command, CancellationToken cancellationToken = default)
    {
        var allImported = await _importedSignalRepository.GetAllAsync(cancellationToken);
        var importedSignal = allImported.FirstOrDefault(s => s.Id == command.ImportedSignalId);

        if (importedSignal is null)
        {
            throw new KeyNotFoundException($"ID'si {command.ImportedSignalId} olan ham sinyal bulunamadı.");
        }

        if (importedSignal.IsProcessed)
        {
            throw new InvalidOperationException("Bu sinyal zaten işlenmiş.");
        }

        if (command.ProblemId.HasValue && command.ProblemId.Value != Guid.Empty)
        {
            // 1. Mevcut probleme sinyal olarak bağlama
            var existingProblem = await _problemRepository.GetByIdAsync(command.ProblemId.Value, cancellationToken);
            if (existingProblem is null)
            {
                throw new KeyNotFoundException($"ID'si {command.ProblemId.Value} olan problem bulunamadı.");
            }

            var newSignal = Signal.Create(
                command.ProblemId.Value,
                importedSignal.Title,
                string.IsNullOrEmpty(importedSignal.Summary) ? importedSignal.RawContent : importedSignal.Summary,
                SignalType.Manual,
                SeverityLevel.Medium
            );

            await _signalRepository.AddAsync(newSignal, cancellationToken);
            await _signalRepository.SaveChangesAsync(cancellationToken);

            importedSignal.MarkAsProcessed(command.ProblemId.Value);
            await _importedSignalRepository.UpdateAsync(importedSignal, cancellationToken);
        }
        else
        {
            // 2. Yeni problem oluşturma ve AI ile otomatik analiz
            string desc = string.IsNullOrEmpty(importedSignal.RawContent)
                ? (importedSignal.Summary ?? importedSignal.Title)
                : importedSignal.RawContent;

            var newProblem = Problem.Create(
                importedSignal.Title,
                desc,
                source: importedSignal.SourceUrl
            );

            // Yeni problemi kaydediyoruz
            await _problemRepository.AddAsync(newProblem, cancellationToken);
            await _problemRepository.SaveChangesAsync(cancellationToken);

            // Yapay zeka ile otomatik ön analiz gerçekleştiriyoruz (Coordinator'dan bağımsız, temel analiz)
            try
            {
                var analysisResult = await _aiProvider.AnalyzeProblemAsync(
                    newProblem.Title,
                    newProblem.Description,
                    newProblem.Sector
                );

                var problemAnalysis = ProblemAnalysis.Create(
                    newProblem.Id,
                    analysisResult.Summary,
                    analysisResult.SuggestedCategory,
                    analysisResult.SuggestedTags,
                    analysisResult.RiskLevel,
                    analysisResult.SolutionTypeSuggestion,
                    analysisResult.ConfidenceScore
                );

                await _analysisRepository.AddAsync(problemAnalysis, cancellationToken);
                await _analysisRepository.SaveChangesAsync(cancellationToken);

                // Problemin durumunu analiz edildi olarak güncelliyoruz
                newProblem.ChangeStatus(ProblemStatus.Analyzed);
                await _problemRepository.UpdateAsync(newProblem, cancellationToken);
                await _problemRepository.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                // Yapay zeka adımı hata verirse süreci kesmiyoruz, problemi taslak durumunda bırakıyoruz.
            }

            importedSignal.MarkAsProcessed(newProblem.Id);
            await _importedSignalRepository.UpdateAsync(importedSignal, cancellationToken);
        }

        await _importedSignalRepository.SaveChangesAsync(cancellationToken);
    }
}

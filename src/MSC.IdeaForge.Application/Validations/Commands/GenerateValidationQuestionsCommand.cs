using System.Text.Json;
using MSC.IdeaForge.Application.Validations.DTOs;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Enums;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Validations.Commands;

/// <summary>
/// Bir problem için yapay zeka ile doğrulama soruları üretilmesi komut nesnesi.
/// </summary>
public class GenerateValidationQuestionsCommand
{
    public Guid ProblemId { get; set; }
}

/// <summary>
/// Yapay zeka ile doğrulama sorularını üretip mevcut doğrulama kaydına kaydeden işleyici.
/// </summary>
public class GenerateValidationQuestionsHandler
{
    private readonly IProblemRepository _problemRepository;
    private readonly IValidationRepository _validationRepository;
    private readonly IAIProvider _aiProvider;

    public GenerateValidationQuestionsHandler(
        IProblemRepository problemRepository,
        IValidationRepository validationRepository,
        IAIProvider aiProvider)
    {
        _problemRepository = problemRepository;
        _validationRepository = validationRepository;
        _aiProvider = aiProvider;
    }

    /// <summary>
    /// Problemi Gemini AI'ye göndererek doğrulama sorularını üretir ve doğrulama kaydına yazar.
    /// </summary>
    public async Task<List<ValidationQuestionDto>> HandleAsync(GenerateValidationQuestionsCommand command, CancellationToken cancellationToken = default)
    {
        var problem = await _problemRepository.GetByIdAsync(command.ProblemId, cancellationToken);
        if (problem is null)
        {
            throw new KeyNotFoundException($"ID'si {command.ProblemId} olan problem bulunamadı.");
        }

        // Yapay zekadan doğrulama sorularını alıyoruz
        var questions = await _aiProvider.GenerateValidationQuestionsAsync(problem.Title, problem.Description);

        var questionDtos = questions
            .Where(q => !string.IsNullOrWhiteSpace(q))
            .Select(q => new ValidationQuestionDto { Question = q, IsAsked = false })
            .ToList();

        var json = JsonSerializer.Serialize(questionDtos);

        // Mevcut doğrulama kaydına yazıyoruz; yoksa minimal geçerli değerlerle yeni kayıt oluşturuyoruz
        var validation = await _validationRepository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        if (validation is null)
        {
            validation = Validation.Create(command.ProblemId, 0, 0, 0, 1, null, null, ValidationStatus.NotStarted);
            validation.SetValidationQuestions(json);
            await _validationRepository.AddAsync(validation, cancellationToken);
        }
        else
        {
            validation.SetValidationQuestions(json);
            await _validationRepository.UpdateAsync(validation, cancellationToken);
        }

        await _validationRepository.SaveChangesAsync(cancellationToken);

        return questionDtos;
    }
}

/// <summary>
/// Doğrulama sorularının güncel durumunu (soruldu mu bilgisi) kaydeden komut nesnesi.
/// </summary>
public class SaveValidationQuestionsCommand
{
    public Guid ProblemId { get; set; }
    public List<ValidationQuestionDto> Questions { get; set; } = new();
}

/// <summary>
/// Doğrulama sorularının güncel durumunu mevcut doğrulama kaydına yazan işleyici.
/// </summary>
public class SaveValidationQuestionsHandler
{
    private readonly IValidationRepository _validationRepository;

    public SaveValidationQuestionsHandler(IValidationRepository validationRepository)
    {
        _validationRepository = validationRepository;
    }

    /// <summary>
    /// Soruların güncel (checkbox) durumunu JSON olarak doğrulama kaydına kaydeder.
    /// </summary>
    public async Task HandleAsync(SaveValidationQuestionsCommand command, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(command.Questions);

        var validation = await _validationRepository.GetByProblemIdAsync(command.ProblemId, cancellationToken);
        if (validation is null)
        {
            validation = Validation.Create(command.ProblemId, 0, 0, 0, 1, null, null, ValidationStatus.NotStarted);
            validation.SetValidationQuestions(json);
            await _validationRepository.AddAsync(validation, cancellationToken);
        }
        else
        {
            validation.SetValidationQuestions(json);
            await _validationRepository.UpdateAsync(validation, cancellationToken);
        }

        await _validationRepository.SaveChangesAsync(cancellationToken);
    }
}

using MSC.IdeaForge.Application.Validations.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Validations.Queries;

/// <summary>
/// Belirtilen probleme ait doğrulama verilerini getiren sorgu işleyicisi.
/// </summary>
public class GetValidationByProblemHandler
{
    private readonly IValidationRepository _repository;

    public GetValidationByProblemHandler(IValidationRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Probleme ait doğrulama kaydını DTO formatında döner.
    /// </summary>
    public async Task<ValidationDto?> HandleAsync(Guid problemId, CancellationToken cancellationToken = default)
    {
        var validation = await _repository.GetByProblemIdAsync(problemId, cancellationToken);
        if (validation is null)
        {
            return null;
        }

        return new ValidationDto
        {
            Id = validation.Id,
            ProblemId = validation.ProblemId,
            InterviewCount = validation.InterviewCount,
            ValidatedUserCount = validation.ValidatedUserCount,
            WillingToPayCount = validation.WillingToPayCount,
            ConfidenceScore = validation.ConfidenceScore,
            Notes = validation.Notes,
            Risks = validation.Risks,
            Status = validation.Status.ToString(),
            CreatedAt = validation.CreatedAt,
            ValidationQuestions = validation.ValidationQuestions
        };
    }
}

namespace MSC.IdeaForge.Application.Validations.DTOs;

/// <summary>
/// Yapay zeka tarafından üretilen tek bir doğrulama sorusunu ve sorulup sorulmadığı bilgisini taşıyan DTO.
/// </summary>
public class ValidationQuestionDto
{
    public string Question { get; set; } = string.Empty;

    // Bu soru potansiyel müşterilere soruldu mu?
    public bool IsAsked { get; set; }
}

using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Enums;

namespace MSC.IdeaForge.Tests;

public class ValidationTests
{
    [Fact]
    public void Create_WithValidData_ShouldCalculateCorrectScores()
    {
        // Arrange
        var problemId = Guid.NewGuid();
        var interviewCount = 20;
        var validatedUserCount = 15;
        var willingToPayCount = 8;
        var confidenceScore = (int)Math.Round((double)validatedUserCount / interviewCount * 100);
        var notes = "Çiftçilerle yapılan görüşmelerde problem doğrulandı";
        var risks = "Rakip ürünler mevcut, fiyat hassasiyeti yüksek";
        var status = ValidationStatus.Validated;

        // Act
        var validation = Validation.Create(
            problemId,
            interviewCount,
            validatedUserCount,
            willingToPayCount,
            confidenceScore,
            notes,
            risks,
            status
        );

        // Assert
        Assert.Equal(problemId, validation.ProblemId);
        Assert.Equal(20, validation.InterviewCount);
        Assert.Equal(15, validation.ValidatedUserCount);
        Assert.Equal(8, validation.WillingToPayCount);
        Assert.Equal(75, validation.ConfidenceScore); // 15 / 20 * 100 = 75
        Assert.Equal(notes, validation.Notes);
        Assert.Equal(risks, validation.Risks);
        Assert.Equal(ValidationStatus.Validated, validation.Status);
    }
}

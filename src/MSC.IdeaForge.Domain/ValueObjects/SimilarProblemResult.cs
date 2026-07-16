using System;

namespace MSC.IdeaForge.Domain.ValueObjects;

public class SimilarProblemResult
{
    public Guid ProblemId { get; set; }
    public string Title { get; set; } = string.Empty;
    public double SimilarityScore { get; set; }
    public string Reason { get; set; } = string.Empty;
}

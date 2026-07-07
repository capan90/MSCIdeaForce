namespace MSC.IdeaForge.Application.Problems.DTOs;

public class ProblemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string? Sector { get; set; }
    public string? Tags { get; set; }
    public string? Source { get; set; }
    public DateTime CreatedAt { get; set; }
}

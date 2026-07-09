using System;

namespace MSC.IdeaForge.Application.Automation.DTOs;

public class RSSFeedDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? LastScannedAt { get; set; }
    public int LastSignalCount { get; set; }
}

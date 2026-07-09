using System;

namespace MSC.IdeaForge.Domain.Entities;

public class RSSFeed : BaseEntity
{
    public string Url { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime? LastScannedAt { get; set; }
    public int LastSignalCount { get; set; }

    public RSSFeed()
    {
    }

    public RSSFeed(string url, string name, bool isActive = true)
    {
        Url = url;
        Name = name;
        IsActive = isActive;
    }

    public void UpdateScanInfo(int newSignalCount)
    {
        LastScannedAt = DateTime.UtcNow;
        LastSignalCount = newSignalCount;
        SetUpdated();
    }
}

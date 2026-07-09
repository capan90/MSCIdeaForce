namespace MSC.IdeaForge.Application.Automation.DTOs;

public class EmailSettingDto
{
    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 587;
    public string SmtpUser { get; set; } = string.Empty;
    public string SmtpPassword { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = "MSC IdeaForge";
    public string ReceiverEmail { get; set; } = string.Empty;
    public bool DailySummaryEnabled { get; set; }
}

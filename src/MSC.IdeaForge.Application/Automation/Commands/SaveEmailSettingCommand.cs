using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Automation.Commands;

public class SaveEmailSettingCommand
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

public class SaveEmailSettingHandler
{
    private readonly IEmailSettingRepository _repository;

    public SaveEmailSettingHandler(IEmailSettingRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(SaveEmailSettingCommand command, CancellationToken cancellationToken = default)
    {
        var setting = new EmailSetting
        {
            SmtpHost = command.SmtpHost,
            SmtpPort = command.SmtpPort,
            SmtpUser = command.SmtpUser,
            SmtpPassword = command.SmtpPassword,
            FromEmail = command.FromEmail,
            FromName = command.FromName,
            ReceiverEmail = command.ReceiverEmail,
            DailySummaryEnabled = command.DailySummaryEnabled
        };

        await _repository.SaveSettingAsync(setting, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}

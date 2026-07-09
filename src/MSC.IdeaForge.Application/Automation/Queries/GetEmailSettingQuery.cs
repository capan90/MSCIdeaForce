using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Application.Automation.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Automation.Queries;

public class GetEmailSettingQuery
{
}

public class GetEmailSettingHandler
{
    private readonly IEmailSettingRepository _repository;

    public GetEmailSettingHandler(IEmailSettingRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmailSettingDto> HandleAsync(GetEmailSettingQuery query, CancellationToken cancellationToken = default)
    {
        var setting = await _repository.GetSettingAsync(cancellationToken);
        if (setting == null)
        {
            return new EmailSettingDto();
        }

        return new EmailSettingDto
        {
            SmtpHost = setting.SmtpHost,
            SmtpPort = setting.SmtpPort,
            SmtpUser = setting.SmtpUser,
            SmtpPassword = setting.SmtpPassword,
            FromEmail = setting.FromEmail,
            FromName = setting.FromName,
            ReceiverEmail = setting.ReceiverEmail,
            DailySummaryEnabled = setting.DailySummaryEnabled
        };
    }
}

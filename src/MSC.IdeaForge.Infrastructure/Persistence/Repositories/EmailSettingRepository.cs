using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Persistence.Repositories;

public class EmailSettingRepository : IEmailSettingRepository
{
    private readonly AppDbContext _context;

    public EmailSettingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EmailSetting?> GetSettingAsync(CancellationToken cancellationToken = default)
        => await _context.EmailSettings.FirstOrDefaultAsync(cancellationToken);

    public async Task SaveSettingAsync(EmailSetting setting, CancellationToken cancellationToken = default)
    {
        var existing = await GetSettingAsync(cancellationToken);
        if (existing == null)
        {
            await _context.EmailSettings.AddAsync(setting, cancellationToken);
        }
        else
        {
            existing.SmtpHost = setting.SmtpHost;
            existing.SmtpPort = setting.SmtpPort;
            existing.SmtpUser = setting.SmtpUser;
            existing.SmtpPassword = setting.SmtpPassword;
            existing.FromEmail = setting.FromEmail;
            existing.FromName = setting.FromName;
            existing.DailySummaryEnabled = setting.DailySummaryEnabled;
            existing.SetUpdated();
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}

using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

public interface IEmailSettingRepository
{
    Task<EmailSetting?> GetSettingAsync(CancellationToken cancellationToken = default);
    Task SaveSettingAsync(EmailSetting setting, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

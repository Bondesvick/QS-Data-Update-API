using System.Threading.Tasks;
using QSDataUpdateAPI.Domain.Models.Requests.Redbox;

namespace QSDataUpdateAPI.Domain.Services.RedboxServiceProxies.Interfaces
{
    public interface IRedboxEmailService
    {
        Task<BaseRedboxResponse> SendEmailAsync(RedboxEmailMessageModel mailMessage);
    }
}
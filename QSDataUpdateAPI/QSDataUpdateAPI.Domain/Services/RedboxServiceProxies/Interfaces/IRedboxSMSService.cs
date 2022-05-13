using QSDataUpdateAPI.Domain.Models.Requests;
using System.Threading.Tasks;
using QSDataUpdateAPI.Domain.Models.Requests.Redbox;

namespace QSDataUpdateAPI.Domain.Services
{
    public interface IRedboxSMSService
    {
        Task<BaseRedboxResponse> SendSMSAsync(string phoneNumber, string message, string acctNumber = "");
    }
}
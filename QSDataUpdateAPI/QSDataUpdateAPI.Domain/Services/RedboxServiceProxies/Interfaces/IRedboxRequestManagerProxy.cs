using System.Threading.Tasks;
using QSDataUpdateAPI.Domain.Models.Requests;
using QSDataUpdateAPI.Domain.Models.Requests.Redbox;

namespace QSDataUpdateAPI.Domain.Services.RedboxServiceProxies.Interfaces
{
    public interface IRedboxRequestManagerProxy
    {
        Task<BaseRequestManagerResponse<T2>> Post<T2>(string xmlReq, string module = "1", string authId = "1") where T2 : class;
    }
}
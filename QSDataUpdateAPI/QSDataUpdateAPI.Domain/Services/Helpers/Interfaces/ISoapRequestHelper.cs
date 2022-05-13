using System.Threading.Tasks;
using QSDataUpdateAPI.Domain.Models.Requests;
using QSDataUpdateAPI.Domain.Models.Requests.Redbox;

namespace QSDataUpdateAPI.Domain.Services.Helpers.Interfaces
{
    public interface ISoapRequestHelper
    {
        Task<BaseRedboxResponse> SoapCall(string soapRequest, string soapAction, string url, string moduleId = "", string authId = "", string contenttype = "text/xml");
    }
}
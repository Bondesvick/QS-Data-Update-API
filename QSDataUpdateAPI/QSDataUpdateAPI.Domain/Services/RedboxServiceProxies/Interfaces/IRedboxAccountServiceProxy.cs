using System.Collections.Generic;
using System.Threading.Tasks;
using QSDataUpdateAPI.Domain.Models.Requests;
using QSDataUpdateAPI.Domain.Models.Response;
using QSDataUpdateAPI.Domain.Models.Response.Redbox;

namespace QSDataUpdateAPI.Domain.Services.RedboxServiceProxies.Interfaces
{
    public interface IRedboxAccountServiceProxy
    {
        Task<(string responseCode, string responseDescription, CustomerAccountInfo result)> GetCustomerAccountDetails(string accountNumber, string phoneNumber);

        Task<(string responseCode, string responseDescription, string segment)> GetAccountSegment(string accountNumber);

        Task<(string responseCode, string responseDescription, CustomerAccountInfo result)> DoCorporateAccountEnquiry(string accountNumber);

        Task<(string responseCode, string responseDescription, AccountEnquiryInfo result)> DoAccountCIFEnquiry(string accountNumber);

        Task<(string responseCode, string responseDescription, CustomerAccountInfo result)> DoBVNEnquiry(string bvnId);

        Task<(string responseCode, string responseDescription, TinEnquiryInfo result)> DoTinEnquiry(string tinNumber);

        Task<(string responseCode, string responseDescription)> DoStraightThroughSave(DataUpdateRequest model, AccountEnquiryInfo cifInfo, string accountNumber);

        Task<List<CityState>> GetCityState();
    }
}
using QSDataUpdateAPI.Core.Domain.Entities;
using QSDataUpdateAPI.Domain.Models.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QSDataUpdateAPI.Domain.Services
{
    public interface ICustomerRequestService
    {
        Task<(bool status, string statusMessage, string result)> VerifyAndSaveAdditionalAccountOpeningRequest(DataUpdateRequest request);
        Task<(bool status, string statusMessage, string result)> SaveAndContinue(DataUpdateRequest request, string status);
        Task<(bool status, string statusMessage, DataUpdateDetails result)> VerifyCaseId(string caseId);
        Task<object> GetAccountOpeningRequest(int requestId);
        Task<IEnumerable<object>> GetAccountOpeningRequests();
    }
}

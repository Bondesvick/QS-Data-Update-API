using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QSDataUpdateAPI.Core.Domain.Entities;
using QSDataUpdateAPI.Core.Interfaces.Services.Helpers.Redbox;
using QSDataUpdateAPI.Data.Repositories;
using QSDataUpdateAPI.Domain.Models.Requests;
using QSDataUpdateAPI.Domain.Models.Requests.Redbox;
using QSDataUpdateAPI.Domain.Services.Helpers.Interfaces;
using QSDataUpdateAPI.Domain.Services.RedboxServiceProxies.Interfaces;

namespace QSDataUpdateAPI.Domain.Services
{
    public class CustomerRequestService : ICustomerRequestService
    {
        private readonly IRepository<CustomerRequest, long> _customerRequestRepository;
        private readonly IRepository<DataUpdateDetails, int> _accOpeningRepository;
        private readonly ICustomerRequestDataRepository _customerRequestDataRepository;
        private readonly IRedboxOtpServiceProxy _redboxOtpServiceProxy;
        private readonly IAppLogger _logger;
        private readonly IRedboxEmailService _emailServiceProxy;
        private readonly IRedboxSMSService _smsServiceProxy;
        private readonly IAppSettings _configSettings;
        private readonly IRedboxAccountServiceProxy _accountServiceProxy;

        public CustomerRequestService(IRepository<CustomerRequest, long> cusReqRepo, IAppLogger appLogger, IRedboxOtpServiceProxy otpServiceProxy
            , IRedboxEmailService emailService, IAppSettings settings, IRedboxSMSService smsService, IRedboxAccountServiceProxy accountServiceProxy,
            IRepository<DataUpdateDetails, int> accOpeningRepository, ICustomerRequestDataRepository customerRequestDataRepository)
        {
            _customerRequestRepository = cusReqRepo;
            _logger = appLogger;
            _redboxOtpServiceProxy = otpServiceProxy;
            _emailServiceProxy = emailService;
            _smsServiceProxy = smsService;
            _accountServiceProxy = accountServiceProxy;
            _configSettings = settings;
            _accOpeningRepository = accOpeningRepository;
            _customerRequestDataRepository = customerRequestDataRepository;
        }

        public async Task<(bool status, string statusMessage, string result)> VerifyAndSaveAdditionalAccountOpeningRequest(DataUpdateRequest request)
        {
            try
            {
                var dataToupdate = request.DataToUpdate.Split(',');
                //var otpVerificationResponse = await _redboxOtpServiceProxy.VerifyOtp(new RedboxOtpVerificationRequest(request.OtpSourceReference, request.OtpReasonCode));
                if (!request.ExistingAccountType.Equals("Business Account") && ((dataToupdate.Length == 1 && !dataToupdate.Contains("Tin Update")) || dataToupdate.Length > 1) && request.AuthType.Equals("signature", StringComparison.OrdinalIgnoreCase))
                {
                    var otpVerificationResponse = await _redboxOtpServiceProxy.VerifyOtpReqManager(request.OtpIdentifier, request.Otp, request.OtpSourceReference);
                    if (otpVerificationResponse.responseCode != "00")
                        return (false, $"Otp verification failed. Check that {request.Otp} is correct and not expired and retry: {otpVerificationResponse.responseDescription}", otpVerificationResponse.responseDescription);
                }
                //await _accountServiceProxy.DoStraightThroughSave(request);
                return await SaveAdditionalAccountOpeningRequest(request);
            }
            catch (Exception exception)
            {
                _logger.Error($"Error occured while saving data update request", exception.Message, exception);
                return (false, "An error was encountered while saving customer request", null);
            }
        }

        public async Task<object> GetAccountOpeningRequest(int requestId)
        {
            try
            {
                return await _customerRequestRepository.GetItem(requestId);
            }
            catch (Exception exception)
            {
                _logger.Error("An Error occured while retrieving data update request", exception.Message, exception);
                throw;
            }
        }

        public async Task<IEnumerable<object>> GetAccountOpeningRequests()
        {
            try
            {
                return await _customerRequestRepository.GetItems();
            }
            catch (Exception exception)
            {
                _logger.Error($"An error occured while retrieving account requests: {exception.Message}", exception.Message, exception);
                throw;
            }
        }

        private async Task<(bool status, string statusMessage, string result)> SaveAdditionalAccountOpeningRequest(DataUpdateRequest request)
        {
            try
            {
                string @status = "";

                #region UPDATE PHONE AND EMAIL ON FINACLE

                //// Update phone and email on finacle
                //request.NewPhoneNumber = "08165585587";
                //request.NewEmail = "bondesvick@gmail.com";
                if (!string.IsNullOrWhiteSpace(request.NewEmail) || !string.IsNullOrWhiteSpace(request.NewPhoneNumber))
                {
                    var cifInfo = await _accountServiceProxy.DoAccountCIFEnquiry(request.ExistingAccount);
                    var updatePhoneAndEmailResponse = await _accountServiceProxy.DoStraightThroughSave(request, cifInfo.result, request.ExistingAccount);

                    if (updatePhoneAndEmailResponse.responseCode != "00")
                        return (false, $"Your data update request was not successful.", updatePhoneAndEmailResponse.responseDescription);

                    @status = "RESOLVED";
                }

                #endregion UPDATE PHONE AND EMAIL ON FINACLE

                if (!string.IsNullOrWhiteSpace(request.IdType.Trim()) || !string.IsNullOrWhiteSpace(request.IdNumber.Trim())
                        || !string.IsNullOrWhiteSpace(request.HouseNumber.Trim()) || !string.IsNullOrWhiteSpace(request.HouseDescription.Trim())
                        || !string.IsNullOrWhiteSpace(request.StreetName.Trim()) || !string.IsNullOrWhiteSpace(request.CityTown.Trim())
                        || !string.IsNullOrWhiteSpace(request.State.Trim()) || !string.IsNullOrWhiteSpace(request.Lga.Trim())
                        || !string.IsNullOrWhiteSpace(request.BusStop.Trim()) || !string.IsNullOrWhiteSpace(request.Alias.Trim())
                         || !string.IsNullOrWhiteSpace(request.TinNumber.Trim()))
                {
                    @status = "PENDING";
                    //request.NewEmail = "";
                    //request.NewPhoneNumber = "";
                    //request.CountryCode = "";
                }

                //var cifI = await _accountServiceProxy.DoAccountCIFEnquiry(request.ExistingAccount);
                var saveFinalSubmission = await SaveAndContinue(request, @status);
                if (!saveFinalSubmission.status)
                    return (false, "Could not submit data update request at this time. please retry after some time", string.Empty);

                // Send email
#pragma warning disable
                Task.Factory.StartNew(async () =>
                {
                    (string firstname, string phoneNo, string emailAddress) = await GetAccountDetailsAsync(request.ExistingAccount);

                    if (!string.IsNullOrEmpty(phoneNo) || !string.IsNullOrEmpty(emailAddress))
                        await SendSubmissionNotificationMessageAsync(firstname, saveFinalSubmission.result, phoneNo, emailAddress, "Submit");
                });
#pragma warning restore

                return (true, "Request saved successfully", saveFinalSubmission.result);
            }
            catch (Exception exception)
            {
                var xx = exception;
                throw;
            }
        }

        private async Task SendSubmissionNotificationMessageAsync(string firstname, string ticketId, string phoneNo, string email, string task)
        {
            try
            {
                string message = "";

                switch (task)
                {
                    case ("SaveAndContinue"):
                        message = _configSettings.GetString("AppSettings:SaveAndContinueMessage");
                        // message = @" <br><br>Dear #FirstName#,<br><br> your data update progress has been successfully saved with Ticket ID #TicketId#.<br><br> Please keep this ID safe for future reference.<br><br> For further enquiries, contact the Customer Contact Centre on 0700 909 909 909 or CustomerCareNigeria@Stanbicibtc.com.<br><br> Thank you";
                        break;

                    case ("Submit"):
                        message = _configSettings.GetString("AppSettings:SubmissionMessage");
                        break;
                }

                if (!string.IsNullOrEmpty(message))
                {
                    if (!string.IsNullOrEmpty(firstname))
                    {
                        var name = firstname.ToLower();
                        message = message.Replace("#FirstName#", char.ToUpper(name[0]) + name.Substring(1)).Replace("#TicketId#", ticketId).Replace("?", " ");
                    }
                    else
                        message = message.Replace("#FirstName#", "customer").Replace("#TicketId#", ticketId);

                    if (!string.IsNullOrEmpty(email))
                        await SendNotificationEmailAsync(email, message);

                    if (!string.IsNullOrEmpty(phoneNo))
                        await SendNotificationSMSAsync(phoneNo, message);
                }
            }
            catch (Exception exception)
            {
                _logger.Error($"Error occured while sending notification message -> {exception.Message}", exception);
            }
        }

        private async Task SendNotificationEmailAsync(string emailAddress, string message)
        {
            var mailMessage = ComposeEmailMessage(emailAddress, message);
            var emailResponse = await _emailServiceProxy.SendEmailAsync(mailMessage);
            _logger.Info($"Email Response for {emailAddress} -> {emailResponse}");
        }

        private async Task SendNotificationSMSAsync(string phoneNumber, string message, string accountNumber = null)
        {
            var response = await _smsServiceProxy.SendSMSAsync(phoneNumber, message, accountNumber);
            _logger.Info($"SMS Response for {phoneNumber} -> {response}");
        }

        private RedboxEmailMessageModel ComposeEmailMessage(string email, string message)
        {
            var fromAddress = _configSettings.GetString("AppSettings:SenderEmail");
            var subject = _configSettings.GetString("AppSettings:EmailSubject");

            return new RedboxEmailMessageModel
            {
                FromAddress = fromAddress,
                ToAddress = email,
                Subject = subject,
                MailBody = message
            };
        }

        private async Task<(string firstname, string phoneNo, string emailAdd)> GetAccountDetailsAsync(string accountNo)
        {
            try
            {
                var accountInfo = await _accountServiceProxy.DoAccountCIFEnquiry(accountNo);
                if (accountInfo.responseCode == "00")
                    return (accountInfo.result?.FirstName, accountInfo.result?.PhoneNumber, accountInfo.result?.EmailAddress);

                _logger.Info($"Attempt to update customer onboarding request with Account name failed on CifEnquiry with account number {accountNo}");
            }
            catch (Exception exception)
            {
                _logger.Error($"Attempt to update customer onboarding request with Account name failed -> {exception}");
            }
            return default;
        }

        private IList<DataUpdateDocument> BuildExtraAccountDocsFromPayload(DataUpdateRequest request)
        {
            return request.Documents?.Select(doc => new DataUpdateDocument { FileName = doc.Name, Title = doc.Title, ContentOrPath = doc.Base64Content, ContentType = GetDocumentContentType(doc.Name) }).ToList();
        }

        private string GetDocumentContentType(string fileName)
        {
            string contenttype = "";
            switch (fileName.Split('.')[1].ToLower())
            {
                case "doc":
                    contenttype = "application/vnd.ms-word";
                    break;

                case "docx":
                    contenttype = "application/vnd.ms-word";
                    break;

                case "pdf":
                    contenttype = "application/pdf";
                    break;

                case "jpg":
                    contenttype = "image/jpeg";
                    break;

                case "svg":
                    contenttype = "image/svg+xml";
                    break;

                case "jpeg":
                    contenttype = "image/jpeg";
                    break;

                case "png":
                    contenttype = "image/png";
                    break;

                case "gif":
                    contenttype = "image/gif";
                    break;
            }
            return contenttype;
        }

        private CustomerRequest BuildCustomerRequestEntityFromPayload(DataUpdateRequest request, string status)
        {
            return new CustomerRequest
            {
                CreatedDate = DateTime.Now,
                RequestType = "Data Update",
                CustomerAuthType = string.IsNullOrEmpty(request.TinNumber) ? GetAuthType(request.AuthType) : "",
                Status = status,
                TreatedByUnit = "ACCOUNT ORIGINATION",
                AccountName = request.AccountName,
                AccountNumber = request.ExistingAccount,
                Bvn = request.BvnId
            };
        }

        private string GetAuthType(string authType)
        {
            if (authType.ToLower().Equals("signature"))
                return "OTP";
            if (authType.ToLower().Equals("debit-card"))
                return "WITH_DEBIT_CARD";
            return authType.ToUpper();
        }

        public async Task<(bool status, string statusMessage, string result)> SaveAndContinue(DataUpdateRequest request, string status)
        {
            try
            {
                CustomerRequest customerRequest = BuildCustomerRequestEntityFromPayload(request, status);

                customerRequest.DataUpdateDetails = BuildDataUpdateDetails(request);

                CustomerRequest result = new CustomerRequest();

                //Check if caseId is available and handle cases
                if (String.IsNullOrEmpty(request.CaseId))
                {
                    var saveResult = await _customerRequestRepository.AddItem(customerRequest);
                    if (saveResult == null || saveResult.Id < 1)
                        return (false, "Could not save data update request at this time. please retry after some time", string.Empty);

                    saveResult.TranId = GenerateTranId(saveResult.Id);
                    DataUpdateDetails first = saveResult.DataUpdateDetails.FirstOrDefault();

                    if (first != null) first.CaseId = saveResult.TranId;
                    await _customerRequestDataRepository.UpdateCustomerRequest(saveResult);

                    // Send notification for new case id
                    await Task.Factory.StartNew(async () =>
                     {
                         var (firstname, phoneNo, emailAddress) = await GetAccountDetailsAsync(request.ExistingAccount);

                         if (!string.IsNullOrEmpty(phoneNo) || !string.IsNullOrEmpty(emailAddress))
                             await SendSubmissionNotificationMessageAsync(firstname, saveResult.TranId, phoneNo, emailAddress, "SaveAndContinue");
                     });

                    result = saveResult;
                }
                else
                {
                    //retrieve saved session
                    var id = Convert.ToInt32(ExtractIdFromTranId(request.CaseId));
                    if (id == 0)
                        return (false, "Invalid Case ID. Please confirm that the case ID is correct", null);

                    var existingSession = await _customerRequestDataRepository.GetCustomerRequestById(id);

                    if (existingSession == null || existingSession.Id < 1)
                        return (false, "Could not save data update request at this time. please retry after some time", string.Empty);

                    // Set IDs
                    customerRequest.Id = existingSession.Id;
                    if (customerRequest.DataUpdateDetails != null)
                    {
                        var first = customerRequest.DataUpdateDetails.FirstOrDefault();

                        if (existingSession.DataUpdateDetails != null)
                        {
                            var first1 = existingSession.DataUpdateDetails.FirstOrDefault();

                            if (first != null)
                                if (first1 != null)
                                    first.Id = first1.Id;
                            customerRequest.TranId = customerRequest.DataUpdateDetails.FirstOrDefault()?.CaseId;
                        }
                    }

                    //update with new record
                    var updateResult = await _customerRequestDataRepository.UpdateCustomerRequest(customerRequest);

                    result = updateResult;
                }

                return (true, "Request saved successfully", result.TranId);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured while saving data update progress", ex.Message, ex);
                return (false, "Error occured while saving data update progress", ex.Message);
            }
        }

        private string GenerateTranId(long id)
        {
            return $"DA-{id}";
        }

        private List<DataUpdateDetails> BuildDataUpdateDetails(DataUpdateRequest request)
        {
            var result = new List<DataUpdateDetails>
            {
                new DataUpdateDetails {
                    ExistingAccType = request.ExistingAccountType,
                    Documents = BuildExtraAccountDocsFromPayload(request),
                    IdType = request.IdType,
                    IdNumber = request.IdNumber,
                    CaseId = request.CaseId,
                    CurrentStep = request.CurrentStep,
                    Submitted = request.Submitted,
                    IAcceptTermsAndCondition = request.IAcceptTermsAndCondition,
                    DateOfAcceptingTAndC = DateTime.Now,
                    DataToUpdate = request.DataToUpdate,
                    HouseNumber = request.HouseNumber,
                    StreetName = request.StreetName,
                    CityTown = request.CityTown,
                    State = request.State,
                    Lga = request.Lga,
                    BusStop = request.BusStop,
                    Alias = request.Alias,
                    NewEmail = request.NewEmail,
                    NewPhoneNumber = request.NewPhoneNumber,
                    BvnId = request.BvnId,
                    HouseDescription = request.HouseDescription,
                    CountryCode = request.CountryCode,
                    TinNumber = request.TinNumber
                }
            };

            return result;
        }

        public async Task<(bool status, string statusMessage, DataUpdateDetails result)> VerifyCaseId(string caseId)
        {
            if (!caseId.Contains('-'))
                return (false, "Invalid Case ID. Please confirm that the case ID is correct", null);

            var id = Convert.ToInt32(ExtractIdFromTranId(caseId));
            if (id == 0)
                return (false, "Invalid Case ID. Please confirm that the case ID is correct", null);

            try
            {
                var customerReq = await _customerRequestDataRepository.GetCustomerRequestById(id);
                if (customerReq == null)
                    return (false, "Could not retrieve case id at this time. please retry after some time", null);

                var result = customerReq.DataUpdateDetails.FirstOrDefault();
                if (result == null || result.Id < 1)
                    return (false, "Could not retrieve case id at this time. please retry after some time", null);
                return (true, "Data update progress retrieved successfully", result);
            }
            catch (Exception exception)
            {
                _logger.Error("An Error occured while retrieving data update progress", exception.Message, exception);
                throw;
            }
        }

        private long ExtractIdFromTranId(string tranId)
        {
            var stringId = "";
            try
            {
                stringId = tranId.Split('-')[1];
            }
            catch (Exception)
            {
                return 0;
            }

            return long.Parse(stringId);
        }
    }
}
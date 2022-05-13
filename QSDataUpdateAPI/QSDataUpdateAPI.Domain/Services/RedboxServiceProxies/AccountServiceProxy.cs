using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QSDataUpdateAPI.Core.Interfaces.Services.Helpers.Redbox;
using QSDataUpdateAPI.Domain.Common;
using QSDataUpdateAPI.Domain.Models.Requests;
using QSDataUpdateAPI.Domain.Models.Response;
using QSDataUpdateAPI.Domain.Models.Response.Redbox;
using QSDataUpdateAPI.Domain.Services.Helpers.Interfaces;
using QSDataUpdateAPI.Domain.Services.RedboxServiceProxies.Interfaces;

namespace QSDataUpdateAPI.Domain.Services.RedboxServiceProxies
{
    public class AccountServiceProxy : IRedboxAccountServiceProxy
    {
        private readonly IAppLogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IRedboxRequestManagerProxy _requestManagerProxy;

        public AccountServiceProxy(IConfiguration configuration, IAppLogger logger, IRedboxRequestManagerProxy requestManagerProxy)
        {
            _logger = logger;
            _requestManagerProxy = requestManagerProxy;
            _configuration = configuration;
        }

        public async Task<(string responseCode, string responseDescription, CustomerAccountInfo result)> GetCustomerAccountDetails(string accountNumber, string phoneNumber)
        {
            try
            {
                var fetchCustomerAccountInfoPayload = FormFetchCustomerRequestPayload(phoneNumber, accountNumber);
                var response = await _requestManagerProxy.Post<CustomerAccountInfo>(fetchCustomerAccountInfoPayload);
                if (response.ResponseCode == "00" || response.ResponseCode == "000")
                {
                    return ("00", response.ResponseDescription, response.Model);
                }
                return (response.ResponseCode, response.ResponseDescription, null);
            }
            catch (Exception exception)
            {
                _logger.Error("Exception occured while fetching customer profile", ex: exception);
                throw;
            }
        }

        public async Task<(string responseCode, string responseDescription)> DoStraightThroughSave(DataUpdateRequest model, AccountEnquiryInfo cifInfo, string accountNumber)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.NewPhoneNumber) && !string.IsNullOrWhiteSpace(model.NewEmail))
                {
                    var payload = BuildPhoneUpdatePayload(model, accountNumber);
                    var response = await _requestManagerProxy.Post<string>(payload);

                    if (response.ResponseCode == "00" || response.ResponseCode == "000" || response.ResponseCode == "202")
                    {
                        var payload2 = BuildEmailUpdatePayload(model, accountNumber);
                        var response2 = await _requestManagerProxy.Post<string>(payload2);

                        if (response2.ResponseCode == "00" || response2.ResponseCode == "000" || response2.ResponseCode == "202")
                        {
                            return ("00", response2.ResponseDescription);
                        }
                        return (response.ResponseCode, response.ResponseDescription);
                    }

                    return (response.ResponseCode, response.ResponseDescription);
                }

                if (!string.IsNullOrWhiteSpace(model.NewPhoneNumber))
                {
                    var payload = BuildPhoneUpdatePayload(model, accountNumber);
                    var response = await _requestManagerProxy.Post<string>(payload);

                    if (response.ResponseCode == "00" || response.ResponseCode == "000" || response.ResponseCode == "202")
                    {
                        return ("00", response.ResponseDescription);
                    }
                    return (response.ResponseCode, response.ResponseDescription);
                }

                if (!string.IsNullOrWhiteSpace(model.NewEmail))
                {
                    var payload2 = BuildEmailUpdatePayload(model, accountNumber);
                    var response = await _requestManagerProxy.Post<string>(payload2);
                    if (response.ResponseCode == "00" || response.ResponseCode == "000" || response.ResponseCode == "202")
                    {
                        return ("00", response.ResponseDescription);
                    }
                    return (response.ResponseCode, response.ResponseDescription);
                }
                //var updatePayload = UpdateCustomerEmailOrPhoneToCoreBanking(model, cifInfo);

                //var response = await _requestManagerProxy.Post<string>(updatePayload);
                //if (response.ResponseCode == "00" || response.ResponseCode == "000" || response.ResponseCode == "202")
                //{
                //    return ("00", response.ResponseDescription);
                //}
                return ("99", "Phone and Email was not updated");
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw;
            }
        }

        public async Task<(string responseCode, string responseDescription, string segment)> GetAccountSegment(string accountNumber)
        {
            try
            {
                var fetchAccountSegment = FormGetCustomerAccountSegmentRequestPayload(accountNumber);
                var response = await _requestManagerProxy.Post<string>(fetchAccountSegment);
                if (response.ResponseCode == "00" || response.ResponseCode == "000" || response.ResponseCode == "202")
                {
                    var accountSegment = Util.GetXmlTagValue(response.Detail, "segmentName");
                    return ("00", response.ResponseDescription, accountSegment);
                }
                return (response.ResponseCode, response.ResponseDescription, null);
            }
            catch (Exception exception)
            {
                _logger.Error("Exception occured while fetching customer profile", ex: exception);
                throw;
            }
        }

        public async Task<(string responseCode, string responseDescription, CustomerAccountInfo result)> DoCorporateAccountEnquiry(string accountNumber)
        {
            try
            {
                var fetchCustomerAccountInfoPayload = FormCorpAccountEnquiryRequestPayload(accountNumber);
                var response = await _requestManagerProxy.Post<string>(fetchCustomerAccountInfoPayload);
                if (response.ResponseCode == "00" || response.ResponseCode == "000" || response.ResponseCode == "202")
                {
                    return ("00", response.ResponseDescription, BuildAccountInfoFromResponse(response.Detail));
                }
                return (response.ResponseCode, response.ResponseDescription, null);
            }
            catch (Exception exception)
            {
                _logger.Error("Exception occured while doing corporate account name enquiry", ex: exception);
                throw;
            }
        }

        public async Task<(string responseCode, string responseDescription, AccountEnquiryInfo result)> DoAccountCIFEnquiry(string accountNumber)
        {
            try
            {
                var fetchCustomerAccountInfoPayload = FormAccountCifEnquiryRequestPayload(accountNumber);
                var response = await _requestManagerProxy.Post<string>(fetchCustomerAccountInfoPayload);
                if (response.ResponseCode == "00" || response.ResponseCode == "000" || response.ResponseCode == "202")
                {
                    return ("00", response.ResponseDescription, BuildAccountEnquiryInfoFromResponse(response.Detail));
                }
                return (response.ResponseCode, response.ResponseDescription, null);
            }
            catch (Exception exception)
            {
                _logger.Error("Exception occured while doing corporate account name enquiry", ex: exception);
                throw;
            }
        }

        public async Task<(string responseCode, string responseDescription, TinEnquiryInfo result)> DoTinEnquiry(string tinNumber)
        {
            try
            {
                var fetchCustomerAccountInfoPayload = FormCustomerTinEnquiryRequestPayload(tinNumber);
                var response = await _requestManagerProxy.Post<string>(fetchCustomerAccountInfoPayload);
                if (response.ResponseCode == "00" || response.ResponseCode == "000" || response.ResponseCode == "202")
                {
                    return ("00", response.ResponseDescription, BuildTinEnquiryInfoFromResponse(response.Detail));
                }
                return (response.ResponseCode, response.ResponseDescription, null);
            }
            catch (Exception exception)
            {
                _logger.Error("Exception occured while doing corporate account name enquiry", ex: exception);
                throw;
            }
        }

        public async Task<(string responseCode, string responseDescription, CustomerAccountInfo result)> DoBVNEnquiry(string bvnId)
        {
            try
            {
                var fetchBVNEnquiryPayload = BuildBVNValidationRequestPayload(bvnId);
                var response = await _requestManagerProxy.Post<string>(fetchBVNEnquiryPayload);
                if (response.ResponseCode == "00" || response.ResponseCode == "000" || response.ResponseCode == "202")
                {
                    return ("00", response.ResponseDescription, BuildAccountInfoFromBVNEnquiryResponse(response.Detail));
                }
                return (response.ResponseCode, response.ResponseDescription, null);
            }
            catch (Exception exception)
            {
                _logger.Error("Exception occured while doing BVN enquiry", ex: exception);
                throw;
            }
        }

        public async Task<List<CityState>> GetCityState()
        {
            string sql = "Select * FROM [QuickService].[dbo].[CITY_STATE] ORDER BY region asc";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("QuickServiceDbConn")))
            {
                var cityState = connection.QueryAsync<CityState>(sql).Result.ToList();

                return cityState;
            }
        }

        #region private_helpers

        private string FormFetchCustomerRequestPayload(string phoneNumber, string accountNumber)
        {
            string payload = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:soap=""http://soap.request.manager.redbox.stanbic.com/"">
                               <soapenv:Header/>
                                <soapenv:Body>
                                    <soap:request>
                                        <reqTranId>{Util.TimeStampCode()}</reqTranId>
                                        <channel>USSD</channel>
                                        <type>FETCH_CUSTOMER</type>
                                        <customerId>{accountNumber}</customerId>
                                        <customerIdType>ACCOUNT_NUMBER</customerIdType>
                                        <submissionTime>{DateTime.Now}</submissionTime>
                                        <body>
                                            <otherRequestDetails>
                                                <passCode />
                                                <passId>{phoneNumber}</passId>
                                                <passIdType>PHONE_NUMBER</passIdType>
                                                <passCodeType>01</passCodeType>
                                            </otherRequestDetails>
                                          </body>
                                     </soap:request>
                               </soapenv:Body>
                              </soapenv:Envelope>";
            return payload;
        }

        private string BuildPhoneUpdatePayload(DataUpdateRequest model, string accountNumber)
        {
            var code = model.CountryCode;
            var number = model.NewPhoneNumber;
            if (number.StartsWith("0"))
            {
                number = "+" + model.CountryCode + "(0)" + number.Substring(1);
            }
            else
            {
                number = "+" + model.CountryCode + "(0)" + number;
            }
            return $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:soap=""http://soap.request.manager.redbox.stanbic.com/"">
                        <soapenv:Header/>
                        <soapenv:Body>
                        <soap:request>
                        <reqTranId>142576</reqTranId>
                        <channel>ATM</channel>
                        <type>UPDATE_DATA</type>
                        <customerId>{accountNumber}</customerId>
                        <customerIdType>ACCOUNT_NUMBER</customerIdType>
                        <body><![CDATA[<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
                        <otherRequestDetails>
                        <newPhoneNumber>{number}</newPhoneNumber>
                        </otherRequestDetails>]]></body>
                        <submissionTime>07-NOV-18 05.31.17</submissionTime>
                        </soap:request>
                        </soapenv:Body>
                      </soapenv:Envelope>";
        }

        private string BuildEmailUpdatePayload(DataUpdateRequest model, string accountNumber)
        {
            return $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:soap=""http://soap.request.manager.redbox.stanbic.com/"">
                        <soapenv:Header/>
                        <soapenv:Body>
                        <soap:request>
                        <reqTranId>142576</reqTranId>
                        <channel>MOBILE_APP</channel>
                        <type>UPDATE_EMAIL</type>
                        <customerId>{accountNumber}</customerId>
                        <customerIdType>ACCOUNT_NUMBER</customerIdType>
                        <body><![CDATA[<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
                        <otherRequestDetails>
                        <newEmail>{model.NewEmail}</newEmail>
                        </otherRequestDetails>]]></body>
                        <submissionTime>07-NOV-18 05.31.17</submissionTime>
                        </soap:request>
                        </soapenv:Body>
                       </soapenv:Envelope>";
        }

        private string UpdateCustomerEmailOrPhoneToCoreBanking(DataUpdateRequest model, AccountEnquiryInfo cifInfo)
        {
            string payload = "";
            if (!string.IsNullOrEmpty(model.NewPhoneNumber) && !string.IsNullOrEmpty(model.NewEmail))
            {
                payload = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:soap=""http://soap.request.manager.redbox.stanbic.com/"">
   <soapenv:Header/>
     <soapenv:Body>
          <ns2:request xmlns:ns2=""http://soap.request.manager.redbox.stanbic.com/"">
            <channel>BPM</channel>
            <type>RETAIL_CUSTOMER_UPDATE</type>
              <body>
                   <![CDATA[
                      <RetailCustomerUpdate>
                      <ModuleTranReferenceId>NGNTXT20201231201312</ModuleTranReferenceId>
                      <CustId>{cifInfo.CustomerId}</CustId>
                      <RetailCustomerData>
                      </RetailCustomerData>
                    <CustomerPhoneEmailData>
                   <PhoneEmailID>{cifInfo.phoneEmailIdPhone}</PhoneEmailID>
                   <PhoneOrEmail>PHONE</PhoneOrEmail>
                   <PhoneEmailType>CELLPH</PhoneEmailType>
                      <PhoneNum>({model.CountryCode})(0){model.NewPhoneNumber.Substring(model.NewPhoneNumber.Length - (model.NewPhoneNumber.Length - 1))}</PhoneNum>
                      <PhoneNumLocalCode>{model.NewPhoneNumber.Substring(model.NewPhoneNumber.Length - (model.NewPhoneNumber.Length - 1))}</PhoneNumLocalCode>
                   </CustomerPhoneEmailData>
                      <CustomerPhoneEmailData>
                   <PhoneEmailID>{cifInfo.phoneEmailIdEmail}</PhoneEmailID>
                   <PhoneOrEmail>EMAIL</PhoneOrEmail>
                   <PhoneEmailType>COMMEML</PhoneEmailType>
                      <Email>{model.NewEmail}</Email>
                        <PrefFlag>Y</PrefFlag>
                   </CustomerPhoneEmailData>
                   </RetailCustomerUpdate>
                   ]]>
              </body>
               <submissionTime>{DateTime.Now}</submissionTime>
                   </ns2:request>
                 </soapenv:Body>
               </soapenv:Envelope>
                ";
            }
            else if (!string.IsNullOrEmpty(model.NewEmail))
            {
                payload = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:soap=""http://soap.request.manager.redbox.stanbic.com/"">
   <soapenv:Header/>
     <soapenv:Body>
          <ns2:request xmlns:ns2=""http://soap.request.manager.redbox.stanbic.com/"">
            <channel>BPM</channel>
            <type>RETAIL_CUSTOMER_UPDATE</type>
              <body>
                   <![CDATA[
                      <RetailCustomerUpdate>
                      <ModuleTranReferenceId>NGNTXT20201231201312</ModuleTranReferenceId>
                      <CustId>{cifInfo.CustomerId}</CustId>
                      <RetailCustomerData>
                      </RetailCustomerData>
                      <CustomerPhoneEmailData>
                   <PhoneEmailID>{cifInfo.phoneEmailIdEmail}</PhoneEmailID>
                   <PhoneOrEmail>EMAIL</PhoneOrEmail>
                   <PhoneEmailType>COMMEML</PhoneEmailType>
                      <Email>{model.NewEmail}</Email>
                      <PrefFlag>Y</PrefFlag>
                   </CustomerPhoneEmailData>
                   </RetailCustomerUpdate>
                   ]]>
              </body>
               <submissionTime>{DateTime.Now}</submissionTime>
                   </ns2:request>
                 </soapenv:Body>
               </soapenv:Envelope>
                ";
            }
            else
            {
                payload = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:soap=""http://soap.request.manager.redbox.stanbic.com/"">
   <soapenv:Header/>
     <soapenv:Body>
          <ns2:request xmlns:ns2=""http://soap.request.manager.redbox.stanbic.com/"">
            <channel>BPM</channel>
            <type>RETAIL_CUSTOMER_UPDATE</type>
              <body>
                   <![CDATA[
                      <RetailCustomerUpdate>
                      <ModuleTranReferenceId>NGNTXT20201231201312</ModuleTranReferenceId>
                      <CustId>{cifInfo.CustomerId}</CustId>
                      <RetailCustomerData>
                      </RetailCustomerData>
                      <CustomerPhoneEmailData>
                   <PhoneEmailID>{cifInfo.phoneEmailIdPhone}</PhoneEmailID>
                   <PhoneOrEmail>PHONE</PhoneOrEmail>
                   <PhoneEmailType>CELLPH</PhoneEmailType>
                      <PhoneNum>{model.CountryCode}(0){model.NewPhoneNumber.Substring(model.NewPhoneNumber.Length - 10)}</PhoneNum>
                      <PhoneNumLocalCode>{model.NewPhoneNumber.Substring(model.NewPhoneNumber.Length - (model.NewPhoneNumber.Length - 1))}</PhoneNumLocalCode>
                   </CustomerPhoneEmailData>
                   </RetailCustomerUpdate>
                   ]]>
              </body>
               <submissionTime>{DateTime.Now}</submissionTime>
                   </ns2:request>
                 </soapenv:Body>
               </soapenv:Envelope>
                ";
            }

            return payload;
        }

        private string FormGetCustomerAccountSegmentRequestPayload(string accountNumber)
        {
            string payload = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:soap=""http://soap.request.manager.redbox.stanbic.com/"">
                               <soapenv:Header/>
                                <soapenv:Body>
                                    <soap:request>
                                        <reqTranId>{Util.TimeStampCode()}</reqTranId>
                                        <channel>INTERNET_BANKING</channel>
                                        <type>GET_ACCOUNT_SEGMENT</type>
                                        <customerId>{accountNumber}</customerId>
                                        <customerIdType>ACCOUNT_NUMBER</customerIdType>
                                        <submissionTime>{DateTime.Now:f}</submissionTime>
                                        <body></body>
                                     </soap:request>
                               </soapenv:Body>
                              </soapenv:Envelope>";
            return payload;
        }

        private string FormCorpAccountEnquiryRequestPayload(string accountNumber)
        {
            var payload = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:soap=""http://soap.request.manager.redbox.stanbic.com/"">
   <soapenv:Header/>
   <soapenv:Body>
      <soap:request>
         <reqTranId>{Util.TimeStampCode()}</reqTranId>
         <channel>BPM</channel>
         <type>ACCOUNT_ENQUIRY</type>
         <submissionTime>{DateTime.Now.ToString("o")}</submissionTime>
         <body><![CDATA[<otherRequestDetails>
         <cifId></cifId>
        <accountNumber>{accountNumber}</accountNumber>
 <moduleTranReferenceId>34343434343</moduleTranReferenceId>
      </otherRequestDetails>]]></body>
      </soap:request>
   </soapenv:Body>
</soapenv:Envelope>";
            return payload;
        }

        private CustomerAccountInfo BuildAccountInfoFromResponse(string detail)
        {
            _logger.Info(detail);
            return new CustomerAccountInfo
            {
                BVN = Util.GetTagValue(detail, "AccountBvn"),
                FirstName = Util.GetTagValue(detail, "CustomerFirstName"),
                LastName = Util.GetTagValue(detail, "CustomerLastName"),
                EnrollmentBank = "221",
                EnrollmentBranch = Util.GetTagValue(detail, "AccountBranchName"),
                CifId = Util.GetTagValue(detail, "CustomerId")
            };
        }

        private AccountEnquiryInfo BuildAccountEnquiryInfoFromResponse(string detail)
        {
            //var mainData = Deserailizer.DeserializeXML<DoCustomerInformationEnquiryResponse>(detail.Replace("ns2:", "").Replace("xmlns:ns2=\"http://soap.finacle.redbox.stanbic.com/\"", ""));
            AccountEnquiryInfo accountEnquiryInfo = new AccountEnquiryInfo
            {
                FirstName = Util.GetFirstTagValue(detail, "FirstName", ignoreCase: false),
                LastName = Util.GetFirstTagValue(detail, "LastName", ignoreCase: false),
                EmailAddress = Util.GetFirstTagValue(detail, "Email", ignoreCase: false),
                PhoneNumber = Util.GetTagValue(detail, "PhoneNumber1"),
                PhoneNumber1 = Util.GetTagValue(detail, "PhoneNumber2"),
                AccountName = Util.GetTagValue(detail, "AccountName"),
                AccountSchemeCode = Util.GetTagValue(detail, "AccountSchemeCode"),
                AccountSchemeType = Util.GetTagValue(detail, "AccountSchemeType"),
                BVN = Util.GetTagValue(detail, "Bvn").Length <= 11 ? Util.GetTagValue(detail, "Bvn") : Util.GetTagValue(detail, "Bvn").Substring(0, 11),
                CustomerCreationDate = Util.GetTagValue(detail, "AccountOpenDate"),
                AccountStatus = Util.GetTagValue(detail, "AccountStatus"),
                CustomerId = Util.GetTagValue(detail, "CustId"),
            };
            var firstTag = Util.GetFirstTagValue(detail, "PhoneEmailType");
            var secondTag = Util.GetSecondTagValue(detail, "PhoneEmailType");
            var thirdTag = Util.GetThirdTagValue(detail, "PhoneEmailType");
            if (firstTag == "HOMEEML")
            {
                accountEnquiryInfo.phoneEmailIdEmail = Util.GetFirstTagValue(detail, "PhoneEmailId");
                accountEnquiryInfo.phoneEmailIdEmailType = "HOMEEML";
            }
            else if (firstTag == "COMMEML")
            {
                accountEnquiryInfo.phoneEmailIdEmail = Util.GetFirstTagValue(detail, "PhoneEmailId");
                accountEnquiryInfo.phoneEmailIdEmailType = "COMMEML";
            }
            else if (firstTag == "CELLPH")
            {
                accountEnquiryInfo.phoneEmailIdPhone = Util.GetFirstTagValue(detail, "PhoneEmailId");
                accountEnquiryInfo.phoneEmailIdPhoneType = "CELLPH";
            }

            if (secondTag == "HOMEEML")
            {
                accountEnquiryInfo.phoneEmailIdEmail = Util.GetSecondTagValue(detail, "PhoneEmailId");
                accountEnquiryInfo.phoneEmailIdEmailType = "HOMEEML";
            }
            else if (secondTag == "COMMEML")
            {
                accountEnquiryInfo.phoneEmailIdEmail = Util.GetSecondTagValue(detail, "PhoneEmailId");
                accountEnquiryInfo.phoneEmailIdEmailType = "COMMEML";
            }
            else if (secondTag == "CELLPH")
            {
                accountEnquiryInfo.phoneEmailIdPhone = Util.GetSecondTagValue(detail, "PhoneEmailId");
                accountEnquiryInfo.phoneEmailIdPhoneType = "CELLPH";
            }

            if (thirdTag == "HOMEEML")
            {
                accountEnquiryInfo.phoneEmailIdEmail = Util.GetSecondTagValue(detail, "PhoneEmailId");
                accountEnquiryInfo.phoneEmailIdEmailType = "HOMEEML";
            }
            else if (thirdTag == "COMMEML")
            {
                accountEnquiryInfo.phoneEmailIdEmail = Util.GetSecondTagValue(detail, "PhoneEmailId");
                accountEnquiryInfo.phoneEmailIdEmailType = "COMMEML";
            }
            else if (thirdTag == "CELLPH")
            {
                accountEnquiryInfo.phoneEmailIdPhone = Util.GetSecondTagValue(detail, "PhoneEmailId");
                accountEnquiryInfo.phoneEmailIdPhoneType = "CELLPH";
            }

            //if (thirdTag == "COMMEML")
            //{
            //    accountEnquiryInfo.phoneEmailIdEmail = Util.GetThirdTagValue(detail, "PhoneEmailId");
            //    accountEnquiryInfo.phoneEmailIdEmailType = "COMMEML";
            //}
            return accountEnquiryInfo;
        }

        private TinEnquiryInfo BuildTinEnquiryInfoFromResponse(string detail)
        {
            return new TinEnquiryInfo
            {
                AccountName = Util.GetTagValue(detail, "taxPayername"),
            };
        }

        private CustomerAccountInfo BuildAccountInfoFromBVNEnquiryResponse(string detail)
        {
            return new CustomerAccountInfo
            {
                BVN = Util.GetTagValue(detail, "bvn"),
                FirstName = Util.GetTagValue(detail, "firstName"),
                LastName = Util.GetTagValue(detail, "lastName"),
                EnrollmentBank = "221",
                EnrollmentBranch = Util.GetTagValue(detail, "enrollmentBranch"),
                CifId = Util.GetTagValue(detail, "cifId"),
                DateOfBirth = Util.GetTagValue(detail, "dateOfBirth"),
                MaskedPhoneNumber = Util.GetTagValue(detail, "maskedPhoneNumber")
            };
        }

        private string FormAccountCifEnquiryRequestPayload(string accountNumber)
        {
            var reqTranId = Util.TimeStampCode();
            var payload = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:soap=""http://soap.request.manager.redbox.stanbic.com/"">
                               <soapenv:Header/>
                               <soapenv:Body>
                                  <soap:request>
                                     <reqTranId>{reqTranId}</reqTranId>
                                     <channel>INTERNET_BANKING</channel>
                                     <type>CIF_ENQUIRY</type>
                                     <submissionTime>{DateTime.Now.ToString("o")}</submissionTime>
                                     <body><![CDATA[<otherRequestDetails>
                                     <cifId></cifId>
                                     <cifType></cifType>
                                    <accountNumber>{accountNumber}</accountNumber>
                                    <moduleTranReferenceId>{reqTranId}</moduleTranReferenceId>
                                  </otherRequestDetails>]]></body>
                                  </soap:request>
                               </soapenv:Body>
                            </soapenv:Envelope>";
            return payload;
        }

        private string FormCustomerTinEnquiryRequestPayload(string tinNumber)
        {
            var reqTranId = Util.TimeStampCode();
            var payload = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:soap=""http://soap.request.manager.redbox.stanbic.com/"">
                              <soapenv:Header/>
                               <soapenv:Body>
                                  <soap:request>
                                     <reqTranId>{reqTranId}</reqTranId>
                                     <channel>BPM|AOUSER</channel>
                                     <type>VALIDATE_TIN</type>
                                     <customerId>{tinNumber}</customerId>
                                     <customerIdType>TIN </customerIdType>
                                     <body/>
                                     <submissionTime>{DateTime.Now.ToString("o")}</submissionTime>
                                  </soap:request>
                               </soapenv:Body>
                            </soapenv:Envelope>";
            return payload;
        }

        private string BuildBVNValidationRequestPayload(string bvnId)
        {
            var payload = $@"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <soap:Body>

                                    <request xmlns=""http://soap.request.manager.redbox.stanbic.com/""
                                        xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                                        xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
                                        <reqTranId xmlns="""">154673685</reqTranId>
                                        <channel xmlns="""">MOBILE_APP</channel>
                                        <type xmlns="""">FETCH_CUSTOMER</type>
                                        <customerId xmlns="""">{bvnId}</customerId>
                                        <customerIdType xmlns="""">BVN</customerIdType>
                                        <body xmlns=""""/>
                                        <submissionTime>{DateTime.Now}</submissionTime>
                                    </request>

                                </soap:Body>
                            </soap:Envelope>";
            return payload;
        }

        #endregion private_helpers
    }
}
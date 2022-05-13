using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QSDataUpdateAPI.Domain.Models.Requests
{
    public class DataUpdateRequest
    {
        [Required(ErrorMessage = "You need to provide existing account to do data update")]
        public string ExistingAccount { get; set; }
        public string ExistingAccountType { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string AuthType { get; set; }
        public List<DataUpdateDocumentModel> Documents { get; set; }
        public string OtpSourceReference { get; set; }
        public string Otp{get;set;}
        public string OtpIdentifier {get;set;}
        public string OtpReasonCode { get; set; }
        public string AccountName {get;set; }
        public string BvnId { get; set; }


        // Terms and Conditions
        public bool IAcceptTermsAndCondition { get; set; }
        public DateTime DateOfAcceptingTAndC { get; set; }

        // Data
        public string DataToUpdate { get; set; }
        public string HouseNumber { get; set; }
        public string StreetName { get; set; }
        public string CityTown { get; set; }
        public string State { get; set; }
        public string Lga { get; set; }
        public string BusStop { get; set; }
        public string Alias { get; set; }
        public string HouseDescription { get; set; }
        public string NewPhoneNumber { get; set; }
        public string NewEmail { get; set; }


        // ID Info
        public string IdType { get; set; }
        public string IdNumber { get; set; }

        public string TinNumber { get; set; }
        // Session
        public string CaseId { get; set; }
        public string CurrentStep { get; set; }
        public bool Submitted { get; set; } = false;

    }


    public class DataUpdateDocumentModel
    {
        public string Title { get; set; }
        [Required(ErrorMessage = "Document name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Base 64 encoded document content is required")]
        public string Base64Content { get; set; }
        public string ContentType { get; set; }
    }
}

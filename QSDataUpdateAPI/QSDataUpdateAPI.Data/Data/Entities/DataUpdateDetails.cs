using System;
using System.Collections.Generic;

namespace QSDataUpdateAPI.Core.Domain.Entities
{
    public partial class DataUpdateDetails
    {
        public int Id { get; set; }
        public long CustomerReqId { get; set; }
        public virtual CustomerRequest CustomerReq { get; set; }
        public virtual ICollection<DataUpdateDocument> Documents { get; set; }

        // Account Info
        public string ExistingAccountSegment { get; set; }
        public string ExistingAccType { get; set; }
        public string PhoneNumber { get; set; }
        public string BvnId { get; set; }

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
        public string CountryCode { get; set; }
        public string NewPhoneNumber { get; set; }
        public string NewEmail { get; set; }

        // ID Info
        public string IdType { get; set; }
        public string IdNumber { get; set; }

        // Session
        public string CaseId { get; set; }
        public string CurrentStep { get; set; }
        public string TinNumber { get; set; }
        public bool Submitted { get; set; }

        // Terms and Conditions
        public bool IAcceptTermsAndCondition { get; set; }
        public DateTime DateOfAcceptingTAndC { get; set; }

        public DataUpdateDetails()
        {
            Documents ??= new List<DataUpdateDocument>();
        }
    }
}

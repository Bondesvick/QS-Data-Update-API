using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace QSDataUpdateAPI.Domain.Models.Requests.Redbox
{
    // using System.Xml.Serialization;
    // XmlSerializer serializer = new XmlSerializer(typeof(DoCustomerInformationEnquiryResponse));
    // using (StringReader reader = new StringReader(xml))
    // {
    //    var test = (DoCustomerInformationEnquiryResponse)serializer.Deserialize(reader);
    // }

    [XmlRoot(ElementName = "ContactDetails")]
    public class ContactDetails
    {
        [XmlElement(ElementName = "ResidentialAddress")]
        public string ResidentialAddress { get; set; }

        [XmlElement(ElementName = "StateOfResidence")]
        public string StateOfResidence { get; set; }

        [XmlElement(ElementName = "LGAOfResidence")]
        public object LGAOfResidence { get; set; }

        [XmlElement(ElementName = "Landmarks")]
        public object Landmarks { get; set; }

        [XmlElement(ElementName = "Email")]
        public string Email { get; set; }

        [XmlElement(ElementName = "PhoneNumber1")]
        public string PhoneNumber1 { get; set; }

        [XmlElement(ElementName = "PhoneNumber2")]
        public string PhoneNumber2 { get; set; }
    }

    [XmlRoot(ElementName = "Demography")]
    public class Demography
    {
        [XmlElement(ElementName = "Salutation")]
        public string Salutation { get; set; }

        [XmlElement(ElementName = "Surname")]
        public string Surname { get; set; }

        [XmlElement(ElementName = "MiddleName")]
        public string MiddleName { get; set; }

        [XmlElement(ElementName = "FirstName")]
        public string FirstName { get; set; }

        [XmlElement(ElementName = "Gender")]
        public string Gender { get; set; }

        [XmlElement(ElementName = "DateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [XmlElement(ElementName = "MaritalStatus")]
        public int MaritalStatus { get; set; }

        [XmlElement(ElementName = "Nationality")]
        public string Nationality { get; set; }
    }

    [XmlRoot(ElementName = "Address")]
    public class Address
    {
        [XmlElement(ElementName = "addressCategory")]
        public string AddressCategory { get; set; }

        [XmlElement(ElementName = "addressId")]
        public int AddressId { get; set; }

        [XmlElement(ElementName = "addressLine1")]
        public string AddressLine1 { get; set; }

        [XmlElement(ElementName = "addressLine2")]
        public object AddressLine2 { get; set; }

        [XmlElement(ElementName = "addressLine3")]
        public object AddressLine3 { get; set; }

        [XmlElement(ElementName = "country")]
        public object Country { get; set; }

        [XmlElement(ElementName = "houseNumber")]
        public object HouseNumber { get; set; }

        [XmlElement(ElementName = "isPreferredAddr")]
        public int IsPreferredAddr { get; set; }

        [XmlElement(ElementName = "postalCode")]
        public int PostalCode { get; set; }

        [XmlElement(ElementName = "startDate")]
        public DateTime StartDate { get; set; }

        [XmlElement(ElementName = "streetName")]
        public object StreetName { get; set; }

        [XmlElement(ElementName = "streetNumber")]
        public object StreetNumber { get; set; }

        [XmlElement(ElementName = "endDate")]
        public DateTime EndDate { get; set; }

        [XmlElement(ElementName = "buildingLevel")]
        public object BuildingLevel { get; set; }

        [XmlElement(ElementName = "cityCode")]
        public string CityCode { get; set; }

        [XmlElement(ElementName = "countryCode")]
        public string CountryCode { get; set; }

        [XmlElement(ElementName = "domicile")]
        public object Domicile { get; set; }

        [XmlElement(ElementName = "freeTextAddress")]
        public string FreeTextAddress { get; set; }

        [XmlElement(ElementName = "localityName")]
        public object LocalityName { get; set; }

        [XmlElement(ElementName = "premiseName")]
        public object PremiseName { get; set; }

        [XmlElement(ElementName = "stateCode")]
        public string StateCode { get; set; }

        [XmlElement(ElementName = "suburb")]
        public object Suburb { get; set; }

        [XmlElement(ElementName = "town")]
        public object Town { get; set; }
    }

    [XmlRoot(ElementName = "CustomerAddressInformation")]
    public class CustomerAddressInformation
    {
        [XmlElement(ElementName = "Address")]
        public List<Address> Address { get; set; }
    }

    [XmlRoot(ElementName = "CorporateCustomerInformation")]
    public class CorporateCustomerInformation
    {
        [XmlElement(ElementName = "CityOfIncorporation")]
        public object CityOfIncorporation { get; set; }

        [XmlElement(ElementName = "LegalEntityType")]
        public object LegalEntityType { get; set; }

        [XmlElement(ElementName = "PrincipalNatureOfBusiness")]
        public object PrincipalNatureOfBusiness { get; set; }

        [XmlElement(ElementName = "PrincipalPlaceOfOperation")]
        public object PrincipalPlaceOfOperation { get; set; }
    }

    [XmlRoot(ElementName = "RetailCustomerInformation")]
    public class RetailCustomerInformation
    {
        [XmlElement(ElementName = "FirstName")]
        public string FirstName { get; set; }

        [XmlElement(ElementName = "LastName")]
        public string LastName { get; set; }

        [XmlElement(ElementName = "AdditionalName")]
        public object AdditionalName { get; set; }

        [XmlElement(ElementName = "BirthDate")]
        public DateTime BirthDate { get; set; }

        [XmlElement(ElementName = "Gender")]
        public string Gender { get; set; }

        [XmlElement(ElementName = "MaritalStatusCode")]
        public object MaritalStatusCode { get; set; }

        [XmlElement(ElementName = "MaritalStatusDesc")]
        public int MaritalStatusDesc { get; set; }

        [XmlElement(ElementName = "NationalityCode")]
        public object NationalityCode { get; set; }

        [XmlElement(ElementName = "NationalityDesc")]
        public string NationalityDesc { get; set; }

        [XmlElement(ElementName = "OptOutInd")]
        public object OptOutInd { get; set; }

        [XmlElement(ElementName = "Race")]
        public object Race { get; set; }

        [XmlElement(ElementName = "RaceDescription")]
        public object RaceDescription { get; set; }

        [XmlElement(ElementName = "ResidingCountryCode")]
        public object ResidingCountryCode { get; set; }

        [XmlElement(ElementName = "ResidingCountryDescription")]
        public string ResidingCountryDescription { get; set; }

        [XmlElement(ElementName = "EmployementStatus")]
        public int EmployementStatus { get; set; }

        [XmlElement(ElementName = "NameOfEmployer")]
        public object NameOfEmployer { get; set; }

        [XmlElement(ElementName = "JobTitleCode")]
        public object JobTitleCode { get; set; }

        [XmlElement(ElementName = "JobTitleDesc")]
        public object JobTitleDesc { get; set; }

        [XmlElement(ElementName = "OccupationCode")]
        public object OccupationCode { get; set; }

        [XmlElement(ElementName = "OccupationDesc")]
        public int OccupationDesc { get; set; }

        [XmlElement(ElementName = "PeriodOfEmployment")]
        public object PeriodOfEmployment { get; set; }
    }

    [XmlRoot(ElementName = "DocumentData")]
    public class DocumentData
    {
        [XmlElement(ElementName = "CountryOfIssue")]
        public string CountryOfIssue { get; set; }

        [XmlElement(ElementName = "DocumentCode")]
        public string DocumentCode { get; set; }

        [XmlElement(ElementName = "IssueDate")]
        public DateTime IssueDate { get; set; }

        [XmlElement(ElementName = "TypeCode")]
        public string TypeCode { get; set; }

        [XmlElement(ElementName = "PlaceOfIssue")]
        public string PlaceOfIssue { get; set; }

        [XmlElement(ElementName = "UniqueId")]
        public string UniqueId { get; set; }
    }

    [XmlRoot(ElementName = "CustomerDocumentData")]
    public class CustomerDocumentData
    {
        [XmlElement(ElementName = "DocumentData")]
        public DocumentData DocumentData { get; set; }
    }

    [XmlRoot(ElementName = "CustomerGeneralData")]
    public class CustomerGeneralData
    {
        [XmlElement(ElementName = "ChannelId")]
        public object ChannelId { get; set; }

        [XmlElement(ElementName = "ChannelcustId")]
        public object ChannelcustId { get; set; }

        [XmlElement(ElementName = "CustId")]
        public int CustId { get; set; }

        [XmlElement(ElementName = "CustomerCreationDate")]
        public DateTime CustomerCreationDate { get; set; }

        [XmlElement(ElementName = "CustTypeCode")]
        public object CustTypeCode { get; set; }

        [XmlElement(ElementName = "DefaultAddrType")]
        public string DefaultAddrType { get; set; }

        [XmlElement(ElementName = "DsaId")]
        public object DsaId { get; set; }

        [XmlElement(ElementName = "EmployeeId")]
        public object EmployeeId { get; set; }

        [XmlElement(ElementName = "GroupIdCode")]
        public int GroupIdCode { get; set; }

        [XmlElement(ElementName = "GstFlag")]
        public object GstFlag { get; set; }

        [XmlElement(ElementName = "LanguageCode")]
        public string LanguageCode { get; set; }

        [XmlElement(ElementName = "NameInNativeLanguage")]
        public object NameInNativeLanguage { get; set; }

        [XmlElement(ElementName = "PrimaryDocType")]
        public object PrimaryDocType { get; set; }

        [XmlElement(ElementName = "primaryRelationshipManagerId")]
        public string PrimaryRelationshipManagerId { get; set; }

        [XmlElement(ElementName = "RatingCode")]
        public object RatingCode { get; set; }

        [XmlElement(ElementName = "SalutationCode")]
        public int SalutationCode { get; set; }

        [XmlElement(ElementName = "SecondaryRelationshipManagerId")]
        public object SecondaryRelationshipManagerId { get; set; }

        [XmlElement(ElementName = "SectorCode")]
        public int SectorCode { get; set; }

        [XmlElement(ElementName = "Segment")]
        public int Segment { get; set; }

        [XmlElement(ElementName = "SegmentLevel")]
        public object SegmentLevel { get; set; }

        [XmlElement(ElementName = "SegmentNum")]
        public object SegmentNum { get; set; }

        [XmlElement(ElementName = "SegmentType")]
        public int SegmentType { get; set; }

        [XmlElement(ElementName = "ShortName")]
        public string ShortName { get; set; }

        [XmlElement(ElementName = "SicCode")]
        public object SicCode { get; set; }

        [XmlElement(ElementName = "IsStaff")]
        public string IsStaff { get; set; }

        [XmlElement(ElementName = "SubSectorCode")]
        public string SubSectorCode { get; set; }

        [XmlElement(ElementName = "IsSuspended")]
        public string IsSuspended { get; set; }

        [XmlElement(ElementName = "TertiaryRelationshipManagerId")]
        public object TertiaryRelationshipManagerId { get; set; }

        [XmlElement(ElementName = "BranchId")]
        public int BranchId { get; set; }
    }

    [XmlRoot(ElementName = "phoneEmailInfo")]
    public class PhoneEmailInfo
    {
        [XmlElement(ElementName = "PhoneEmailId")]
        public int PhoneEmailId { get; set; }

        [XmlElement(ElementName = "PhoneEmailType")]
        public string PhoneEmailType { get; set; }

        [XmlElement(ElementName = "IsPhoneInfo")]
        public int IsPhoneInfo { get; set; }

        [XmlElement(ElementName = "PreferredFlag")]
        public int PreferredFlag { get; set; }

        [XmlElement(ElementName = "Email")]
        public object Email { get; set; }

        [XmlElement(ElementName = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [XmlElement(ElementName = "PhoneNumberCountryCode")]
        public int PhoneNumberCountryCode { get; set; }

        [XmlElement(ElementName = "PhoneNumberCityCode")]
        public int PhoneNumberCityCode { get; set; }

        [XmlElement(ElementName = "PhoneNumberLocalCode")]
        public double PhoneNumberLocalCode { get; set; }
    }

    [XmlRoot(ElementName = "CustomerPhoneEmailInformation")]
    public class CustomerPhoneEmailInformation
    {
        [XmlElement(ElementName = "phoneEmailInfo")]
        public List<PhoneEmailInfo> PhoneEmailInfo { get; set; }
    }

    [XmlRoot(ElementName = "CurrencyInformation")]
    public class CurrencyInformation
    {
        [XmlElement(ElementName = "Currency")]
        public List<string> Currency { get; set; }
    }

    [XmlRoot(ElementName = "CustomerInformation")]
    public class CustomerInformation
    {
        [XmlElement(ElementName = "AccountNumber")]
        public int AccountNumber { get; set; }

        [XmlElement(ElementName = "Bvn")]
        public double Bvn { get; set; }

        [XmlElement(ElementName = "IsRetailCustomerFlag")]
        public int IsRetailCustomerFlag { get; set; }

        [XmlElement(ElementName = "RiskGrading")]
        public string RiskGrading { get; set; }

        [XmlElement(ElementName = "ContactDetails")]
        public ContactDetails ContactDetails { get; set; }

        [XmlElement(ElementName = "Demography")]
        public Demography Demography { get; set; }

        [XmlElement(ElementName = "CustomerAddressInformation")]
        public CustomerAddressInformation CustomerAddressInformation { get; set; }

        [XmlElement(ElementName = "CorporateCustomerInformation")]
        public CorporateCustomerInformation CorporateCustomerInformation { get; set; }

        [XmlElement(ElementName = "RetailCustomerInformation")]
        public RetailCustomerInformation RetailCustomerInformation { get; set; }

        [XmlElement(ElementName = "CustomerDocumentData")]
        public CustomerDocumentData CustomerDocumentData { get; set; }

        [XmlElement(ElementName = "CustomerGeneralData")]
        public CustomerGeneralData CustomerGeneralData { get; set; }

        [XmlElement(ElementName = "CustomerPhoneEmailInformation")]
        public CustomerPhoneEmailInformation CustomerPhoneEmailInformation { get; set; }

        [XmlElement(ElementName = "RelatedPartyInformation")]
        public object RelatedPartyInformation { get; set; }

        [XmlElement(ElementName = "CurrencyInformation")]
        public CurrencyInformation CurrencyInformation { get; set; }
    }

    [XmlRoot(ElementName = "AccountInformation")]
    public class AccountInformation
    {
        [XmlElement(ElementName = "AccountBranchId")]
        public int AccountBranchId { get; set; }

        [XmlElement(ElementName = "AccountBranchName")]
        public string AccountBranchName { get; set; }

        [XmlElement(ElementName = "AccountBvn")]
        public double AccountBvn { get; set; }

        [XmlElement(ElementName = "AccountCurrencyCode")]
        public string AccountCurrencyCode { get; set; }

        [XmlElement(ElementName = "AccountName")]
        public string AccountName { get; set; }

        [XmlElement(ElementName = "AccountNumber")]
        public int AccountNumber { get; set; }

        [XmlElement(ElementName = "AccountManager")]
        public string AccountManager { get; set; }

        [XmlElement(ElementName = "AccountOpenDate")]
        public DateTime AccountOpenDate { get; set; }

        [XmlElement(ElementName = "AccountOwnership")]
        public string AccountOwnership { get; set; }

        [XmlElement(ElementName = "AccountSchemeCode")]
        public string AccountSchemeCode { get; set; }

        [XmlElement(ElementName = "AccountSchemeType")]
        public string AccountSchemeType { get; set; }

        [XmlElement(ElementName = "AccountSchemeDescription")]
        public string AccountSchemeDescription { get; set; }

        [XmlElement(ElementName = "AccountShortName")]
        public string AccountShortName { get; set; }

        [XmlElement(ElementName = "AccountStatus")]
        public string AccountStatus { get; set; }

        [XmlElement(ElementName = "AccountType")]
        public string AccountType { get; set; }

        [XmlElement(ElementName = "AvailableBalance")]
        public double AvailableBalance { get; set; }

        [XmlElement(ElementName = "CustomerFirstName")]
        public string CustomerFirstName { get; set; }

        [XmlElement(ElementName = "CustomerId")]
        public int CustomerId { get; set; }

        [XmlElement(ElementName = "CustomerLastName")]
        public string CustomerLastName { get; set; }

        [XmlElement(ElementName = "CustomerMiddleName")]
        public string CustomerMiddleName { get; set; }

        [XmlElement(ElementName = "CustomerSalutation")]
        public string CustomerSalutation { get; set; }

        [XmlElement(ElementName = "LedgerBalance")]
        public double LedgerBalance { get; set; }
    }

    [XmlRoot(ElementName = "doCustomerInformationEnquiryResponse")]
    public class DoCustomerInformationEnquiryResponse
    {
        [XmlElement(ElementName = "SinkTransactionRef")]
        public string SinkTransactionRef { get; set; }

        [XmlElement(ElementName = "ResponseCode")]
        public int ResponseCode { get; set; }

        [XmlElement(ElementName = "ResponseStatus")]
        public string ResponseStatus { get; set; }

        [XmlElement(ElementName = "ResponseDescription")]
        public string ResponseDescription { get; set; }

        [XmlElement(ElementName = "CustomerInformation")]
        public CustomerInformation CustomerInformation { get; set; }

        [XmlElement(ElementName = "AccountInformation")]
        public AccountInformation AccountInformation { get; set; }

        [XmlAttribute(AttributeName = "ns2")]
        public string Ns2 { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
}
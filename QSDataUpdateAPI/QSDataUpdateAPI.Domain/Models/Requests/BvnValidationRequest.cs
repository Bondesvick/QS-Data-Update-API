using System;
using System.Collections.Generic;
using System.Text;

namespace QSDataUpdateAPI.Domain.Models.Requests
{
    public class BvnValidationRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        public string BvnId { get; set; }
    }
}

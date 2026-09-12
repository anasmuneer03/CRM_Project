using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Request.Leads
{
    public class ConvertToCustomerRequest
    {
        public string Address { get; set; }
        public string? Phone { get; set; }

    }
}

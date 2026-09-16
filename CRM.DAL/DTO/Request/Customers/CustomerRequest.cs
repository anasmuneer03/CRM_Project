using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Request.Customers
{
    public class CustomerRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? CompanyName { get; set; }
        public string Address { get; set; }
        public string? AssignedAgentId { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Lead> AssignedLeads { get; set; } = new List<Lead>();
        public ICollection<Customer> AssignedCustomers { get; set; } = new List<Customer>();
        public ICollection<Opportunity> AssignedOpportunities { get; set; } = new List<Opportunity>();
        public ICollection<Sale> AssignedSales { get; set; } = new List<Sale>();
        public ICollection<CrmTask> AssignedTasks { get; set; } = new List<CrmTask>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}

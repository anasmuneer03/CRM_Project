using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Request.Invoices
{
    public class UpdateInvoiceRequest
    {
        public decimal? Amount { get; set; }
        public DateTime? DueDate { get; set; }
    }
}

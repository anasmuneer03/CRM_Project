using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Response.Authentication
{
    public class ResetPasswordResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}

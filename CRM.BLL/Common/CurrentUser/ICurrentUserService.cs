using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Common.CurrentUser
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        bool IsInRole(string roleName);
    }
}

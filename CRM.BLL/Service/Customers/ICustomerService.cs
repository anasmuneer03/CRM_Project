using CRM.BLL.Common;
using CRM.DAL.DTO.Request.Customers;
using CRM.DAL.DTO.Response.Customers;
using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Service.Customers
{
    public interface ICustomerService
    {
        Task<List<CustomerResponse>> GetAllCustomers();
        Task<CustomerResponse?> GetCustomer(Expression<Func<Customer, bool>> filter);
        Task<CustomerResponse> CreateCustomer(CustomerRequest request);
        Task<ServiceResult<CustomerResponse>> UpdateCustomer(int id, UpdateCustomerRequest request);
        Task<ServiceResult<bool>> UpdateCustomerStatus(int id, UpdateCustomerStatusRequest request);
        Task<ServiceResult<bool>> DeleteCustomer(int id);
    }
}

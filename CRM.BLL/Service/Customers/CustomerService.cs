using CRM.BLL.Common;
using CRM.DAL.DTO.Request.Customers;
using CRM.DAL.DTO.Response.Customers;
using CRM.DAL.DTO.Response.Leads;
using CRM.DAL.Models;
using CRM.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace CRM.BLL.Service.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _uow;
        public CustomerService(IUnitOfWork uow) 
        {
            _uow = uow;
        }
        public async Task<List<CustomerResponse>> GetAllCustomers()
        {
            var customers = await _uow.Repository<Customer>().GetAllAsync(
                includes : new string[] {nameof(Customer.AssignedAgent)});
            return customers.Adapt<List<CustomerResponse>>();
        }

        public async Task<CustomerResponse?> GetCustomer(Expression<Func<Customer, bool>> filter)
        {
            var customer = await _uow.Repository<Customer>().GetOneAsync(filter);
            return customer?.Adapt<CustomerResponse>();
        }

        public async Task<CustomerResponse> CreateCustomer(CustomerRequest request)
        {
            var customer = request.Adapt<Customer>();
            await _uow.Repository<Customer>().CreateAsync(customer);
            await _uow.SaveChangesAsync();
            return customer.Adapt<CustomerResponse>();
        }

        public async Task<ServiceResult<CustomerResponse>> UpdateCustomer(int id, UpdateCustomerRequest request)
        {
            var customer = await _uow.Repository<Customer>().GetOneAsync(c => c.Id == id);
            if(customer == null)
            {
                return ServiceResult<CustomerResponse>.NotFound("Customer not found");
            }
            if(request.FullName != null)
                customer.FullName = request.FullName;
            
            if (request.Email != null)
                customer.Email = request.Email;
            
            if (!string.IsNullOrWhiteSpace(request.Phone))
                customer.Phone = request.Phone;

            if (!string.IsNullOrWhiteSpace(request.Address))
                customer.Address = request.Address;
            
            if (request.CompanyName != null)
                customer.CompanyName = string.IsNullOrWhiteSpace(request.CompanyName) ? null : request.CompanyName;
            
            if (request.AssignedAgentId != null)
                customer.AssignedAgentId = string.IsNullOrWhiteSpace(request.AssignedAgentId) ? null : request.AssignedAgentId;

            _uow.Repository<Customer>().Update(customer);
            var affected = await _uow.SaveChangesAsync();

            if (affected == 0)
                return ServiceResult<CustomerResponse>.Fail("No changes were made");

            var updated = await GetCustomer(c => c.Id == id);

            return updated is not null ?
                ServiceResult<CustomerResponse>.Ok(updated, "Customer updated successfully")
                : ServiceResult<CustomerResponse>.Fail("Customer was updated but could not be reloaded");

        }

        public async Task<ServiceResult<bool>> UpdateCustomerStatus(int id, UpdateCustomerStatusRequest request)
        {
            var customer = await _uow.Repository<Customer>().GetOneAsync(c => c.Id == id);

            if (customer is null)
                return ServiceResult<bool>.NotFound("Customer not found");

            if (request.CustomerStatus == CustomerStatusEnum.Churned && string.IsNullOrWhiteSpace(request.ChurnReason))
                return ServiceResult<bool>.Fail("A reason is required when marking a Customer as Churned");

            customer.CustomerStatus = request.CustomerStatus;
            customer.ChurnReason = request.CustomerStatus == CustomerStatusEnum.Churned ? request.ChurnReason : null;

            _uow.Repository<Customer>().Update(customer);
            var affected = await _uow.SaveChangesAsync();

            return affected > 0
                ? ServiceResult<bool>.Ok(true, "Customer status updated.")
                : ServiceResult<bool>.Fail("Failed to update customer status.");
        }

        public async Task<ServiceResult<bool>> DeleteCustomer(int id)
        {
            var customer = await _uow.Repository<Customer>().GetOneAsync(c=> c.Id == id);
            if (customer is null)
                return ServiceResult<bool>.NotFound("customer not found.");

            customer.EntityStatus = EntityStatusEnum.InActive;
            _uow.Repository<Customer>().Update(customer);
            var affected = await _uow.SaveChangesAsync();

            return affected > 0 ?
                ServiceResult<bool>.Ok(true, "customer deleted successfully.") 
                : ServiceResult<bool>.Fail("Failed to delete customer.");
        }
    }
}

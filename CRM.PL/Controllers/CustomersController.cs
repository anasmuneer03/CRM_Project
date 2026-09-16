using CRM.BLL.Service.Customers;
using CRM.DAL.DTO.Request.Customers;
using CRM.DAL.Models;
using CRM.PL.Common;
using CRM.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CRM.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController :BaseApiController
    {
        private readonly ICustomerService _customerService;
        private readonly IStringLocalizer _stringLocalizer;
        public CustomersController(ICustomerService customerService,
            IStringLocalizer<SharedResources> stringLocalizer)
        {
            _customerService = customerService;
            _stringLocalizer = stringLocalizer;
        }
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerService.GetAllCustomers();
            return SuccessResponse(customers, _stringLocalizer["Success"].Value);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var customer = await _customerService.GetCustomer(c => c.Id == id);
            if (customer is null)
                return NotFoundResponse(_stringLocalizer["CustomerNotFound"].Value);
            return SuccessResponse(customer, _stringLocalizer["Success"].Value);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Create(CustomerRequest request)
        {
            var created = await _customerService.CreateCustomer(request);
            return CreatedResponse(nameof(GetOne), new { id = created.Id }, created, _stringLocalizer["Created"].Value);

        }

        [HttpPatch("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UpdateCustomerRequest request)
        {
            var updated = await _customerService.UpdateCustomer(id, request);
            if (!updated.Success)
            {
                return updated.IsNotFound ? NotFoundResponse(updated.Message) :
                    BadRequestResponse(updated.Message);
            }
            return SuccessResponse(updated.Data ,updated.Message);

        }

        [HttpPatch("{id:int}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateStatus(int id, UpdateCustomerStatusRequest request)
        {
            var statusUpdated = await _customerService.UpdateCustomerStatus(id, request);
            if (!statusUpdated.Success)
            {
                return statusUpdated.IsNotFound ? NotFoundResponse(statusUpdated.Message) :
                   BadRequestResponse(statusUpdated.Message);
            }
            return SuccessResponse(statusUpdated.Message);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _customerService.DeleteCustomer(id);
            if (!deleted.Success)
            {
                return deleted.IsNotFound
                    ? NotFoundResponse(deleted.Message)
                    : BadRequestResponse(deleted.Message);
            }
            return SuccessResponse(deleted.Message);
        }
    }
}

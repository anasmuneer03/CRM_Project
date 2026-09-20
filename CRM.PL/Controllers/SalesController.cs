using CRM.BLL.Service.Sales;
using CRM.DAL.DTO.Request.Opportunities;
using CRM.DAL.DTO.Request.Sales;
using CRM.PL.Common;
using CRM.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace CRM.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : BaseApiController
    {
        private readonly ISaleService _saleService;
        private readonly IStringLocalizer _stringLocalizer;
        public SalesController(ISaleService saleService,
            IStringLocalizer<SharedResources> stringLocalizer)
        {
            _saleService = saleService;
            _stringLocalizer = stringLocalizer;
        }
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var sales = await _saleService.GetAllSales();
            return SuccessResponse(sales, _stringLocalizer["Success"].Value);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var sale = await _saleService.GetSale(s=> s.Id == id);
            if (sale == null)
                return NotFoundResponse("Sale not found");
            return SuccessResponse(sale, _stringLocalizer["Success"].Value);
        }

        [HttpPatch("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UpdateSaleRequest request)
        {
            var updated = await _saleService.UpdateSale(id, request);
            if (!updated.Success)
            {
                return updated.IsNotFound ? NotFoundResponse(updated.Message) :
                   BadRequestResponse(updated.Message);
            }
            return SuccessResponse(updated.Data, updated.Message);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _saleService.DeleteSale(id);
            if (!deleted.Success)
            {
                return deleted.IsNotFound ? NotFoundResponse(deleted.Message) :
                   BadRequestResponse(deleted.Message);
            }
            return SuccessResponse(deleted.Message);
        }
    }
}

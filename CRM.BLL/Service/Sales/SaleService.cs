using CRM.BLL.Common;
using CRM.DAL.DTO.Request.Sales;
using CRM.DAL.DTO.Response.Opportunities;
using CRM.DAL.DTO.Response.Sales;
using CRM.DAL.Models;
using CRM.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Service.Sales
{
    public class SaleService :ISaleService
    {
        private readonly IUnitOfWork _uow;
        private readonly string [] Includes = { nameof(Sale.Opportunity), nameof(Sale.Customer)
                , nameof(Sale.AssignedAgent), nameof(Sale.Invoices) };
        public SaleService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<SaleResponse>> GetAllSales()
        {
            var sales = await _uow.Repository<Sale>().GetAllAsync(includes: Includes);
            return sales.Adapt<List<SaleResponse>>();
        }

        public async Task<SaleResponse?> GetSale(Expression<Func<Sale, bool>> filter)
        {
            var sale = await _uow.Repository<Sale>().GetOneAsync(filter, includes: Includes);
            return sale?.Adapt<SaleResponse>();
        }

        public async Task<ServiceResult<SaleResponse>> UpdateSale(int id, UpdateSaleRequest request)
        {
            var sale = await _uow.Repository<Sale>().GetOneAsync(s => s.Id == id);

            if(sale == null)
                return ServiceResult<SaleResponse>.NotFound("Sale not found");

            if (request.AssignedAgentId != null)
                sale.AssignedAgentId = string.IsNullOrWhiteSpace(request.AssignedAgentId) ? null : request.AssignedAgentId;

            _uow.Repository<Sale>().Update(sale);
            var affected = await _uow.SaveChangesAsync();

            if (affected == 0)
                return ServiceResult<SaleResponse>.Fail("No changes were made");

            var updated = await GetSale(s => s.Id == id);
            return updated is not null ?
               ServiceResult<SaleResponse>.Ok(updated, "Sale updated successfully")
               : ServiceResult<SaleResponse>.Fail("Sale was updated but could not be reloaded");
        }

        public async Task<ServiceResult<bool>> DeleteSale(int id)
        {

            var sale = await _uow.Repository<Sale>().GetOneAsync(s => s.Id == id);

            if (sale is null)
                return ServiceResult<bool>.NotFound("Sale not found");

            if (sale.Invoices.Any(i => i.EntityStatus != EntityStatusEnum.InActive))
                return ServiceResult<bool>.Fail("Cannot delete a sale that still has active invoices.");

            sale.EntityStatus = EntityStatusEnum.InActive;
            _uow.Repository<Sale>().Update(sale);
            var affected = await _uow.SaveChangesAsync();

            return affected > 0 ?
                ServiceResult<bool>.Ok(true, "Sale deleted successfully.")
                : ServiceResult<bool>.Fail("Failed to delete sale");
        }
    }
}

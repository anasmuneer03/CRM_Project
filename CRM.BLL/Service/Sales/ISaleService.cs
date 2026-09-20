using CRM.BLL.Common;
using CRM.DAL.DTO.Request.Opportunities;
using CRM.DAL.DTO.Request.Sales;
using CRM.DAL.DTO.Response.Opportunities;
using CRM.DAL.DTO.Response.Sales;
using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Service.Sales
{
    public interface ISaleService
    {
        Task<List<SaleResponse>> GetAllSales();
        Task<SaleResponse?> GetSale(Expression<Func<Sale, bool>> filter);
        Task<ServiceResult<SaleResponse>> UpdateSale(int id,UpdateSaleRequest  request);
        Task<ServiceResult<bool>> DeleteSale(int id);
    }
}

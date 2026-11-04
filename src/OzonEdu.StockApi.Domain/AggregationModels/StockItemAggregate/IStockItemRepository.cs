using System;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.StockApi.Domain.Contracts;

namespace OzonEdu.StockApi.Domain.AggregationModels.StockItemAggregate
{
    public interface IStockItemRepository: IRepository<StockItem> 
    {
        Task<StockItem> CreateAsync(StockItem stockItem, CancellationToken cancellationToken = default);
        Task<StockItem> UpdateAsync(StockItem stockItem, CancellationToken cancellationToken = default);
        Task<StockItem> FindByIdAsync(Sku sku,  CancellationToken cancellationToken = default);
        Task<StockItem> FindBySkuAsync(Sku sku,  CancellationToken cancellationToken = default);
    }
}
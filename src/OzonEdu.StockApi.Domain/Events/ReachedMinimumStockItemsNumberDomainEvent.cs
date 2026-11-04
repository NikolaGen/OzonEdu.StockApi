using MediatR;
using OzonEdu.StockApi.Domain.AggregationModels.StockItemAggregate;

namespace OzonEdu.StockApi.Domain.Events
{
    public class ReachedMinimumStockItemsNumberDomainEvent : INotification
    {
        public Sku StockItemsSku { get; }

        public ReachedMinimumStockItemsNumberDomainEvent(Sku stockItemsSku)
        {
            StockItemsSku = stockItemsSku;
        }
    }
}
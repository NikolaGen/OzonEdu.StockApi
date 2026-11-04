using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OzonEdu.StockApi.Domain.AggregationModels.StockItemAggregate;
using OzonEdu.StockApi.Domain.Contracts;
using OzonEdu.StockApi.Infrastructure.Commands.GiveOutStockItem;

namespace OzonEdu.StockApi.Infastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfastructure(this IServiceCollection service)
        {
            service.AddMediatR(typeof(CreateStockItemCommand).Assembly);
            service.AddScoped<IStockItemRepository, Repo>();

            return service;
        }
    }

    public class Repo : IStockItemRepository /// Это просто заглушка (созданный класс) Так делать не надо, надо смокировать нормально репозиторий
    {
        public IUnitOfWork UnitOfWork { get; }

        public Task<StockItem> CreateAsync(StockItem stockItem, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<StockItem> UpdateAsync(StockItem stockItem, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<StockItem> FindByIdAsync(Sku sku, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<StockItem> FindBySkuAsync(Sku sku, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
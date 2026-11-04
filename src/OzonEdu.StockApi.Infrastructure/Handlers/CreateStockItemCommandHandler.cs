using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.StockApi.Domain.AggregationModels.StockItemAggregate;
using OzonEdu.StockApi.Infrastructure.Commands.GiveOutStockItem;

namespace OzonEdu.StockApi.Infrastructure.Handlers
{
    public class CreateStockItemCommandHandler : IRequestHandler<CreateStockItemCommand, int>
    {
        private readonly  IStockItemRepository _stockItemRepository;

        public CreateStockItemCommandHandler(IStockItemRepository stockItemRepository)
        {
            _stockItemRepository = stockItemRepository;
        }

        public async Task<int> Handle(CreateStockItemCommand request, CancellationToken cancellationToken)
        {
            var stockInDb = await _stockItemRepository.FindBySkuAsync(new Sku(request.Sku), cancellationToken);
            if (stockInDb is not null)
                throw new Exception($"Stock item with sku {request.Sku} already exists ");
            
            var newStockItem = new StockItem(
                new Sku(request.Sku),
                new Name(request.Name),
                new Item(request.StockItemType),
                request.ClothingSize,
                new Quantity(request.Quantity),
                new Quantity((int)request.MinimalQuantity));

            var createResult = await _stockItemRepository.CreateAsync(newStockItem, cancellationToken);
            await  _stockItemRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            
            return newStockItem.Id;
        }
    }
}
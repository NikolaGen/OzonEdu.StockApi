using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using OzonEdu.StockApi.Controllers;
using OzonEdu.StockApi.Grpc;
using OzonEdu.StockApi.Services;


namespace OzonEdu.StockApi.GrpcServices
{
    public class StockApiGrpcService: StockApiGrpc.StockApiGrpcBase

    {
        private readonly IStockService _stockService;

        public StockApiGrpcService(IStockService stockService)
        {
            _stockService = stockService;
        }

        public override async Task<GetAllStockItemsResponse> GetAllStockItems(
            GetAllStockItemsRequest request, 
            ServerCallContext context)
        {
            var stockitems = await _stockService.GetAll(context.CancellationToken);
            return new GetAllStockItemsResponse
            {
                Stocks =
                {
                    stockitems.Select(x => new GetAllStockItemsResponseUnit
                    {
                        ItemId = x.ItemId,
                        ItemName = x.ItemName,
                        Quantity = x.Quantity
                    })}
            };
            // return base.GetAllStockItems(request, context); 
        }

        public override async Task<GetAllStockItemsWithNullsResponse> GetAllStockItemsWithNulls(Empty request,
            ServerCallContext context)
        {
            var stockitems = await _stockService.GetAll(context.CancellationToken);
            return new GetAllStockItemsWithNullsResponse
            {
                Stocks =
                {
                    stockitems.Select(x => new GetAllStockItemsWithNullsResponseUnit
                    {
                        ItemId = x.ItemId,
                        ItemName = x.ItemName,
                        Quantity = x.Quantity
                    })
                }
            };
        }
            
        public override async Task<GetAllStockItemsMapResponse> GetAllStockItemsMap(Empty request,
            ServerCallContext context)
        {
            var stockitems = await _stockService.GetAll(context.CancellationToken);
            return new GetAllStockItemsMapResponse
            {
                Stocks =
                {
                    stockitems.ToDictionary(x=>x.ItemId, x => new GetAllStockItemsResponseUnit
                    {
                        ItemId = x.ItemId,
                        ItemName = x.ItemName,
                        Quantity = x.Quantity
                    })
                }
            };
        }

        public override Task<Empty> AddStockItem(AddStockItemRequest request, ServerCallContext context)
        {
            throw new RpcException(
                new Status(StatusCode.InvalidArgument, "validation failed"),
                new Metadata {new Metadata.Entry("key", " ourvalue")});
        }

        public override async Task GetAllStockItemsStreaming(
            GetAllStockItemsRequest request,
            IServerStreamWriter<GetAllStockItemsResponseUnit> responseStream,
            ServerCallContext context)
        {
            var stockItems = await _stockService.GetAll(context.CancellationToken);
            foreach (var item in stockItems)
            {
                if (context.CancellationToken.IsCancellationRequested)
                {
                    break;
                }

                await responseStream.WriteAsync(new GetAllStockItemsResponseUnit
                {
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    Quantity = item.Quantity
                });
            }
        }

        public override async Task<Empty> AddStockItemStreaming(IAsyncStreamReader<AddStockItemRequest> requestStream,
            ServerCallContext context)
        {
            while (!context.CancellationToken
                   .IsCancellationRequested) //пока не получили отмену закрытия стрима (со стороны пользователя)
            {
                await requestStream.MoveNext();
                var currentItem = requestStream.Current;

                await _stockService.Add(new StockItemCreationModel
                {
                    Quantity = currentItem.Quantity,
                    ItemName = currentItem.ItemName
                }, context.CancellationToken);
            }
            return new Empty();
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.StockApi.HttpModels;
using OzonEdu.StockApi.Services;

namespace OzonEdu.StockApi.Controllers
{
    [ApiController]
    [Route("v2/api/stocks")]
    public class V2StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public V2StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpPost]
        public async Task<ActionResult<StockItem>> Add(StockItemModelV2 model, CancellationToken token)
        {
            var createdStockItem = await _stockService.Add(new StockItemCreationModel
            {
                ItemName = model.ItemName,
                Quantity = model.Quantity.Value
            }, token);
            return Ok(createdStockItem);
        }
    }
}
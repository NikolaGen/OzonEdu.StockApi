using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.StockApi.HttpModels;
using OzonEdu.StockApi.Infrastructure.Commands.GiveOutStockItem;
using OzonEdu.StockApi.Services;

namespace OzonEdu.StockApi.Controllers
{
    [ApiController]
    [Route("v1/api/stocks")]
    [Produces("application/json")] // эта строчка нужна чтобы определить в каком формате Swagger будет возвращать наш успешный результат
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly IMediator _mediator;

        public StockController(IStockService stockService,  IMediator mediator)
        {
            _stockService = stockService;
            _mediator = mediator;
        }

        // 
        [HttpGet]
        [ProducesResponseType(typeof(List<StockItem>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken token) // ActionResult<List<StockItem>> - так было вместо Task<IActionResult>, но чтобы так работало мы добавили строчку кода (1 строка выше) полностью (это нужно для работы в Swagger)
        {
            var stockItems = await _stockService.GetAll(token);
            return Ok(stockItems);
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(StockItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProdecesResponceType(404)] - можно писать и так вместо строчки выше
        public async Task<ActionResult<StockItem>> GetById(long id, CancellationToken token)
        {
            var stockItem = await _stockService.GetById(id, token);
            if (stockItem is null)
            {
                return NotFound();
            }

            return Ok(stockItem);
        }

        /*[HttpPost]
        public async Task<ActionResult<StockItem>> Add(StockItemModel model, CancellationToken token)
        {
            throw new CustomException();
            var createdStockItem = await _stockService.Add(new StockItemCreationModel
            {
                ItemName = model.ItemName,
                Quantity = model.Quantity
            }, token);
            return Ok(createdStockItem);
        }*/

        [HttpPost]
        public async Task<ActionResult<int>> Add(StockItemModel model, CancellationToken token)
        {
            var createStockItemCommand = new CreateStockItemCommand()
            {
                Name = model.ItemName
                // ... что то добавить надо
            };
            
            var result = await _mediator.Send(createStockItemCommand, token);
            
            return Ok(result);
        }
        
        [HttpGet("boom")]
        public IActionResult Boom()
        {
            throw new Exception("Тестовое исключение для проверки фильтра");
        }
    }

    public class CustomException : Exception
    {
        public CustomException() : base("some custom exception")
        {
            
        }
    }
}

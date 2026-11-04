using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.StockApi.Services;

namespace OzonEdu.StockApi.Services
{
    public interface IStockService
    {
        Task<List<StockItem>> GetAll(CancellationToken token);

        Task<StockItem> GetById(long itemId, CancellationToken _);
        
        Task<StockItem> Add(StockItemCreationModel stockItem, CancellationToken _);
    }
    
    public class StockService : IStockService
    {
        private readonly List<StockItem> StockItems = new List<StockItem>
        {
            new StockItem(itemId: 1, itemName: "Футболка", quantity: 10),
            new StockItem(itemId: 2, itemName: "Толстовка", quantity: 20),
            new StockItem(itemId: 3, itemName: "Кепка", quantity: 15)
        };

        public Task<List<StockItem>> GetAll(CancellationToken _)
        {
            return Task.FromResult(StockItems);
        }

        public Task<StockItem> GetById(long itemId, CancellationToken _)
        {
            var stockItem = StockItems.FirstOrDefault(x => x.ItemId == itemId);
            return Task.FromResult(stockItem);
        }

        public Task<StockItem> Add(StockItemCreationModel stockItem, CancellationToken _)
        {
            var itemId = StockItems.Max(x => x.ItemId) + 1;
            var newStockItem = new StockItem(itemId, stockItem.ItemName, stockItem.Quantity);
            StockItems.Add(newStockItem);
            return Task.FromResult(newStockItem);
        }
    }
    
    public class StockItemCreationModel
    {
        public string ItemName { get; set; }
        
        public int Quantity { get; set; }
    }

    public class StockItem
    {
        public StockItem(long itemId, string itemName, int quantity)
        {
            ItemId = itemId;
            ItemName = itemName;
            Quantity = quantity;
        }
        public long ItemId { get; }
        
        public string ItemName { get; }
        
        public int Quantity { get; }
    }
}
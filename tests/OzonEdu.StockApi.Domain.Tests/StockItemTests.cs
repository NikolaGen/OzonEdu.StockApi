using System;
using OzonEdu.StockApi.Domain.AggregationModels.StockItemAggregate;
using Xunit;

namespace OzonEdu.StockApi.Domain.Tests
{
    public class StockItemTests
    {
        // можно сразу тут сделать private readonly stockItem = new StockItem(...). То есть сразу создать 1 объект глобально для всех тестов
        
        [Fact]
        public void IncreaseStockItemQuantity()
        {
            //Arrange
            var stockItem = new StockItem(
                new Sku(149568),
                new Name("Super puper Tshirt"),
                new Item(ItemType.TShirt),
                ClothingSize.S,
                new Quantity(10),
                new Quantity(5));

            var valueToIncrese = 10;
            
            //Act
            stockItem.IncreaseQuantity(valueToIncrese);

            //Assert
            Assert.Equal(20, stockItem.Quantity.Value);
        }
        
        [Fact]
        public void IncreaseQuantityNegativeSuccess()
        {
            //Arrange
            var stockItem = new StockItem(
                new Sku(149568),
                new Name("Super puper Tshirt"),
                new Item(ItemType.TShirt),
                ClothingSize.S,
                new Quantity(10),
                new Quantity(5));

            var valueToIncrease = -10;
            
            //Act
            

            //Assert
            // Assert.Throws<Exception>(() => stockItem.IncreaseQuantity(valueToIncrease)); - одинаковая запись
            Assert.Throws<Exception>(() =>
            {
                stockItem.IncreaseQuantity(valueToIncrease);
            });
        }
    }
}
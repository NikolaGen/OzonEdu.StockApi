using System;
using OzonEdu.StockApi.Domain.Events;
using OzonEdu.StockApi.Domain.Models;

namespace OzonEdu.StockApi.Domain.AggregationModels.StockItemAggregate
{
    public class StockItem : Entity
    {
        public StockItem(Sku sku, Name name, Item itemType, ClothingSize size, Quantity quantity, Quantity minimalQuantity)
        {
            Sku = sku;
            Name = name;
            ItemType = itemType;
            SetClothingSize(size);
            /*Quantity = quantity;
            MinimalQuantity = minimalQuantity;*/
            
        }
        
        public Sku Sku { get; }
        public Name Name { get; }
        public Item ItemType { get; }
        public ClothingSize ClothingSize { get; private set; }
        public Quantity Quantity { get; private set; }
        public Quantity MinimalQuantity { get; private set; }
        public Tag Tag { get; set; }

        public void IncreaseQuantity(int valueToIncrease)
        {
            if (valueToIncrease < 0)
                throw new Exception($"{nameof(valueToIncrease)} value is negative");
            Quantity = new Quantity(this.Quantity.Value + valueToIncrease);
        }

        public void GiveOutItems(int valueToGiveOut)
        {
            if (valueToGiveOut < 0)
                throw new Exception($"{nameof(valueToGiveOut)} value is negative");
            if (valueToGiveOut > Quantity.Value)
                throw new Exception("Not enough Items"); 
            
            Quantity = new Quantity(Quantity.Value - valueToGiveOut);
            
            if (Quantity.Value <= MinimalQuantity.Value)
                AddReachedMinimumDomainEvent(Sku);
        }

        public void SetClothingSize(ClothingSize size)
        {
            if (size is not null && (
                    ItemType.Type.Equals(StockItemAggregate.ItemType.TShirt) ||
                    ItemType.Type.Equals(StockItemAggregate.ItemType.Sweatshirt)))
                ClothingSize = size;
            else if (size is null)
                ClothingSize = size;
            else
            {
                throw  new Exception($"Item with type {ItemType.Type.Name} cannot get size");
            }
        }

        private void AddReachedMinimumDomainEvent(Sku sku)
        {
            var orederStartedDomainEvent = new ReachedMinimumStockItemsNumberDomainEvent(sku);
            
            this.AddDomainEvent(orederStartedDomainEvent);
        }
    }
}
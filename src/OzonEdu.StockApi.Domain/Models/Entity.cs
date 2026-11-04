using System;
using System.Collections.Generic;
using MediatR;

namespace OzonEdu.StockApi.Domain.Models
{
    public abstract class  Entity
    {
        int? _requestedHashCode;
        
        public virtual int Id { get; protected set; }
        
        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents ??= new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
        
        public bool IsTransient()
        {
            return this.Id == default(Int32);
        }

        public override bool Equals(object obj)
        {
            if (obj is not Entity  entity)
                return false;
            
            if (object.ReferenceEquals(this, entity))
                return true;
            
            if (this.GetType() != entity.GetType())
                return false;
            
            if (entity.IsTransient() || this.IsTransient())
                return false;
            else 
                return entity.Id == this.Id;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OzonEdu.StockApi.Domain.Models
{
    public abstract class Enumeration : IComparable
    {
        public string Name { get; private set; }
        
        public int Id {get; private set;}
        
        protected Enumeration(int id, string name)
        {
            (Id, Name) = (id, name);
        }

        public override string ToString()
        {
            return Name;
        }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            return typeof(T).GetFields(BindingFlags.Public |
                                       BindingFlags.Static |
                                       BindingFlags.DeclaredOnly)
                .Select(f => f.GetValue(null))
                .Cast<T>();
        }

        public override bool Equals(object obj)
        {
            if (obj is not Enumeration otherValue)
            {
                return false;
            }
            
            var typeMathes = GetType().Equals(obj.GetType());
            var valueMathes = Id.Equals(otherValue.Id);
            
            return typeMathes && valueMathes;
        }
        
        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);
    }
}
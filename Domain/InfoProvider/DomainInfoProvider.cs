using System;
using System.Collections.Generic;
using System.Linq;

namespace UserOrdersApiDemo.Domain
{
    public static class DomainInfoProvider
    {
        // Quick solution to get all IEntities
        public static IEnumerable<Type> GetIEntitiesTypes() => typeof(IEntity)
            .Assembly
            .GetTypes()
            .Where(type => typeof(IEntity).IsAssignableFrom(type) && typeof(IEntity) != type);
    }
}
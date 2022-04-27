using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UserOrdersApiDemo.Application
{
    public class ValueMapBuilder
    {
        private readonly ValueMapper mapper;
        private List<ValueMapperRule> map;

        public ValueMapBuilder(ValueMapper mapper, List<ValueMapperRule> map)
        {
            this.mapper = mapper;
            this.map = map;
        }

        public ValueMapBuilder FlatClasses()
        {
            map.Add(new ValueMapperRule(
                prop => prop.PropertyType.IsClass
                        && prop.PropertyType.GetConstructor(Type.EmptyTypes) is not null
                        && prop.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)) == false,
                (prop, target, source) =>
                {
                    prop.SetValue(target,
                        mapper.Map(
                            source.GetType().GetProperty(prop.Name)?.GetValue(source),
                            prop.PropertyType));
                }
            ));
            return this;
        }

        public ValueMapBuilder FlatCollections()
        {
            map.Add(new ValueMapperRule(
                prop => prop.PropertyType.IsGenericType && prop.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)),
                (prop, target, source) =>
                {
                    var enumerableType = prop.PropertyType
                        .GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        .GetGenericArguments()[0];

                    var collectionType = typeof(List<>)
                        .GetGenericTypeDefinition()
                        .MakeGenericType(enumerableType);

                    prop.SetValue(target, Activator.CreateInstance(collectionType));

                    var sourceCollection = (IEnumerable)source.GetType().GetProperty(prop.Name)?.GetValue(source);

                    if (sourceCollection is not null)
                    {
                        foreach (var item in sourceCollection)
                        {
                            ((IList)prop.GetValue(target)).Add(mapper.Map(item, enumerableType));
                        }
                    }
                }
            ));
            return this;
        }
    }
}
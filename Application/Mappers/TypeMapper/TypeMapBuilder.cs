using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UserOrdersApiDemo.Application
{
    public class TypeMapBuilder
    {
        private readonly TypeMapper mapper;
        private List<TypeMapperRule> map;

        public TypeMapBuilder(TypeMapper mapper, List<TypeMapperRule> map)
        {
            this.mapper = mapper;
            this.map = map;
        }

        public TypeMapBuilder IgnoreName(string name)
        {
            map.Add(new TypeMapperRule(
                prop => prop.Name == name,
                (prop, builder) => { }
            ));
            return this;
        }

        public TypeMapBuilder IgnoreClasses()
        {
            map.Add(new TypeMapperRule(
                prop => prop.PropertyType.IsClass,
                (prop, builder) => { }
            ));
            return this;
        }

        public TypeMapBuilder IgnoreInterfaces()
        {
            map.Add(new TypeMapperRule(
                prop => prop.PropertyType.IsInterface,
                (prop, builder) => { }
            ));
            return this;
        }

        public TypeMapBuilder KeepStrings()
        {
            map.Add(new TypeMapperRule(
                prop => prop.PropertyType == typeof(string),
                (prop, builder) => builder.CreateProperty(prop.PropertyType, prop.Name, prop.CustomAttributes),
                0
            ));
            return this;
        }

        public TypeMapBuilder IgnoreNamePattern(string pattern)
        {
            map.Add(new TypeMapperRule(
                prop => new Regex(pattern).IsMatch(prop.Name),
                (prop, builder) => { }
            ));
            return this;
        }

        public TypeMapBuilder FlatClasses()
        {
            map.Add(new TypeMapperRule(
                prop => prop.PropertyType.IsClass 
                        && prop.PropertyType.GetConstructor(Type.EmptyTypes) is not null
                        && prop.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)) == false,
                (prop, builder) => builder.CreateProperty(mapper.Map(prop.PropertyType, "Flat"), prop.Name, prop.CustomAttributes)
            ));
            return this;
        }

        public TypeMapBuilder FlatCollections()
        {
            map.Add(new TypeMapperRule(
                prop => prop.PropertyType.IsGenericType && prop.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)),
                (prop, builder) =>
                {
                    var collectionType = prop.PropertyType
                        .GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        .GetGenericArguments()[0];

                    var listType = typeof(ICollection<>)
                        .GetGenericTypeDefinition()
                        .MakeGenericType(mapper.Map(collectionType, "Flat"));
                    builder.CreateProperty(listType, prop.Name, prop.CustomAttributes);
                }
            ));
            return this;
        }
    }
}
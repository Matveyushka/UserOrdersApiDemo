using System;
using System.Collections.Generic;
using System.Linq;

namespace UserOrdersApiDemo.Application
{
    public class ValueMapper
    {
        private Dictionary<(Type, Type), List<ValueMapperRule>> maps
            = new Dictionary<(Type, Type), List<ValueMapperRule>>();

        private List<ValueMapperRule> defaultMap = new List<ValueMapperRule>();

        public object Map(object source, Type targetType)
        {
            if (source is null)
            {
                return null;
            }

            var fitMap = maps.ContainsKey((targetType, source.GetType()))
                ? maps[(targetType, source.GetType())]
                : defaultMap;

            var orderedRules = fitMap.OrderBy(rule => rule.Priority);

            var properties = targetType.GetProperties();

            var instance = Activator.CreateInstance(targetType);

            foreach (var prop in properties)
            {
                var fitRule = orderedRules.FirstOrDefault(rule => rule.Fits(prop));
                if (fitRule is default(ValueMapperRule))
                {
                    fitRule = ValueMapperRule.Default;
                }
                fitRule.Apply(prop, instance, source);
            }

            return instance;
        }

        public T Map<T>(object source) => (T)Map(source, typeof(T));

        public ValueMapBuilder CreateDefaultMap()
        {
            var map = new List<ValueMapperRule>();
            defaultMap = map;
            return new ValueMapBuilder(this, map);
        }

        public ValueMapBuilder CreateMap(Type source, Type target)
        {
            var map = new List<ValueMapperRule>();
            maps.Add((target, source), map);
            return new ValueMapBuilder(this, map);
        }

        public ValueMapper()
        {
            CreateDefaultMap()
                .FlatClasses()
                .FlatCollections();
        }
    }
}
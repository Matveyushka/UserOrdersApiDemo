using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace UserOrdersApiDemo.Application
{
    public class TypeMapperRule
    {
        public int Priority { get; set; }
        public Action<PropertyInfo, TypeBuilder> action;
        public Func<PropertyInfo, bool> fit;

        public TypeMapperRule(
            Func<PropertyInfo, bool> fit,
            Action<PropertyInfo, TypeBuilder> action,
            int priority = 1)
        {
            this.Priority = priority;
            this.action = action;
            this.fit = fit;
        }

        public bool Fits(PropertyInfo property) => this.fit(property);

        public void Apply(PropertyInfo property, TypeBuilder typeBuilder)
        {
            action(property, typeBuilder);
        }

        private static TypeMapperRule defaultRule = new TypeMapperRule(
            _ => true,
            (prop, builder) => builder.CreateProperty(prop.PropertyType, prop.Name, prop.CustomAttributes)
        );

        public static TypeMapperRule Default => defaultRule;
    }
}
using System;
using System.Reflection;

namespace UserOrdersApiDemo.Application
{
    public class ValueMapperRule
    {
        public int Priority { get; set; }
        private Action<PropertyInfo, object, object> action;
        private Func<PropertyInfo, bool> fit;

        public ValueMapperRule(
            Func<PropertyInfo, bool> fit,
            Action<PropertyInfo, object, object> action,
            int priority = 1)
        {
            this.action = action;
            this.fit = fit;
            this.Priority = priority;
        }

        public bool Fits(PropertyInfo property) => this.fit(property);

        public void Apply(PropertyInfo targetProperty, object target, object source)
        {
            action(targetProperty, target, source);
        }

        private static ValueMapperRule defaultRule = new ValueMapperRule(
            _ => true,
            (prop, target, source) =>
            {
                prop.SetValue(target, source.GetType().GetProperty(prop.Name)?.GetValue(source));
            }
        );

        public static ValueMapperRule Default => defaultRule;
    }
}
using System;
using Microsoft.AspNetCore.Mvc.Routing;

namespace UserOrdersApiDemo.WebApi
{
    public class GenericRouteTemplateProvider : IRouteTemplateProvider
    {
        const string templateBegin = "api/entity/";
        private readonly string template;

        public GenericRouteTemplateProvider(Type type)
        {
            this.template = templateBegin + type.Name;
        }

        public string Template => this.template;

        public int? Order => 0;

        public string Name => null;
    }
}
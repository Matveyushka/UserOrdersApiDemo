using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace UserOrdersApiDemo.WebApi
{
    public class GenericControllerRouteConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.IsGenericType &&
                controller.ControllerType.GetGenericTypeDefinition() == typeof(EntityController<,,,>))
            {
                var entityType = controller.ControllerType.GetGenericArguments()[0];

                controller.ControllerName = $"{entityType.Name}";

                var routeTemplateProvider = new GenericRouteTemplateProvider(entityType);
                controller.Selectors.Add(new SelectorModel
                {
                    AttributeRouteModel = new AttributeRouteModel(routeTemplateProvider),
                });
            }
        }
    }
}
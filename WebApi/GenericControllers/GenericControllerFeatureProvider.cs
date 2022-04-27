using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using UserOrdersApiDemo.Application;
using UserOrdersApiDemo.Domain;

namespace UserOrdersApiDemo.WebApi
{
    public class GenericControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly IEnumerable<Type> controllerTypes;

        public GenericControllerFeatureProvider(IEnumerable<Type> controllerTypes)
        {
            this.controllerTypes = controllerTypes;
            foreach (var type in controllerTypes)
            {
                if (type.IsClass == false || type.GetInterfaces().Contains(typeof(IEntity)) == false)
                {
                    throw new Exception();
                }
            }
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var typeMapper = new TypeMapper();
            foreach (var type in controllerTypes)
            {
                var postRequestType = typeMapper.ToCreateRequest(type);
                var putRequestType = typeMapper.ToUpdateRequest(type);
                var dtoType = typeMapper.ToDtoType(type);

                var genericControllerType = typeof(EntityController<,,,>)
                    .MakeGenericType(type, postRequestType, putRequestType, dtoType);

                feature.Controllers.Add(genericControllerType.GetTypeInfo());
            }
        }
    }
}
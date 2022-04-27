using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace UserOrdersApiDemo.Application
{
    public class Mediator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, Type> handlersTypes = new Dictionary<string, Type>();

        public Mediator(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public Output Send<Input, Output>(IMediatorRequest<Input, Output> request)
        {
            var serviceType = typeof(IMediatorHandler<,,>)
                .GetGenericTypeDefinition()
                .MakeGenericType(typeof(Input), request.GetType(), typeof(Output));

            var handler = this._serviceProvider
                .CreateScope()
                .ServiceProvider
                .GetService(serviceType);

            return (Output)serviceType
                .GetMethod("Handle")
                .Invoke(handler, new object[] { request });
        }
    }
}
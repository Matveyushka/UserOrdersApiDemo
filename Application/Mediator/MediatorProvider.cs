using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using UserOrdersApiDemo.Domain;

namespace UserOrdersApiDemo.Application
{
    public static class MediatorProvider
    {
        // This method is not generic enough.
        // TODO: reflection scan for IMediatorHandlers
        public static void AddMediator(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            var entitiesTypes = GetEntityTypes();
            var typeMapper = new TypeMapper();

            foreach (var entityType in entitiesTypes)
            {
                var createType = typeMapper.ToCreateRequest(entityType);
                var updateType = typeMapper.ToUpdateRequest(entityType);
                var dtoType = typeMapper.ToDtoType(entityType);

                var createHandler = typeof(CreateHandler<,>)
                    .GetGenericTypeDefinition()
                    .MakeGenericType(createType, entityType);

                var updateHandler = typeof(UpdateHandler<,>)
                    .GetGenericTypeDefinition()
                    .MakeGenericType(updateType, entityType);

                var deleteHandler = typeof(DeleteHandler<>)
                    .GetGenericTypeDefinition()
                    .MakeGenericType(entityType);

                var readHandler = typeof(ReadHandler<,>)
                    .GetGenericTypeDefinition()
                    .MakeGenericType(entityType, dtoType);

                var readSingleHandler = typeof(ReadSingleHandler<,>)
                    .GetGenericTypeDefinition()
                    .MakeGenericType(entityType, dtoType);

                services.AddScoped(
                    typeof(IMediatorHandler<,,>).GetGenericTypeDefinition().MakeGenericType(
                        createType,
                        typeof(CreateCommand<>).GetGenericTypeDefinition().MakeGenericType(createType),
                        typeof(CommandResponse)),
                    createHandler);

                services.AddScoped(
                    typeof(IMediatorHandler<,,>).GetGenericTypeDefinition().MakeGenericType(
                        updateType,
                        typeof(UpdateCommand<>).GetGenericTypeDefinition().MakeGenericType(updateType),
                        typeof(CommandResponse)),
                    updateHandler);

                services.AddScoped(
                    typeof(IMediatorHandler<,,>).GetGenericTypeDefinition().MakeGenericType(
                        entityType,
                        typeof(DeleteCommand<>).GetGenericTypeDefinition().MakeGenericType(entityType),
                        typeof(CommandResponse)),
                    deleteHandler);

                services.AddScoped(
                    typeof(IMediatorHandler<,,>).GetGenericTypeDefinition().MakeGenericType(
                        typeof(object),
                        typeof(ReadQuery<>).GetGenericTypeDefinition().MakeGenericType(dtoType),
                        typeof(IEnumerable<>).GetGenericTypeDefinition().MakeGenericType(dtoType)),
                    readHandler);

                services.AddScoped(
                    typeof(IMediatorHandler<,,>).GetGenericTypeDefinition().MakeGenericType(
                        typeof(int),
                        typeof(ReadSingleQuery<>).GetGenericTypeDefinition().MakeGenericType(dtoType),
                        dtoType),
                    readSingleHandler);
            }
            services.AddScoped<Mediator>();
        }

        public static IEnumerable<Type> GetEntityTypes()
        {
            var assembly = Assembly.GetAssembly(typeof(IEntity));

            var types = assembly.GetTypes();

            return types.Where(type => typeof(IEntity).IsAssignableFrom(type) && type != typeof(IEntity));
        }
    }
}
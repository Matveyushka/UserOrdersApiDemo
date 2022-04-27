using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace UserOrdersApiDemo.Application
{
    public class TypeMapper
    {
        private static readonly AssemblyName mappedTypesAssemblyName;
        private static readonly ModuleBuilder moduleBuilder;
        private static Dictionary<string, Type> mappedTypes = new Dictionary<string, Type>();

        private Dictionary<string, List<TypeMapperRule>> typelessMaps
            = new Dictionary<string, List<TypeMapperRule>>();

        private Dictionary<(Type, string), List<TypeMapperRule>> maps =
            new Dictionary<(Type, string), List<TypeMapperRule>>();

        private TypeBuilder GetTypeBuilder(string typeName) =>
            moduleBuilder
                .DefineType(typeName, TypeAttributes.Public);

        private Type BuildType(Type sourceType, string typeName, List<TypeMapperRule> buildRules)
        {
            var typeBuilder = GetTypeBuilder(typeName);

            var sourceTypeProperties = sourceType.GetProperties();

            var orderedRules = buildRules.OrderBy(rule => rule.Priority);

            foreach (var prop in sourceTypeProperties)
            {
                var fitRule = orderedRules.FirstOrDefault(rule => rule.Fits(prop));
                if (fitRule is default(TypeMapperRule))
                {
                    fitRule = TypeMapperRule.Default;
                }
                fitRule.Apply(prop, typeBuilder);
            }

            return typeBuilder.CreateType();
        }

        public Type Map(Type sourceType, string targetName)
        {
            var fitMap = maps.ContainsKey((sourceType, targetName))
                ? maps[(sourceType, targetName)]
                : typelessMaps.ContainsKey(targetName)
                ? typelessMaps[targetName]
                : throw new ArgumentException($"There are no fitting maps for type {sourceType.Name} and mapping name {targetName}.");

            var mappingName = $"{sourceType.Name}{targetName}";

            if (mappedTypes.ContainsKey(mappingName) == false)
            {
                mappedTypes.Add(mappingName, BuildType(sourceType, mappingName, fitMap));
            }

            return mappedTypes[mappingName];
        }

        public TypeMapBuilder CreateMap(string mappingName)
        {
            List<TypeMapperRule> map = new List<TypeMapperRule>();
            typelessMaps.Add(mappingName, map);
            return new TypeMapBuilder(this, map);
        }

        public TypeMapBuilder CreateMap<T>(string mappingName)
        {
            List<TypeMapperRule> map = new List<TypeMapperRule>();
            maps.Add((typeof(T), mappingName), map);
            return new TypeMapBuilder(this, map);
        }

        public Type ToDtoType<T>() => this.Map(typeof(T), "Dto");

        public Type ToDtoType(Type source) => this.Map(source, "Dto");

        public Type ToCreateRequest<T>() => this.Map(typeof(T), "Create");

        public Type ToCreateRequest(Type source) => this.Map(source, "Create");

        public Type ToUpdateRequest<T>() => this.Map(typeof(T), "Update");

        public Type ToUpdateRequest(Type source) => this.Map(source, "Update");

        static TypeMapper()
        {
            mappedTypesAssemblyName = new AssemblyName();
            mappedTypesAssemblyName.Name = "MappedTypesAssembly";

            moduleBuilder = AssemblyBuilder
                .DefineDynamicAssembly(
                    mappedTypesAssemblyName,
                    AssemblyBuilderAccess.Run)
                .DefineDynamicModule(mappedTypesAssemblyName.Name);
        }

        public TypeMapper()
        {
            CreateMap("Create")
                .IgnoreName("Id")
                .IgnoreClasses()
                .IgnoreInterfaces()
                .KeepStrings();

            CreateMap("Update")
                .IgnoreClasses()
                .IgnoreInterfaces()
                .KeepStrings();

            CreateMap("Dto")
                .FlatClasses()
                .FlatCollections()
                .IgnoreNamePattern(@".+Id$");

            CreateMap("Flat")
                .IgnoreClasses()
                .IgnoreInterfaces()
                .KeepStrings()
                .IgnoreNamePattern(@".+Id$");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace UserOrdersApiDemo.Application
{
    public static class TypeBuilderExtensions
    {

        public static void CreateProperty(this TypeBuilder typeBuilder, Type propertyType, string propertyName, IEnumerable<CustomAttributeData> attributes = null)
        {
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType, null);

            FieldBuilder fieldBuilder = typeBuilder.DefineField(propertyName.ToLower(), propertyType, FieldAttributes.Private);

            if (attributes is not null)
            {
                foreach (var customAttribute in attributes)
                {
                    var attrCtorInfo = customAttribute.Constructor;
                    var arguments = customAttribute.ConstructorArguments.Select(argument => (object)argument.Value).ToArray();
                    var attrBuilder = new CustomAttributeBuilder(attrCtorInfo, arguments);
                    propertyBuilder.SetCustomAttribute(attrBuilder);
                }
            }

            // Further part is copy-pasted from stack-overflow

            MethodBuilder getterBuilder = typeBuilder.DefineMethod($"get_{propertyName}",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                propertyType,
                Type.EmptyTypes);

            ILGenerator getterIL = getterBuilder.GetILGenerator();
            getterIL.Emit(OpCodes.Ldarg_0);
            getterIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getterIL.Emit(OpCodes.Ret);

            MethodBuilder setterBuilder = typeBuilder.DefineMethod($"set_{propertyName}",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null,
                new Type[] { propertyType });

            ILGenerator setterIL = setterBuilder.GetILGenerator();
            setterIL.Emit(OpCodes.Ldarg_0);
            setterIL.Emit(OpCodes.Ldarg_1);
            setterIL.Emit(OpCodes.Stfld, fieldBuilder);
            setterIL.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getterBuilder);
            propertyBuilder.SetSetMethod(setterBuilder);
        }
    }
}
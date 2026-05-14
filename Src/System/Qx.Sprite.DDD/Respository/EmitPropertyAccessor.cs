// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.DDD.Respository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// Provides methods for generating efficient property accessors for public instance properties at runtime using
    /// dynamic code emission.
    /// </summary>
    /// <remarks>This class is intended for advanced scenarios where high-performance, reflection-based
    /// property access is required. The generated accessors allow reading and writing property values on objects
    /// without the overhead of standard reflection. All methods are static and thread-safe.</remarks>
    public static class EmitPropertyAccessor
    {
        /// <summary>
        /// Creates a dictionary of property accessors for all public instance properties of the specified type that
        /// support both reading and writing.
        /// </summary>
        /// <remarks>Each entry in the returned dictionary provides getter and setter delegates for the
        /// associated property. Properties that are read-only or write-only are excluded. The method does not include
        /// inherited properties that are not public or not instance members.</remarks>
        /// <param name="type">The type whose public instance properties will be inspected to generate accessors. Must not be null.</param>
        /// <param name="predicate"></param>
        /// <returns>A dictionary mapping property names to their corresponding <see cref="PropertyAccessor"/> instances. Only
        /// properties that can be both read and written are included.</returns>
        public static Dictionary<string, PropertyAccessor> CreateAccessors(Type type, Func<PropertyInfo, bool>? predicate = null)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite);
            if (predicate != null) properties = properties.Where(predicate);

            var accessors = new Dictionary<string, PropertyAccessor>();

            foreach (var prop in properties)
            {
                accessors[prop.Name] = new PropertyAccessor(CreateGetter(prop), CreateSetter(prop));
            }

            return accessors;
        }

        /// <summary>
        /// Creates a delegate that retrieves the value of the specified property from a given object instance.
        /// </summary>
        /// <remarks>The returned delegate performs type casting and boxing as necessary to access the
        /// property value. The property must be readable and compatible with the provided object instance; otherwise,
        /// runtime exceptions may occur.</remarks>
        /// <param name="property">The property metadata used to generate the getter delegate. Must represent a readable property.</param>
        /// <returns>A delegate that takes an object instance and returns the value of the specified property as an object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:Parameters should be on same line or separate lines", Justification = "<挂起>")]
        private static Func<object, object> CreateGetter(PropertyInfo property)
        {
            var method = new DynamicMethod(
                $"Get_{property.Name}",
                typeof(object), [typeof(object)], true);

            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, property.DeclaringType!);
            il.EmitCall(OpCodes.Callvirt, property.GetMethod!, null);

            if (property.PropertyType.IsValueType)
                il.Emit(OpCodes.Box, property.PropertyType);

            il.Emit(OpCodes.Ret);

            return (Func<object, object>)method.CreateDelegate(typeof(Func<object, object>));
        }

        /// <summary>
        /// Creates a delegate that sets the value of the specified property on a target object.
        /// </summary>
        /// <remarks>The returned delegate accepts two parameters: the target object and the value to
        /// assign to the property. The property must have a public set accessor. The caller is responsible for ensuring
        /// that the value is compatible with the property's type; otherwise, an exception may be thrown at
        /// runtime.</remarks>
        /// <param name="property">The property metadata used to generate the setter delegate. Must represent a writable property.</param>
        /// <returns>An Action delegate that sets the value of the specified property on a given object instance.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:Parameters should be on same line or separate lines", Justification = "<挂起>")]
        private static Action<object, object> CreateSetter(PropertyInfo property)
        {
            var method = new DynamicMethod(
                $"Set_{property.Name}",
                null, [typeof(object), typeof(object)], true);

            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, property.DeclaringType!);
            il.Emit(OpCodes.Ldarg_1);

            if (property.PropertyType.IsValueType)
                il.Emit(OpCodes.Unbox_Any, property.PropertyType);
            else
                il.Emit(OpCodes.Castclass, property.PropertyType);

            il.EmitCall(OpCodes.Callvirt, property.SetMethod!, null);
            il.Emit(OpCodes.Ret);

            return (Action<object, object>)method.CreateDelegate(typeof(Action<object, object>));
        }
    }

    /// <summary>
    ///  1
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="PropertyAccessor"/> class.
    /// </remarks>
    /// <param name="getter"></param>
    /// <param name="setter"></param>
    public class PropertyAccessor(Func<object, object> getter, Action<object, object> setter)
    {
        /// <summary>
        ///  Gets sd
        /// </summary>
        public Func<object, object> Getter { get; } = getter;

        /// <summary>
        ///  Gets sd
        /// </summary>
        public Action<object, object> Setter { get; } = setter;
    }
}

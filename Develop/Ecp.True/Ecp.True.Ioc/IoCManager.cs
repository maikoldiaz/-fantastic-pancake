// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoCManager.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using Castle.DynamicProxy;
    using Ecp.True.Core.Attributes;

    /// <summary>
    /// Loads IoC container for the current region.
    /// </summary>
    public static class IoCManager
    {
        /// <summary>
        /// The ECP TRUE namespace.
        /// </summary>
        private const string EcpTrueNamespacePrefix = "Ecp.";

        /// <summary>
        /// The ECP TRUE assembly name prefix.
        /// </summary>
        private const string EcpTrueAssemblyNamePrefix = "Ecp";

        /// <summary>
        /// Registers the by convention.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns>The container.</returns>
        public static IContainer RegisterByConvention(ContainerBuilder containerBuilder)
        {
            return RegisterByConvention(containerBuilder, null, null, null);
        }

        /// <summary>
        /// Register the dependencies to dependency container.
        /// </summary>
        /// <returns>
        /// The instance of <see cref="IContainer" />.
        /// </returns>
        public static IContainer RegisterByConvention()
        {
            return RegisterByConvention((IEnumerable<Assembly>)null);
        }

        /// <summary>
        /// Register the dependencies to dependency container.
        /// </summary>
        /// <param name="assembliesToLoad">
        /// Assemblies used for registration.
        /// </param>
        /// <returns>
        /// The instance of <see cref="IContainer" />.
        /// </returns>
        public static IContainer RegisterByConvention(IEnumerable<Assembly> assembliesToLoad)
        {
            return RegisterByConvention(assembliesToLoad, (IDictionary<Type, Type>)null);
        }

        /// <summary>
        /// Register the dependencies to dependency container.
        /// </summary>
        /// <param name="explicitTypes">
        /// Any explicit types to override default registrations.
        /// </param>
        /// <returns>
        /// The instance of <see cref="IContainer" />.
        /// </returns>
        public static IContainer RegisterByConvention(IDictionary<Type, Type> explicitTypes)
        {
            return RegisterByConvention(explicitTypes, null);
        }

        /// <summary>
        /// Register the dependencies to dependency container.
        /// </summary>
        /// <param name="explicitInstances">
        /// Any explicit instances to override default registrations.
        /// </param>
        /// <returns>
        /// The instance of <see cref="IContainer" />.
        /// </returns>
        public static IContainer RegisterByConvention(IDictionary<Type, object> explicitInstances)
        {
            return RegisterByConvention((IEnumerable<Assembly>)null, explicitInstances);
        }

        /// <summary>
        /// Register the dependencies to dependency container.
        /// </summary>
        /// <param name="assembliesToLoad">
        /// Assemblies used for registration.
        /// </param>
        /// <param name="explicitTypes">
        /// Any explicit types to override default registrations.
        /// </param>
        /// <returns>
        /// The instance of <see cref="IContainer" />.
        /// </returns>
        public static IContainer RegisterByConvention(IEnumerable<Assembly> assembliesToLoad, IDictionary<Type, Type> explicitTypes)
        {
            return RegisterByConvention(assembliesToLoad, explicitTypes, null);
        }

        /// <summary>
        /// Register the dependencies to dependency container.
        /// </summary>
        /// <param name="explicitTypes">
        /// Any explicit types to override default registrations.
        /// </param>
        /// <param name="explicitInstances">
        /// Any explicit instances to override default registrations.
        /// </param>
        /// <returns>
        /// The instance of <see cref="IContainer" />.
        /// </returns>
        public static IContainer RegisterByConvention(IDictionary<Type, Type> explicitTypes, IDictionary<Type, object> explicitInstances)
        {
            return RegisterByConvention(new ContainerBuilder(), null, explicitTypes, explicitInstances);
        }

        /// <summary>
        /// Register the dependencies to dependency container.
        /// </summary>
        /// <param name="assembliesToLoad">
        /// Assemblies used for registration.
        /// </param>
        /// <param name="explicitInstances">
        /// Any explicit instances to override default registrations.
        /// </param>
        /// <returns>
        /// The instance of <see cref="IContainer" />.
        /// </returns>
        public static IContainer RegisterByConvention(IEnumerable<Assembly> assembliesToLoad, IDictionary<Type, object> explicitInstances)
        {
            return RegisterByConvention(assembliesToLoad, null, explicitInstances);
        }

        /// <summary>
        /// Register the dependencies to dependency container.
        /// </summary>
        /// <param name="assembliesToLoad">
        /// Assemblies used for registration.
        /// </param>
        /// <param name="explicitTypes">
        /// Any explicit types to override default registrations.
        /// </param>
        /// <param name="explicitInstances">
        /// Any explicit instances to override default registrations.
        /// </param>
        /// <returns>
        /// The instance of <see cref="IContainer" />.
        /// </returns>
        public static IContainer RegisterByConvention(IEnumerable<Assembly> assembliesToLoad, IDictionary<Type, Type> explicitTypes, IDictionary<Type, object> explicitInstances)
        {
            return RegisterByConvention(new ContainerBuilder(), assembliesToLoad, explicitTypes, explicitInstances);
        }

        /// <summary>
        /// Registers the by convention.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="assembliesToLoad">The assemblies to load.</param>
        /// <param name="explicitTypes">The explicit types.</param>
        /// <param name="explicitInstances">The explicit instances.</param>
        /// <returns>The container.</returns>
        public static IContainer RegisterByConvention(
            ContainerBuilder containerBuilder, IEnumerable<Assembly> assembliesToLoad, IDictionary<Type, Type> explicitTypes, IDictionary<Type, object> explicitInstances)
        {
            IContainer container = new AutofacContainer(containerBuilder);

            // Add entry assembly for exe applications, this is required for Service Fabric projects
            var assembliesToBeRegistered = GetAssembliesToBeRegistered(assembliesToLoad).ToList();
            var callingAssembly = Assembly.GetEntryAssembly();
            if (callingAssembly != null && !assembliesToBeRegistered.Contains(callingAssembly))
            {
                assembliesToBeRegistered.Add(callingAssembly);
            }

            RegisterDependencies(container, assembliesToBeRegistered);

            // Override with explicit types for registration
            if (explicitTypes != null && explicitTypes.Any())
            {
                foreach (var registration in explicitTypes)
                {
                    container.Register(registration.Key, registration.Value, new IoCRegistrationAttribute(IoCLifetime.Transient));
                }
            }

            // Override with explicit instances for registration
            if (explicitInstances != null && explicitInstances.Any())
            {
                foreach (var registration in explicitInstances)
                {
                    container.RegisterInstance(registration.Key, registration.Value);
                }
            }

            containerBuilder.RegisterType<CallInterceptor>().As<IAsyncInterceptor>().InstancePerLifetimeScope().PreserveExistingDefaults();
            containerBuilder.RegisterType<AsyncDeterminationInterceptor>().InstancePerLifetimeScope().PreserveExistingDefaults();
            return container;
        }

        /// <summary>
        /// Adds to internal map.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="interfaces">The interfaces.</param>
        /// <param name="interfaceTypeMapping">Interface Type Mapping.</param>
        private static void AddToInterfaceTypeMap(Type type, IEnumerable<Type> interfaces, IDictionary<Type, HashSet<Type>> interfaceTypeMapping)
        {
            foreach (var interfaceOnType in interfaces)
            {
                if (!interfaceTypeMapping.ContainsKey(interfaceOnType))
                {
                    interfaceTypeMapping[interfaceOnType] = new HashSet<Type>();
                }

                interfaceTypeMapping[interfaceOnType].Add(type);
            }
        }

        /// <summary>
        /// Gets the assemblies in base path.
        /// </summary>
        /// <returns>Gets all assemblies in the base path.</returns>
        private static IEnumerable<Assembly> GetAssembliesInBasePath()
        {
            var basePath = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var basePathAssemblies = new List<Assembly>();

            var files = Directory.GetFiles(basePath, $"{EcpTrueAssemblyNamePrefix}*.dll").ToList();
            files.AddRange(Directory.GetFiles(basePath, $"{EcpTrueAssemblyNamePrefix}*.exe").ToList());
            foreach (var dll in files)
            {
                try
                {
                    var assemblyName = AssemblyName.GetAssemblyName(dll);
                    basePathAssemblies.Add(Assembly.Load(assemblyName.FullName));
                }
                catch (Exception ex)
                {
                    // Ignore the error
                    Debug.WriteLine(ex);
                }
            }

            return basePathAssemblies;
        }

        /// <summary>
        /// The get assemblies to be registered for current region.
        /// </summary>
        /// <param name="assembliesInPath">
        /// The assemblies in path.
        /// </param>
        /// <returns>
        /// The enumeration of assemblies/&gt;.
        /// </returns>
        private static IEnumerable<Assembly> GetAssembliesToBeRegistered(IEnumerable<Assembly> assembliesInPath)
        {
            var basePathAssemblies = assembliesInPath ?? GetAssembliesInBasePath();
            return basePathAssemblies;
        }

        /// <summary>
        /// Gets the classes from assemblies in base path.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>
        /// Gets all the types to register.
        /// </returns>
        private static IEnumerable<Type> GetClassesFromAssembliesInBasePath(IEnumerable<Assembly> assemblies = null)
        {
            var allClasses = assemblies != null ? AllClasses.FromAssemblies(assemblies) : AllClasses.FromAssembliesInBasePath();
            return allClasses
                    .Where(n => n.Namespace != null
                                && n.Namespace.StartsWith(EcpTrueNamespacePrefix, StringComparison.OrdinalIgnoreCase)
                                && !n.IsAbstract
                                && !IoCHelper.ShouldIgnoreType(n));
        }

        /// <summary>
        /// Gets the interfaces to be registered.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Types to be registered.</returns>
        private static IEnumerable<Type> GetInterfacesToBeRegistered(Type type)
        {
            // NOTE: For Generic interfaces the convention is that the class name should be the interface name without an I
            // e.g. IDataStoreStrategy<TEntity> => DataStoreStrategy<TEntity>
            var allInterfacesOnType =
                    type.GetInterfaces()
                            .Where(
                                i =>
                                    i.IsGenericType || type.Name.Contains(i.Name.Substring(1, i.Name.Length - 1), StringComparison.OrdinalIgnoreCase)
                                    || (type.BaseType != null && type.BaseType.IsAbstract && i.Namespace != null && i.Namespace.StartsWith(EcpTrueNamespacePrefix, StringComparison.OrdinalIgnoreCase)))
                            .Select(i => i.IsGenericType ? i.GetGenericTypeDefinition() : i)
                            .ToList();

            var directTypes = allInterfacesOnType.Except(allInterfacesOnType.SelectMany(i => i.GetInterfaces())).ToList();

            if (type.BaseType == null || !type.BaseType.IsAbstract)
            {
                return directTypes;
            }

            // Add all interfaces of base type as well.
            directTypes.AddRange(type.BaseType.GetInterfaces());

            return directTypes;
        }

        /// <summary>
        /// Gets the name for registration.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Type name for named registration.</returns>
        private static string GetNameForRegistration(MemberInfo type)
        {
            var name = type.Name;
            var iocAttribute = (IoCRegistrationAttribute)Attribute.GetCustomAttribute(type, typeof(IoCRegistrationAttribute));
            if (iocAttribute != null)
            {
                name = iocAttribute.ShouldAppendClassName ? iocAttribute.NamePrefix + name : iocAttribute.NamePrefix;
            }

            return name.ToUpperInvariant();
        }

        /// <summary>
        /// Gets the lifetime manager.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// Lifetime manager for unity.
        /// </returns>
        private static IoCRegistrationAttribute GetRegistrationInfo(MemberInfo type)
        {
            var iocAttribute = (IoCRegistrationAttribute)Attribute.GetCustomAttribute(type, typeof(IoCRegistrationAttribute));
            return iocAttribute ?? new IoCRegistrationAttribute(IoCLifetime.Transient);
        }

        /// <summary>
        /// Registers the by convention.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="assembliesToLoad">The assemblies to load. By default it will load all assemblies in base path.</param>
        private static void RegisterDependencies(IContainer container, IEnumerable<Assembly> assembliesToLoad)
        {
            var interfaceTypeMapping = new Dictionary<Type, HashSet<Type>>();
            var classes = GetClassesFromAssembliesInBasePath(assembliesToLoad);
            foreach (var type in classes)
            {
                var interfacesToBeRegistered = GetInterfacesToBeRegistered(type);
                AddToInterfaceTypeMap(type, interfacesToBeRegistered, interfaceTypeMapping);
            }

            RegisterDependencies(container, interfaceTypeMapping);
        }

        /// <summary>
        /// Register dependencies.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="interfaceTypeMapping">Interface Type Mapping.</param>
        private static void RegisterDependencies(IContainer container, Dictionary<Type, HashSet<Type>> interfaceTypeMapping)
        {
            foreach (var typeMapping in interfaceTypeMapping)
            {
                if (typeMapping.Value.Count == 1)
                {
                    var type = typeMapping.Value.First();
                    container.Register(typeMapping.Key, type, GetRegistrationInfo(type));
                }
                else
                {
                    foreach (var type in typeMapping.Value)
                    {
                        container.Register(
                            GetNameForRegistration(type),
                            typeMapping.Key,
                            type,
                            GetRegistrationInfo(type));
                    }
                }
            }
        }
    }
}
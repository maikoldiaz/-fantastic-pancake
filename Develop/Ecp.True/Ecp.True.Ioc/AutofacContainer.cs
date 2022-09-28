// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutofacContainer.cs" company="Microsoft">
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
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using Autofac.Builder;
    using Autofac.Core;
    using Autofac.Extras.DynamicProxy;
    using Castle.DynamicProxy;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Ioc.Entities;

    /// <summary>
    /// The AUTOFAC dependency register.
    /// </summary>
    public sealed class AutofacContainer : IContainer
    {
        /// <summary>
        /// The container builder.
        /// </summary>
        private readonly ContainerBuilder containerBuilder;

        /// <summary>
        /// The is dirty.
        /// </summary>
        private bool isDirty;

        /// <summary>
        /// The scope.
        /// </summary>
        private ILifetimeScope scope;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacContainer" /> class.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        public AutofacContainer(ContainerBuilder containerBuilder)
        {
            this.containerBuilder = containerBuilder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacContainer" /> class.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="scope">The scope.</param>
        private AutofacContainer(ContainerBuilder containerBuilder, ILifetimeScope scope)
            : this(containerBuilder)
        {
            this.scope = scope;
        }

        /// <summary>
        /// Gets the registrations.
        /// </summary>
        /// <value>
        /// The registrations.
        /// </value>
        public IEnumerable<RegistrationInfo> Registrations
        {
            get
            {
                if (this.scope == null)
                {
                    return Enumerable.Empty<RegistrationInfo>();
                }

                var registrations = new List<RegistrationInfo>();

                foreach (var componentRegistryRegistration in this.scope.ComponentRegistry.Registrations)
                {
                    foreach (var service in componentRegistryRegistration.Services)
                    {
                        var serviceKey = service.Description;
                        var serviceName = string.Empty;
                        switch (service)
                        {
                            case TypedService typedService:
                                serviceKey = string.Empty;
                                serviceName = typedService.ServiceType.Name;
                                break;
                            case KeyedService keyedService:
                                serviceKey = keyedService.ServiceKey.ToString();
                                serviceName = keyedService.ServiceType.Name;
                                break;
                            default:

                                // Do nothing
                                break;
                        }

                        registrations.Add(
                            new RegistrationInfo(
                                serviceName,
                                componentRegistryRegistration.Activator.LimitType.Name,
                                serviceKey,
                                $"{componentRegistryRegistration.Sharing.ToString()}_{componentRegistryRegistration.Ownership.ToString()}"));
                    }
                }

                return registrations;
            }
        }

        /// <summary>
        /// Gets the inner container.
        /// </summary>
        /// <value>
        /// The inner container.
        /// </value>
        public object InnerContainer
        {
            get
            {
                this.TryBuildScope();
                return this.scope;
            }
        }

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="component">The component.</param>
        /// <param name="iocRegistrationAttribute">The IOC registration attribute.</param>
        public void Register(Type service, Type component, IoCRegistrationAttribute iocRegistrationAttribute)
        {
            ArgumentValidators.ThrowIfNull(service, nameof(service));
            ArgumentValidators.ThrowIfNull(component, nameof(component));
            ArgumentValidators.ThrowIfNull(iocRegistrationAttribute, nameof(iocRegistrationAttribute));

            if (component.IsGenericType)
            {
                HandleRegistration(this.containerBuilder.RegisterGeneric(component).As(service), iocRegistrationAttribute);
            }
            else if (service.IsGenericType && service.ContainsGenericParameters)
            {
                HandleRegistration(
                    this.containerBuilder
                            .RegisterAssemblyTypes(Assembly.GetAssembly(component))
                            .Where(t => t == component)
                            .AsClosedTypesOf(service),
                    iocRegistrationAttribute);
            }
            else if (service.IsGenericType && !service.ContainsGenericParameters)
            {
                HandleRegistration(
                    this.containerBuilder
                            .RegisterAssemblyTypes(Assembly.GetAssembly(component))
                            .Where(t => t == component),
                    iocRegistrationAttribute);
            }
            else
            {
                HandleRegistration(this.containerBuilder.RegisterType(component).As(service), iocRegistrationAttribute);
            }

            this.isDirty = true;
        }

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="service">The service.</param>
        /// <param name="component">The component.</param>
        /// <param name="iocRegistrationAttribute">The IOC registration attribute.</param>
        public void Register(string name, Type service, Type component, IoCRegistrationAttribute iocRegistrationAttribute)
        {
            ArgumentValidators.ThrowIfNull(service, nameof(service));
            ArgumentValidators.ThrowIfNull(component, nameof(component));
            ArgumentValidators.ThrowIfNull(iocRegistrationAttribute, nameof(iocRegistrationAttribute));

            if (component.IsGenericType)
            {
                HandleRegistration(this.containerBuilder.RegisterGeneric(component).As(service), iocRegistrationAttribute);
                HandleRegistration(this.containerBuilder.RegisterGeneric(component).Named(name, service), iocRegistrationAttribute);
            }
            else if (service.IsGenericType && service.ContainsGenericParameters)
            {
                HandleRegistration(
                    this.containerBuilder
                            .RegisterAssemblyTypes(Assembly.GetAssembly(component))
                            .Where(t => t == component)
                            .AsClosedTypesOf(service, name),
                    iocRegistrationAttribute);
            }
            else if (service.IsGenericType && !service.ContainsGenericParameters)
            {
                HandleRegistration(
                    this.containerBuilder
                            .RegisterAssemblyTypes(Assembly.GetAssembly(component))
                            .Where(t => t == component),
                    iocRegistrationAttribute);
            }
            else
            {
                HandleRegistration(this.containerBuilder.RegisterType(component).As(service), iocRegistrationAttribute);
                HandleRegistration(this.containerBuilder.RegisterType(component).Named(name, service), iocRegistrationAttribute);
            }

            this.isDirty = true;
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="instance">The instance.</param>
        public void RegisterInstance(Type service, object instance)
        {
            this.containerBuilder.RegisterInstance(instance).As(service);
            this.isDirty = true;
        }

        /// <summary>
        /// Creates the child container.
        /// </summary>
        /// <returns>
        /// Child Container.
        /// </returns>
        public IContainer BuildChildContainer()
        {
            this.TryBuildScope();
            return new AutofacContainer(this.containerBuilder, this.scope.BeginLifetimeScope());
        }

        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The type instance.
        /// </returns>
        public T Resolve<T>()
        {
            this.TryBuildScope();
            return this.scope.Resolve<T>();
        }

        /// <summary>
        /// Resolves the specified name.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>The service.</returns>
        public T Resolve<T>(string name, object value)
        {
            this.TryBuildScope();
            return this.scope.Resolve<T>(new NamedParameter(name, value));
        }

        /// <summary>
        /// Resolves the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// The object.
        /// </returns>
        public object Resolve(Type type)
        {
            this.TryBuildScope();
            return this.scope.Resolve(type);
        }

        /// <summary>
        /// Resolves the specified name.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>
        /// The type instance.
        /// </returns>
        public T Resolve<T>(string name)
        {
            this.TryBuildScope();
            return this.scope.ResolveNamed<T>(name);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.scope?.Dispose();
        }

        /// <summary>
        /// Register the dependencies.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="iocRegistrationAttribute">The registration attribute info.</param>
        private static void HandleRegistration(
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder,
            IoCRegistrationAttribute iocRegistrationAttribute)
        {
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration;
            switch (iocRegistrationAttribute.Lifetime)
            {
                case IoCLifetime.ContainerControlled:
                    registration = builder.SingleInstance();
                    break;
                case IoCLifetime.Hierarchical:
                    registration = builder.InstancePerLifetimeScope();
                    break;
                default:
                    registration = builder.InstancePerDependency();
                    break;
            }

            if (iocRegistrationAttribute.ShouldInterceptMethods)
            {
                registration?.EnableInterfaceInterceptors().InterceptedBy(typeof(AsyncDeterminationInterceptor));
            }
        }

        /// <summary>
        /// Register the dependencies.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="iocRegistrationAttribute">The registration attribute info.</param>
        private static void HandleRegistration(
            IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> builder,
            IoCRegistrationAttribute iocRegistrationAttribute)
        {
            IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> registration;
            switch (iocRegistrationAttribute.Lifetime)
            {
                case IoCLifetime.ContainerControlled:
                    registration = builder.SingleInstance();
                    break;
                case IoCLifetime.Hierarchical:
                    registration = builder.InstancePerLifetimeScope();
                    break;
                default:
                    registration = builder.InstancePerDependency();
                    break;
            }

            if (iocRegistrationAttribute.ShouldInterceptMethods)
            {
                registration?.EnableInterfaceInterceptors().InterceptedBy(typeof(AsyncDeterminationInterceptor));
            }
        }

        /// <summary>
        /// Tries the build scope.
        /// </summary>
        private void TryBuildScope()
        {
            if (!this.isDirty && this.scope != null)
            {
                return;
            }

            this.scope = this.containerBuilder.Build();
            this.isDirty = false;

            Debug.WriteLine(IoCHelper.GetRegistrationMappings(this));
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BootstrapperBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Core.Bootstrap
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The base bootstrapper service.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BootstrapperBase
    {
        /// <summary>
        /// The services.
        /// </summary>
        private readonly IServiceCollection services;

        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapperBase"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        protected BootstrapperBase(IServiceCollection services)
        {
            this.services = services;
        }

        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        protected void RegisterTransient<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface
        {
            this.services.AddTransient<TInterface, TImplementation>();
        }

        /// <summary>
        /// Registers the transient.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="implementation">The implementation.</param>
        protected void RegisterTransient(Type service, Type implementation)
        {
            this.services.AddTransient(service, implementation);
        }

        /// <summary>
        /// Registers the singleton.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        protected void RegisterSingleton<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface
        {
            this.services.AddSingleton<TInterface, TImplementation>();
        }

        /// <summary>
        /// Registers the singleton.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="implementation">The implementation.</param>
        protected void RegisterSingleton(Type service, Type implementation)
        {
            this.services.AddSingleton(service, implementation);
        }

        /// <summary>
        /// Registers the scoped.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        protected void RegisterScoped<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface
        {
            this.services.AddScoped<TInterface, TImplementation>();
        }

        /// <summary>
        /// Registers the scoped.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="implementation">The implementation.</param>
        protected void RegisterScoped(Type service, Type implementation)
        {
            this.services.AddScoped(service, implementation);
        }
    }
}

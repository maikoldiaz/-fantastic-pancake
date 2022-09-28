// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContainer.cs" company="Microsoft">
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
    using Ecp.True.Core.Attributes;

    using Ecp.True.Ioc.Entities;

    /// <summary>
    /// The dependency register.
    /// </summary>
    public interface IContainer : IDisposable
    {
        /// <summary>
        /// Gets the inner container.
        /// </summary>
        /// <value>
        /// The inner container.
        /// </value>
        object InnerContainer { get; }

        /// <summary>
        /// Gets the registrations.
        /// </summary>
        /// <value>
        /// The registrations.
        /// </value>
        IEnumerable<RegistrationInfo> Registrations { get; }

        /// <summary>
        /// Creates the child container.
        /// </summary>
        /// <returns>Child Container.</returns>
        IContainer BuildChildContainer();

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="component">The component.</param>
        /// <param name="iocRegistrationAttribute">The IOC registration attribute.</param>
        void Register(Type service, Type component, IoCRegistrationAttribute iocRegistrationAttribute);

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="service">The service.</param>
        /// <param name="component">The component.</param>
        /// <param name="iocRegistrationAttribute">The IOC registration attribute.</param>
        void Register(string name, Type service, Type component, IoCRegistrationAttribute iocRegistrationAttribute);

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="instance">The instance.</param>
        void RegisterInstance(Type service, object instance);

        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The type instance.
        /// </returns>
        T Resolve<T>();

        /// <summary>
        /// Resolves the specified name.
        /// </summary>
        /// <typeparam name="T">T parameter.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>The Task.</returns>
        T Resolve<T>(string name, object value);

        /// <summary>
        /// Resolves the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The object.</returns>
        object Resolve(Type type);

        /// <summary>
        /// Resolves the specified name.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>The type instance.</returns>
        T Resolve<T>(string name);
    }
}
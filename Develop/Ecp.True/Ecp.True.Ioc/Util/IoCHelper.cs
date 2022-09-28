// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoCHelper.cs" company="Microsoft">
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
    using System.CodeDom.Compiler;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Ecp.True.Core;

    /// <summary>
    /// Contains helper methods for IoC registrations and debugging.
    /// </summary>
    public static class IoCHelper
    {
        /// <summary>
        /// The types or namespaces to ignore.
        /// </summary>
        private static readonly string[] TypesOrNamespacesToIgnore =
        {
            "Ecp.True.Entities.Core",
        };

        /// <summary>
        /// The types or namespaces with generated code attribute to ignore.
        /// </summary>
        private static readonly string[] TypesOrNamespacesWithGeneratedCodeAttributeToIgnore =
        {
            string.Empty,
        };

        /// <summary>
        /// Prints the registration mappings.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>All the registration mappings.</returns>
        public static string GetRegistrationMappings(IContainer container)
        {
            ArgumentValidators.ThrowIfNull(container, nameof(container));

            var registrations = new StringBuilder(string.Empty);
            registrations.Append("RegisteredType : MappedType : Named Registration : Lifetime");
            foreach (var containerRegistration in container.Registrations)
            {
                registrations.AppendLine();

                registrations.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "{0} : {1} : {2} : {3}",
                    containerRegistration.Service,
                    containerRegistration.Component,
                    containerRegistration.Name,
                    containerRegistration.Lifetime);
            }

            return registrations.ToString();
        }

        /// <summary>
        /// Should ignore this type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>true or false.</returns>
        public static bool ShouldIgnoreType(Type type)
        {
            if (type == null || string.IsNullOrWhiteSpace(type.Namespace))
            {
                return true;
            }

            return TypesOrNamespacesToIgnore.Any(n1 => type.FullName.Contains(n1, StringComparison.OrdinalIgnoreCase)) ||
                   TypesOrNamespacesWithGeneratedCodeAttributeToIgnore.Any(n2 =>
                       type.FullName.Contains(n2, StringComparison.OrdinalIgnoreCase) &&
                       type.CustomAttributes.Any(a => a.AttributeType == typeof(GeneratedCodeAttribute)));
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IgnorableSerializerContractResolver.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Core
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// This is to ignore properties while serializing.
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.Serialization.DefaultContractResolver" />
    public class IgnorableSerializerContractResolver : DefaultContractResolver
    {
        private readonly Dictionary<Type, HashSet<string>> ignores;

        /// <summary>
        /// Initializes a new instance of the <see cref="IgnorableSerializerContractResolver"/> class.
        /// </summary>
        public IgnorableSerializerContractResolver()
        {
            this.ignores = new Dictionary<Type, HashSet<string>>();
        }

        /// <summary>
        /// Explicitly ignore the given property(s) for the given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyNames">one or more properties to ignore.  Leave empty to ignore the type entirely.</param>
        public void Ignore(Type type, params string[] propertyNames)
        {
            ArgumentValidators.ThrowIfNull(propertyNames, nameof(propertyNames));

            if (!this.ignores.ContainsKey(type))
            {
                this.ignores[type] = new HashSet<string>();
            }

            propertyNames.ForEach(x => this.ignores[type].Add(x));
        }

        /// <summary>
        /// The decision logic goes here.
        /// </summary>
        /// <param name="member">member.</param>
        /// <param name="memberSerialization">member serialization.</param>
        /// <returns>The property.</returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (this.IsIgnored(property.DeclaringType, property.PropertyName)
                || this.IsIgnored(property.DeclaringType.BaseType, property.PropertyName))
            {
                property.ShouldSerialize = instance => { return false; };
            }

            return property;
        }

        /// <summary>
        /// Is the given property for the given type ignored.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">the property name.</param>
        /// <returns>Bool indicating if its ignored.</returns>
        private bool IsIgnored(Type type, string propertyName)
        {
            return this.ignores.ContainsKey(type) &&
                (this.ignores[type].Count == 0 || this.ignores[type].Contains(propertyName));
        }
    }
}

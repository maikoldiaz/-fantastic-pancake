// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FullPropertyNameContractResolver.cs" company="Microsoft">
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
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// The Resolver to serialize based on property names.
    /// </summary>
    public class FullPropertyNameContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// Creates properties for the given entity.
        /// </summary>
        /// <param name="type">The type to create properties for.</param>
        /// <param name="memberSerialization">The member serialization mode for the type.</param>
        /// <returns>
        /// Properties for serialization.
        /// </returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var propertiesList = base.CreateProperties(type, memberSerialization);

            propertiesList?.ForEach(x => x.PropertyName = x.UnderlyingName);

            return propertiesList;
        }
    }
}

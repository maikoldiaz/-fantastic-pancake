// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ODataService.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core.OData
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Ecp.True.Core;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Query;
    using Microsoft.AspNetCore.Http;
    using Microsoft.OData.Edm;
    using Microsoft.OData.UriParser;

    /// <summary>
    /// The OData Service class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ODataService : IODataService
    {
        /// <summary>
        /// Gets the o data query options.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TParent">The type of the parent.</typeparam>
        /// <typeparam name="TChild">The type of the child.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="model">The model.</param>
        /// <param name="keyValue">The key value.</param>
        /// <returns>OData query options.</returns>
        public ODataQueryOptions<TResult> GetODataQueryOptions<TResult, TParent, TChild>(HttpRequest request, IEdmModel model, int keyValue)
        {
            ArgumentValidators.ThrowIfNull(model, nameof(model));
            return BuildOptions<TResult, TChild>(typeof(TParent).Name, typeof(TChild).Name, request, model, keyValue);
        }

        /// <summary>
        /// Gets the o data query options.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TChild">The type of the child.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="model">The model.</param>
        /// <param name="keyValue">The key value.</param>
        /// <param name="parentName">Name of the parent.</param>
        /// <returns>
        /// OData query options.
        /// </returns>
        public ODataQueryOptions<TResult> GetODataQueryOptions<TResult, TChild>(HttpRequest request, IEdmModel model, int keyValue, string parentName)
        {
            ArgumentValidators.ThrowIfNull(model, nameof(model));
            return BuildOptions<TResult, TChild>(parentName, typeof(TChild).Name, request, model, keyValue);
        }

        private static ODataQueryOptions<TResult> BuildOptions<TResult, TChild>(string parent, string child, HttpRequest request, IEdmModel model, int keyValue)
        {
            var entitySet = model.EntityContainer.FindEntitySet(parent);
            var entitySetSegment = new EntitySetSegment(entitySet);

            var childCollectionName = $"{child}s";
            var navigationProperty = entitySet.NavigationPropertyBindings.FirstOrDefault(b => b.NavigationProperty.Name.Equals(childCollectionName, StringComparison.OrdinalIgnoreCase));
            var navigationSegment = new NavigationPropertySegment(navigationProperty.NavigationProperty, navigationProperty.Target);

            var dict = new Dictionary<string, object>();
            dict.Add($"{parent}Id", keyValue);

            var keySegment = new KeySegment(dict, entitySet.EntityType(), entitySet);

            var path = new Microsoft.AspNet.OData.Routing.ODataPath(entitySetSegment, keySegment, navigationSegment);

            var context = new ODataQueryContext(model, typeof(TChild), path);
            return new ODataQueryOptions<TResult>(context, request);
        }
    }
}

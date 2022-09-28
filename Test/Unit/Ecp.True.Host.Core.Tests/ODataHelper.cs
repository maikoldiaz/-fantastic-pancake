// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ODataHelper.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core.Tests.OData
{
    using System;
    using Ecp.True.Core;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Builder;
    using Microsoft.AspNet.OData.Extensions;
    using Microsoft.AspNet.OData.Query;
    using Microsoft.AspNet.OData.Query.Validators;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OData.UriParser;
    using Moq;

    /// <summary>
    /// The odata helper class to build odata query options.
    /// </summary>
    public static class ODataHelper
    {
        /// <summary>
        /// Sets up query options.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="uri">The URI.</param>
        /// <returns>The odata query options.</returns>
        public static ODataQueryOptions<TEntity> SetUpQueryOptions<TEntity>(Uri uri)
            where TEntity : class
        {
            ArgumentValidators.ThrowIfNull(uri, nameof(uri));
            var collection = new ServiceCollection();

            collection.AddOData();
            collection.AddODataQueryFilter();
            collection.AddTransient<ODataUriResolver>();
            collection.AddTransient<ODataQueryValidator>();
            collection.AddTransient<TopQueryValidator>();
            collection.AddTransient<FilterQueryValidator>();
            collection.AddTransient<SkipQueryValidator>();
            collection.AddTransient<OrderByQueryValidator>();

            var provider = collection.BuildServiceProvider();

            var routeBuilder = new RouteBuilder(Mock.Of<IApplicationBuilder>(x => x.ApplicationServices == provider));
            routeBuilder.EnableDependencyInjection();

            var builder = new ODataConventionModelBuilder(provider);
            var entitySet = builder.EntitySet<TEntity>(nameof(TEntity));
            entitySet.EntityType.Count().Filter().OrderBy().Expand().Select().Page();
            var model = builder.GetEdmModel();

            var httpContext = new DefaultHttpContext
            {
                RequestServices = provider,
            };

            httpContext.Request.Method = "GET";
            httpContext.Request.Host = new HostString(uri.Host, uri.Port);
            httpContext.Request.Path = uri.LocalPath;
            httpContext.Request.QueryString = new QueryString(uri.Query);

            var context = new ODataQueryContext(model, typeof(TEntity), new Microsoft.AspNet.OData.Routing.ODataPath());
            var options = new ODataQueryOptions<TEntity>(context, httpContext.Request);

            return options;
        }
    }
}

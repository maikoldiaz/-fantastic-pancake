// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupOptions.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Setup
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Ecp.True.Chaos;
    using Ecp.True.Core;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.UI.Filters;
    using Ecp.True.Host.UI.Formatter;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.AspNetCore.StaticFiles;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using NWebsec.Core.Common.Middleware.Options;

    /// <summary>
    /// The startup options.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class SetupOptions
    {
        /// <summary>
        /// Add MVC options.
        /// </summary>
        /// <param name="options">MVC options.</param>
        public static void AddMvcOptions(MvcOptions options)
        {
            ArgumentValidators.ThrowIfNull(options, nameof(options));
            options.EnableEndpointRouting = false;

            options.Filters.Add(typeof(AntiforgeryRequestTokenFilterAttribute));
            options.Filters.Add(typeof(AutoValidateAntiforgeryTokenAttribute));
            options.Filters.Add(typeof(WebExceptionFilterAttribute));
            options.Filters.Add(new ChaosFilterAttribute(ChaosType.Web));

            // add csp report media type to the json input formatter
            // the json patch formatter derives from the json formatter
            // so use the first one supporting application/json
            options.InputFormatters
                    .OfType<SystemTextJsonInputFormatter>()
                    .Single().SupportedMediaTypes
                    .Add("application/csp-report");

            var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            options.Filters.Add(new AuthorizeFilter(policy));
        }

        /// <summary>
        /// Adds the json options.
        /// </summary>
        /// <param name="options">The options.</param>
        public static void AddJsonOptions(MvcNewtonsoftJsonOptions options)
        {
            ArgumentValidators.ThrowIfNull(options, nameof(options));

            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            options.SerializerSettings.Converters.Add(new DecimalValueFormatConverter());
        }

        /// <summary>
        /// Builds the static file options.
        /// </summary>
        /// <returns>The static file options.</returns>
        public static StaticFileOptions BuildStaticFileOptions()
        {
            var mimeTypeProvider = new FileExtensionContentTypeProvider();
            return new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    var headers = context.Context.Response.Headers;
                    var contentType = headers["Content-Type"];
                    if (contentType != "application/x-gzip" && !context.File.Name.EndsWith(".gz", StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }

                    var fileNameToTry = context.File.Name.Substring(0, context.File.Name.Length - 3);
                    if (mimeTypeProvider.TryGetContentType(fileNameToTry, out var mimeType))
                    {
                        headers.Add("Content-Encoding", "gzip");
                        headers["Content-Type"] = mimeType;
                    }
                },
            };
        }

        /// <summary>
        /// Adds the HSTS options.
        /// </summary>
        /// <param name="options">The options.</param>
        public static void AddHstsOptions(IFluentHstsOptions options)
        {
            ArgumentValidators.ThrowIfNull(options, nameof(options));

            options.AllResponses();

            var ageInDays = Convert.ToInt32(DateTime.UtcNow.AddYears(2).Subtract(DateTime.UtcNow).TotalDays);
            options.MaxAge(ageInDays);
        }
    }
}

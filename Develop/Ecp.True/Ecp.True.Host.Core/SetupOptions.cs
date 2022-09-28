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

namespace Ecp.True.Host.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Net.Mime;
    using Ecp.True.Chaos;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Microsoft.AspNet.OData.Formatter;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Mvc.DataAnnotations;
    using Microsoft.Extensions.Localization;

    /// <summary>
    /// The startup options.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class SetupOptions
    {
        /// <summary>
        /// The english culture.
        /// </summary>
        private const string EnglishCulture = "en-US";

        /// <summary>
        /// The spanish culture.
        /// </summary>
        private const string SpanishCulture = "es-CO";

        /// <summary>
        /// Add MVC options.
        /// </summary>
        /// <param name="options">MVC options.</param>
        public static void AddMvcOptions(MvcOptions options)
        {
            ArgumentValidators.ThrowIfNull(options, nameof(options));
            options.EnableEndpointRouting = false;

            var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            options.Filters.Add(new AuthorizeFilter(policy));
            options.Filters.Add(typeof(ApiExceptionFilterAttribute));
            options.Filters.Add(new ChaosFilterAttribute(ChaosType.Api));
            options.MaxValidationDepth = null;

            // Credit: https://github.com/OData/WebApi/issues/1177
            foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
            {
                inputFormatter.SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Json));
            }

            foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
            {
                outputFormatter.SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Json));
            }
        }

        /// <summary>
        /// Adds the API options.
        /// </summary>
        /// <param name="options">The options.</param>
        public static void AddApiOptions(ApiBehaviorOptions options)
        {
            ArgumentValidators.ThrowIfNull(options, nameof(options));
            options.InvalidModelStateResponseFactory = context =>
            {
                var errorCode = context.ModelState.Values.SelectMany(x => x.Errors, (x, e) => new ErrorInfo(e.ErrorMessage));
                var apiResponse = new ErrorResponse(errorCode.ToList());

                return new BadRequestObjectResult(apiResponse);
            };
        }

        /// <summary>
        /// Adds the localization options.
        /// </summary>
        /// <param name="options">The options.</param>
        public static void AddLocalizationOptions(RequestLocalizationOptions options)
        {
            ArgumentValidators.ThrowIfNull(options, nameof(options));

            var supportedCultures = new[]
            {
                new CultureInfo(EnglishCulture),
                new CultureInfo(SpanishCulture),
            };

            options.DefaultRequestCulture = new RequestCulture(SpanishCulture, SpanishCulture);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;

            options.RequestCultureProviders.Clear();
            options.RequestCultureProviders.Add(new CultureProvider(EnglishCulture, SpanishCulture));
        }

        /// <summary>
        /// Adds the data annotation localization options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="localizerFactory">The localizer factory.</param>
        public static void AddDataAnnotationLocalizationOptions(MvcDataAnnotationsLocalizationOptions options, Func<IStringLocalizerFactory, IStringLocalizer> localizerFactory)
        {
            ArgumentValidators.ThrowIfNull(options, nameof(options));
            options.DataAnnotationLocalizerProvider = (type, factory) => localizerFactory(factory);
        }
    }
}

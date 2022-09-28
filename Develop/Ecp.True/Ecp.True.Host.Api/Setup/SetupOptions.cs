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

namespace Ecp.True.Host.Api.Setup
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Ecp.True.Core;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// The startup options.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class SetupOptions
    {
        /// <summary>
        /// Adds the json options.
        /// </summary>
        /// <param name="options">The options.</param>
        public static void AddJsonOptions(MvcNewtonsoftJsonOptions options)
        {
            ArgumentValidators.ThrowIfNull(options, nameof(options));

            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
        }

        /// <summary>
        /// Builds the shared localizer.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns>The string localizer.</returns>
        public static IStringLocalizer BuildSharedLocalizer(IStringLocalizerFactory factory)
        {
            ArgumentValidators.ThrowIfNull(factory, nameof(factory));

            var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
            return factory.Create("SharedResource", assemblyName.Name);
        }
    }
}

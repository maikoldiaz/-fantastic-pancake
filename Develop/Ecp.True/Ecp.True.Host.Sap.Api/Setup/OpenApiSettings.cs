// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenApiSettings.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Sap.Api.Setup
{
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.Core;
    using Ecp.True.Host.Shared.OpenApi;
    using NSwag;
    using NSwag.Generation.AspNetCore;

    /// <summary>
    /// The Open API settings.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class OpenApiSettings
    {
        /// <summary>
        /// Adds the open API configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void AddOpenApiConfiguration(AspNetCoreOpenApiDocumentGeneratorSettings config)
        {
            ArgumentValidators.ThrowIfNull(config, nameof(config));

            config.PostProcess = document =>
            {
                document.Info.Version = "v1";
                document.Info.Title = "TRUE SAP API";
                document.Info.Description = "The SAP Endpoints exposed by TRUE";
                document.Info.Contact = new NSwag.OpenApiContact
                {
                    Name = "Carlos Peña",
                    Email = "carlos.pena@ecopetrol.com.co",
                };

                document.SecurityDefinitions.Add("Roles", new OpenApiSecurityScheme { Type = OpenApiSecuritySchemeType.Http, Scheme = "bearer" });
            };

            config.OperationProcessors.Add(new ModifyOperationProcessor());
            config.OperationProcessors.Add(new AuthorizeOperationProcessor());
        }
    }
}

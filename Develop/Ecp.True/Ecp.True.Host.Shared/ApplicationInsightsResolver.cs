// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationInsightsResolver.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Shared
{
    using Ecp.True.Core;
    using Ecp.True.Logging;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The application insights resolver.
    /// </summary>
    public class ApplicationInsightsResolver : IApplicationInsightsResolver
    {
        /// <summary>
        /// The application insights settings.
        /// </summary>
        private readonly IConfigurationSection applicationSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInsightsResolver"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ApplicationInsightsResolver(IConfiguration configuration)
        {
            ArgumentValidators.ThrowIfNull(configuration, nameof(configuration));
            this.applicationSettings = configuration.GetSection("ApplicationInsights");
        }

        /// <summary>
        /// Resolves the application insights key.
        /// </summary>
        /// <returns>
        /// The application insights key.
        /// </returns>
        public string ResolveApplicationInsightsKey()
        {
            return this.applicationSettings["InstrumentationKey"];
        }
    }
}

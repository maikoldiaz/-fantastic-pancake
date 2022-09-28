// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceExtensions.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Core.Setup
{
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The Service Extension.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Enables the SQL instrumentation.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        public static void EnableSqlInstrumentation(this IServiceCollection serviceCollection)
        {
            serviceCollection.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) => { module.EnableSqlCommandTextInstrumentation = true; });
        }
    }
}

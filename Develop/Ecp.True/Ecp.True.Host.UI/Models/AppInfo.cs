// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppInfo.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Models
{
    using System.Collections.Generic;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Configuration;

    /// <summary>
    /// The application info.
    /// </summary>
    public class AppInfo
    {
        /// <summary>
        /// Gets or sets the scenarios.
        /// </summary>
        /// <value>
        /// The scenarios.
        /// </value>
        public IEnumerable<Scenario> Scenarios { get; set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The business context.
        /// </value>
        public IBusinessContext Context { get; set; }

        /// <summary>
        /// Gets or sets the system configuration.
        /// </summary>
        /// <value>
        /// The system configuration.
        /// </value>
        public SystemSettings SystemConfig { get; set; }

        /// <summary>
        /// Gets or sets the support configuration.
        /// </summary>
        /// <value>
        /// The support configuration.
        /// </value>
        public SupportSettings SupportConfig { get; set; }

        /// <summary>
        /// Gets or sets the instrumentation key.
        /// </summary>
        /// <value>
        /// The instrumentation key.
        /// </value>
        public string InstrumentationKey { get; set; }
    }
}

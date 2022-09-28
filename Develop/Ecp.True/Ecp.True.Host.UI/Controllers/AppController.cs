// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppController.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Graph.Interfaces;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.UI.Models;
    using Ecp.True.Host.UI.Services.Core;
    using Ecp.True.Host.UI.Services.Interfaces;
    using Ecp.True.Logging;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The application controller.
    /// </summary>
    [Authorize]
    public class AppController : Controller
    {
        /// <summary>
        /// The data service.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// The business context.
        /// </summary>
        private readonly IBusinessContext businessContext;

        /// <summary>
        /// The graph service.
        /// </summary>
        private readonly IGraphService graphService;

        /// <summary>
        /// The report service.
        /// </summary>
        private readonly IReportService reportService;

        /// <summary>
        /// The flow service.
        /// </summary>
        private readonly IFlowService flowService;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// The application insights resolver.
        /// </summary>
        private readonly IApplicationInsightsResolver applicationInsightsResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppController" /> class.
        /// </summary>
        /// <param name="dataService">The data service.</param>
        /// <param name="businessContext">The business context.</param>
        /// <param name="graphService">The graph service.</param>
        /// <param name="reportService">The report service.</param>
        /// <param name="flowService">The flow service.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="applicationInsightsResolver">The application insights resolver.</param>
        public AppController(
            IDataService dataService,
            IBusinessContext businessContext,
            IGraphService graphService,
            IReportService reportService,
            IFlowService flowService,
            IConfigurationHandler configurationHandler,
            IApplicationInsightsResolver applicationInsightsResolver)
        {
            this.dataService = dataService;
            this.businessContext = businessContext;
            this.graphService = graphService;
            this.reportService = reportService;
            this.flowService = flowService;
            this.configurationHandler = configurationHandler;
            this.applicationInsightsResolver = applicationInsightsResolver;
        }

        /// <summary>
        /// Gets the application information asynchronous.
        /// </summary>
        /// <param name="version">the version.</param>
        /// <returns>The task.</returns>
        [HttpGet]
        [Route("v{version}/bootstrap")]
        [TrueAuthorize]
        public async Task<JsonResult> GetAppInfoAsync(string version)
        {
            var scenarios = await this.dataService.GetEntityAsync<IEnumerable<Scenario>>(version, "scenarios").ConfigureAwait(false);
            var image = await this.graphService.GetMyPictureBase64Async(this.HttpContext, (ClaimsIdentity)this.User.Identity).ConfigureAwait(false);

            this.businessContext.PopulateImage(image);

            var appInfo = new AppInfo
            {
                Scenarios = scenarios,
                Context = this.businessContext,
                SystemConfig = await this.configurationHandler.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings).ConfigureAwait(false),
                SupportConfig = await this.configurationHandler.GetConfigurationAsync<SupportSettings>(ConfigurationConstants.SupportSettings).ConfigureAwait(false),
                InstrumentationKey = this.applicationInsightsResolver.ResolveApplicationInsightsKey(),
            };

            return this.Json(appInfo);
        }

        /// <summary>
        /// Gets the application information asynchronous.
        /// </summary>
        /// <param name="key">the key.</param>
        /// <returns>The task.</returns>
        [HttpGet]
        [Route("v{version}/reportConfigs/{key}")]
        [TrueAuthorize]
        public async Task<JsonResult> GetReportConfigAsync(string key)
        {
            var reportDetails = await this.reportService.GetReportDetailsAsync(key).ConfigureAwait(false);
            return this.Json(reportDetails);
        }

        /// <summary>
        /// Gets the application information asynchronous.
        /// </summary>
        /// <param name="key">the key.</param>
        /// <returns>The task.</returns>
        [HttpGet]
        [Route("v{version}/flowConfigs/{key}")]
        [TrueAuthorize]
        public async Task<JsonResult> GetFlowConfigAsync(string key)
        {
            var flowModel = await this.flowService.GetFlowConfigurationAsync(key).ConfigureAwait(false);
            return this.Json(flowModel);
        }

        /// <summary>
        /// Loads the index page.
        /// </summary>
        /// <returns>The action result.</returns>
        [Route("{*allPaths}")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Gets the identifiers for the security groups reported for the current user,
        /// to help troubleshoot access and permissions issues.
        /// </summary>
        /// <returns>A JSON array with the GUIDs of the security groups.</returns>
        [HttpGet]
        [Route("v{version}/myGroups")]
        [TrueAuthorize]
        public JsonResult GetUserGroups()
        {
            var theGroups = this.HttpContext.User.Claims.Where(claim => claim.Type == "groups").Select(v => v.Value);
            return this.Json(theGroups);
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipRuleProxyTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.OwnershipRules.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.OwnershipRules.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using Ecp.True.Proxies.OwnershipRules.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;

    /// <summary>
    /// Ownership Rule Service Tests.
    /// </summary>
    [TestClass]
    public class OwnershipRuleProxyTests
    {
        /// <summary>
        /// The ownership rule service.
        /// </summary>
        private OwnershipRuleProxy ownershipRuleService;

        /// <summary>
        /// The mock ownership rule client.
        /// </summary>
        private Mock<IOwnershipRuleClient> mockOwnershipRuleClient;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<OwnershipRuleProxy>> mockLogger;

        /// <summary>
        /// Intializes the method.
        /// </summary>
        [TestInitialize]
        public void IntializeMethod()
        {
            this.mockOwnershipRuleClient = new Mock<IOwnershipRuleClient>();
            this.mockLogger = new Mock<ITrueLogger<OwnershipRuleProxy>>();

            this.ownershipRuleService = new OwnershipRuleProxy(this.mockOwnershipRuleClient.Object, this.mockLogger.Object);
            this.ownershipRuleService.Initialize(new OwnershipRuleSettings
            {
                BasePath = "TestBase",
                OwnershipRulePath = "TestPath",
            });
        }

        /// <summary>
        /// Ownerships the rule service get active rules should get active rules asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OwnershipRuleService_GetActiveRules_Should_GetActiveRulesAsync()
        {
            using (var response = this.GetOwnershipRulesResponse())
            {
                this.mockOwnershipRuleClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(response);

                var result = await this.ownershipRuleService.GetActiveRulesAsync().ConfigureAwait(false);
                Assert.IsNotNull(result);
                Assert.AreEqual(5, result.AuditedSteps.ToList().Count);
                Assert.AreEqual(3, result.OwnershipRuleConnections.Count());
                Assert.AreEqual(2, result.NodeOwnershipRules.Count());
                Assert.AreEqual(2, result.NodeProductOwnershipRules.Count());

                this.mockOwnershipRuleClient.Verify(a => a.PostAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
                this.mockOwnershipRuleClient.Verify(a => a.Initialize(It.IsAny<OwnershipRuleSettings>()), Times.Once);
            }
        }

        /// <summary>
        /// Ownerships the rule service get active rules throws exception when error response asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(JsonReaderException))]
        public async Task OwnershipRuleService_GetActiveRules_ThrowsException_WhenErrorResponseAsync()
        {
            using (var response = this.GetOwnershipRulesResponse())
            {
                this.mockOwnershipRuleClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(this.GetErrorOwnershipRulesResponse);

                var result = await this.ownershipRuleService.GetActiveRulesAsync().ConfigureAwait(false);
                Assert.IsNull(result);
            }
        }

        /// <summary>
        /// Ownerships the rule service get inactive rules should get inactive rules asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetInactiveRulesAsync_Should_GetInactiveRulesAsync()
        {
            using (var response = this.GetOwnershipRulesResponse())
            {
                this.mockOwnershipRuleClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(response);
                var result = await this.ownershipRuleService.GetInactiveRulesAsync().ConfigureAwait(false);
                Assert.IsNotNull(result);
                Assert.AreEqual(5, result.AuditedSteps.ToList().Count);
                Assert.AreEqual(3, result.OwnershipRuleConnections.Count());
                Assert.AreEqual(2, result.NodeOwnershipRules.Count());
                Assert.AreEqual(2, result.NodeProductOwnershipRules.Count());

                this.mockOwnershipRuleClient.Verify(a => a.PostAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
                this.mockOwnershipRuleClient.Verify(a => a.Initialize(It.IsAny<OwnershipRuleSettings>()), Times.Once);
            }
        }

        /// <summary>
        /// Should process ownership asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ProcessOwnershipAsync_Should_ProcessOwnershipAsync()
        {
            using (var response = this.GetOwnershipRulesResponse())
            {
                var ownershipRuleRequest = new OwnershipRuleRequest
                {
                    InventoryOperationalData = new List<InventoryOperationalData>(),
                    PreviousInventoryOperationalData = new List<PreviousInventoryOperationalData>(),
                    MovementsOperationalData = new List<MovementOperationalData>(),
                    PreviousMovementsOperationalData = new List<PreviousMovementOperationalData>(),
                    NodeConfigurations = new List<NodeConfiguration>(),
                    NodeConnections = new List<NodeConnection>(),
                    Events = new List<Event>(),
                    Contracts = new List<Contract>(),
                };

                this.mockOwnershipRuleClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(response);
                var result = await this.ownershipRuleService.ProcessOwnershipAsync(ownershipRuleRequest, 123456).ConfigureAwait(false);
                Assert.IsNotNull(result);
                Assert.AreEqual(5, result.AuditedSteps.ToList().Count);
                Assert.AreEqual(3, result.OwnershipRuleConnections.Count());
                Assert.AreEqual(2, result.NodeOwnershipRules.Count());
                Assert.AreEqual(2, result.NodeProductOwnershipRules.Count());

                Assert.IsNotNull(ownershipRuleRequest.RawRequest);
                this.mockOwnershipRuleClient.Verify(a => a.PostAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
                this.mockOwnershipRuleClient.Verify(a => a.Initialize(It.IsAny<OwnershipRuleSettings>()), Times.Once);
            }
        }

        /// <summary>
        /// Gets the ownership rules response.
        /// </summary>
        /// <returns>Returns Http Response message.</returns>
        private HttpResponseMessage GetOwnershipRulesResponse()
        {
            var filePath = @"Response/OwnershipRulesReponse.json";
            var fileText = File.ReadAllText(filePath);
            var response = new HttpResponseMessage();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Content = new StringContent(fileText);
            return response;
        }

        /// <summary>
        /// Gets the error ownership rules response.
        /// </summary>
        /// <returns>Returns Http Response message.</returns>
        private HttpResponseMessage GetErrorOwnershipRulesResponse()
        {
            var response = new HttpResponseMessage();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Content = new StringContent(string.Empty);
            return response;
        }
    }
}

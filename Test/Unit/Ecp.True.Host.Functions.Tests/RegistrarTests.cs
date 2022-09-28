// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrarTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Tests
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Transform;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The registrar test.
    /// </summary>
    [TestClass]
    public class RegistrarTests
    {
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        private readonly Mock<ITrueLogger<Registrar>> mockLogger = new Mock<ITrueLogger<Registrar>>();

        /// <summary>
        /// The mock service provider.
        /// </summary>
        private readonly Mock<IServiceProvider> mockServiceProvider = new Mock<IServiceProvider>();

        /// <summary>
        /// The mock registration processor.
        /// </summary>
        private readonly Mock<IRegistrationProcessor> mockRegistrationProcessor = new Mock<IRegistrationProcessor>();

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The mock connection factory.
        /// </summary>
        private Mock<IConnectionFactory> mockConnectionFactory = new Mock<IConnectionFactory>();

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigHandler = new Mock<IConfigurationHandler>();

        /// <summary>
        /// Gets the mock SQL token provider.
        /// </summary>
        /// <value>
        /// The mock SQL token provider.
        /// </value>
        private Mock<ISqlTokenProvider> mockSqlTokenProvider = new Mock<ISqlTokenProvider>();

        /// <summary>
        /// The mock chaos manager.
        /// </summary>
        private Mock<IChaosManager> mockChaosManager = new Mock<IChaosManager>();

        /// <summary>
        /// The registrar.
        /// </summary>
        private Registrar registrar;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.registrar = new Registrar(this.mockLogger.Object, this.mockRegistrationProcessor.Object, this.mockServiceProvider.Object);
        }

        [TestMethod]
        public async Task RegisterMovementAsync_RegisterAsync()
        {
            this.SetupMocks(true);
            var message = new FileRegistrationTransaction { MessageType = MessageType.Movement };
            await this.registrar.RegisterMovementAsync(message, "label", new ExecutionContext()).ConfigureAwait(false);
            this.mockRegistrationProcessor.Verify(a => a.RegisterMovementAsync(message), Times.Once);
        }

        [TestMethod]
        public async Task RegisterInventoryAsync_RegisterAsync()
        {
            this.SetupMocks(true);
            var message = new FileRegistrationTransaction { MessageType = MessageType.Inventory };
            await this.registrar.RegisterInventoryAsync(message, "label", new ExecutionContext()).ConfigureAwait(false);
            this.mockRegistrationProcessor.Verify(a => a.RegisterInventoryAsync(message), Times.Once);
        }

        [TestMethod]
        public async Task RegisterEventsAsync_RegisterAsync()
        {
            this.SetupMocks(true);
            var message = new FileRegistrationTransaction { MessageType = MessageType.Events };
            await this.registrar.RegisterEventAsync(message, "label", new ExecutionContext()).ConfigureAwait(false);
            this.mockRegistrationProcessor.Verify(a => a.RegisterEventAsync(message), Times.Once);
        }

        [TestMethod]
        public async Task RegisterContractAsync_RegisterAsync()
        {
            this.SetupMocks(true);
            var message = new FileRegistrationTransaction { MessageType = MessageType.Contract };
            await this.registrar.RegisterContractAsync(message, "label", new ExecutionContext()).ConfigureAwait(false);
            this.mockRegistrationProcessor.Verify(a => a.RegisterContractAsync(message), Times.Once);
        }

        private void SetupMocks(bool isReady)
        {
            this.mockConnectionFactory = new Mock<IConnectionFactory>();
            this.mockConfigHandler = new Mock<IConfigurationHandler>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockSqlTokenProvider = new Mock<ISqlTokenProvider>();
            this.mockChaosManager = new Mock<IChaosManager>();

            this.mockConnectionFactory.Setup(m => m.SetupStorageConnection(It.IsAny<string>()));
            this.mockConnectionFactory.SetupGet(m => m.IsReady).Returns(isReady);

            this.mockAzureClientFactory.SetupGet(m => m.IsReady).Returns(isReady);
            this.mockAzureClientFactory.Setup(m => m.Initialize(It.IsAny<AzureConfiguration>()));

            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConnectionFactory))).Returns(this.mockConnectionFactory.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConfigurationHandler))).Returns(this.mockConfigHandler.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IAzureClientFactory))).Returns(this.mockAzureClientFactory.Object);
            ////this.mockServiceProvider.Setup(s => s.GetService(typeof(IDataGeneratorService))).Returns(this.mockDataGeneratorService.Object);
            this.mockServiceProvider.Setup(r => r.GetService(typeof(ISqlTokenProvider))).Returns(this.mockSqlTokenProvider.Object);
            this.mockServiceProvider.Setup(r => r.GetService(typeof(IChaosManager))).Returns(this.mockChaosManager.Object);
        }
    }
}
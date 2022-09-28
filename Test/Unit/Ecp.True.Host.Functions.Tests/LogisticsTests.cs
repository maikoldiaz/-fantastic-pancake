// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticsTests.cs" company="Microsoft">
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
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Ownership;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SqlConstants = Ecp.True.DataAccess.Sql.Constants;

    [TestClass]
    public class LogisticsTests
    {
        /// <summary>
        /// The ownership calculator.
        /// </summary>
        private Logistics logistics;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<Logistics>> mockLogger;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private Mock<IServiceProvider> mockServiceProvider;

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// The mock connection factory.
        /// </summary>
        private Mock<IConnectionFactory> mockConnectionFactory;

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigHandler;

        /// <summary>
        /// The mock data generator service.
        /// </summary>
        private Mock<IDataGeneratorService> mockDataGeneratorService;

        /// <summary>
        /// The mock ownership calculation service.
        /// </summary>
        private Mock<IOwnershipCalculationService> mockOwnershipCalculationService;

        /// <summary>
        /// The mock ownership calculation service.
        /// </summary>
        private Mock<IOwnershipRuleProcessor> mockOwnershipRuleProcessor;

        /// <summary>
        /// The mock ownership calculation service.
        /// </summary>
        private Mock<IExcelService> mockExcelService;

        /// <summary>
        /// Gets the mock SQL token provider.
        /// </summary>
        /// <value>
        /// The mock SQL token provider.
        /// </value>
        private Mock<ISqlTokenProvider> mockSqlTokenProvider;

        /// <summary>
        /// The mock chaos manager.
        /// </summary>
        private Mock<IChaosManager> mockChaosManager;

        [TestInitialize]
        public void Initialize()
        {
            this.mockDataGeneratorService = new Mock<IDataGeneratorService>();
            this.mockOwnershipCalculationService = new Mock<IOwnershipCalculationService>();
            this.mockLogger = new Mock<ITrueLogger<Logistics>>();
            this.mockServiceProvider = new Mock<IServiceProvider>();
            this.mockExcelService = new Mock<IExcelService>();
            this.mockOwnershipRuleProcessor = new Mock<IOwnershipRuleProcessor>();
            this.logistics = new Logistics(
                 this.mockLogger.Object,
                 this.mockServiceProvider.Object,
                 this.mockOwnershipCalculationService.Object,
                 this.mockDataGeneratorService.Object,
                 this.mockExcelService.Object);
        }

        /// <summary>
        /// Generates the logistics asynchronous should generate logistics excel asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GenerateLogisticsAsync_ShouldGenerateLogisticsExcelAsync()
        {
            this.SetupMocks(true);
            var dataSet = new DataSet() { Locale = CultureInfo.InvariantCulture };
            LogisticsInfo logisticsInfo = new LogisticsInfo
            {
                Ticket = new Ticket
                {
                    TicketId = 1,
                    CategoryElement = new Entities.Admin.CategoryElement { Name = "Segement" },
                    Owner = new Entities.Admin.CategoryElement { Name = "Ecopetrol" },
                },
            };
            this.mockOwnershipCalculationService.Setup(a => a.GetLogisticsDetailsAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(logisticsInfo);
            this.mockDataGeneratorService.Setup(a => a.TransformLogisticsData(It.IsAny<LogisticsInfo>())).Returns(dataSet);
            this.mockExcelService.Setup(a => a.ExportAndUploadLogisticsExcelAsync(It.IsAny<DataSet>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            await this.logistics.GenerateOfficialLogisticsAsync(new QueueMessage { TicketId = 1, SystemTypeId = 6 }, null, null, new ExecutionContext() { InvocationId = Guid.NewGuid() }).ConfigureAwait(false);
            dataSet.Dispose();

            this.mockOwnershipCalculationService.Verify(a => a.GetLogisticsDetailsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            this.mockDataGeneratorService.Verify(a => a.TransformLogisticsData(logisticsInfo), Times.Once);
            this.mockExcelService.Verify(a => a.ExportAndUploadLogisticsExcelAsync(It.IsAny<DataSet>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Generates the logistics HandleLogisticsErrorsAsyncs.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GenerateLogisticsAsync_ShouldHandleLogisticsErrorsAsyncAsync()
        {
            this.SetupMocks(true);
            var dataSet = new DataSet() { Locale = CultureInfo.InvariantCulture };
            LogisticsInfo logisticsInfo = new LogisticsInfo
            {
                Ticket = new Ticket
                {
                    TicketId = 1,
                    CategoryElement = new Entities.Admin.CategoryElement { Name = "Segement" },
                    Owner = new Entities.Admin.CategoryElement { Name = "Ecopetrol" },
                },
            };
            this.mockOwnershipCalculationService.Setup(a => a.GetLogisticsDetailsAsync(It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new Exception());
            this.mockDataGeneratorService.Setup(a => a.TransformLogisticsData(It.IsAny<LogisticsInfo>())).Returns(dataSet);
            this.mockExcelService.Setup(a => a.ExportAndUploadLogisticsExcelAsync(It.IsAny<DataSet>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            await this.logistics.GenerateOfficialLogisticsAsync(new QueueMessage { TicketId = 1, SystemTypeId = 6 }, null, null, new ExecutionContext() { InvocationId = Guid.NewGuid() }).ConfigureAwait(false);
            dataSet.Dispose();
            this.mockOwnershipCalculationService.Verify(a => a.GetLogisticsDetailsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            this.mockDataGeneratorService.Verify(a => a.TransformLogisticsData(logisticsInfo), Times.Never);
            this.mockExcelService.Verify(a => a.ExportAndUploadLogisticsExcelAsync(It.IsAny<DataSet>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        /// <summary>
        /// Generates the logistics HandleLogistics Sql ErrorsAsyncs.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GenerateLogisticsAsync_ShouldHandleLogisticsSqlErrorsAsyncAsync()
        {
            this.SetupMocks(true);
            var dataSet = new DataSet() { Locale = CultureInfo.InvariantCulture };
            LogisticsInfo logisticsInfo = new LogisticsInfo
            {
                Ticket = new Ticket
                {
                    TicketId = 1,
                    CategoryElement = new Entities.Admin.CategoryElement { Name = "Segement" },
                    Owner = new Entities.Admin.CategoryElement { Name = "Ecopetrol" },
                },
            };
            var exception = FormatterServices.GetUninitializedObject(typeof(SqlException))
                as SqlException;
            this.mockOwnershipCalculationService.Setup(a => a.GetLogisticsDetailsAsync(It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(exception);
            this.mockDataGeneratorService.Setup(a => a.TransformLogisticsData(It.IsAny<LogisticsInfo>())).Returns(dataSet);
            this.mockExcelService.Setup(a => a.ExportAndUploadLogisticsExcelAsync(It.IsAny<DataSet>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            await this.logistics.GenerateOfficialLogisticsAsync(new QueueMessage { TicketId = 1, SystemTypeId = 6 }, null, null, new ExecutionContext() { InvocationId = Guid.NewGuid() }).ConfigureAwait(false);
            dataSet.Dispose();
            this.mockOwnershipCalculationService.Verify(a => a.GetLogisticsDetailsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            this.mockDataGeneratorService.Verify(a => a.TransformLogisticsData(logisticsInfo), Times.Never);
            this.mockExcelService.Verify(a => a.ExportAndUploadLogisticsExcelAsync(It.IsAny<DataSet>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        /// <summary>
        /// Generates the logistics HandleLogistics Sql NoSapHomologationForMovementType.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GenerateLogisticsAsync_ShouldNoSapHomologationForMovementTypecAsync()
        {
            this.SetupMocks(true);
            var dataSet = new DataSet() { Locale = CultureInfo.InvariantCulture };
            LogisticsInfo logisticsInfo = new LogisticsInfo
            {
                Ticket = new Ticket
                {
                    TicketId = 1,
                    CategoryElement = new Entities.Admin.CategoryElement { Name = "Segement" },
                    Owner = new Entities.Admin.CategoryElement { Name = "Ecopetrol" },
                },
            };
            var sqlException = FormatterServices.GetUninitializedObject(typeof(SqlException)) as SqlException;
            var messageField = typeof(SqlException).GetField("_message", BindingFlags.NonPublic | BindingFlags.Instance);
            messageField.SetValue(sqlException, SqlConstants.NoSapHomologationForMovementType);
            this.mockOwnershipCalculationService.Setup(a => a.GetLogisticsDetailsAsync(It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(sqlException);
            this.mockDataGeneratorService.Setup(a => a.TransformLogisticsData(It.IsAny<LogisticsInfo>())).Returns(dataSet);
            this.mockExcelService.Setup(a => a.ExportAndUploadLogisticsExcelAsync(It.IsAny<DataSet>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            await this.logistics.GenerateOfficialLogisticsAsync(new QueueMessage { TicketId = 1, SystemTypeId = 6 }, null, null, new ExecutionContext() { InvocationId = Guid.NewGuid() }).ConfigureAwait(false);
            dataSet.Dispose();
            this.mockOwnershipCalculationService.Verify(a => a.GetLogisticsDetailsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            this.mockDataGeneratorService.Verify(a => a.TransformLogisticsData(logisticsInfo), Times.Never);
            this.mockExcelService.Verify(a => a.ExportAndUploadLogisticsExcelAsync(It.IsAny<DataSet>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        /// <summary>
        /// Generates the logistics HandleLogistics Sql NoSapHomologationForMovementType.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GenerateLogisticsAsync_ShouldNoInvalidCombinationToSivMovementAsync()
        {
            this.SetupMocks(true);
            var dataSet = new DataSet() { Locale = CultureInfo.InvariantCulture };
            LogisticsInfo logisticsInfo = new LogisticsInfo
            {
                Ticket = new Ticket
                {
                    TicketId = 1,
                    CategoryElement = new Entities.Admin.CategoryElement { Name = "Segement" },
                    Owner = new Entities.Admin.CategoryElement { Name = "Ecopetrol" },
                },
            };
            var sqlException = FormatterServices.GetUninitializedObject(typeof(SqlException)) as SqlException;
            var messageField = typeof(SqlException).GetField("_message", BindingFlags.NonPublic | BindingFlags.Instance);
            messageField.SetValue(sqlException, SqlConstants.InvalidCombinationToSivMovement);
            this.mockOwnershipCalculationService.Setup(a => a.GetLogisticsDetailsAsync(It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(sqlException);
            this.mockDataGeneratorService.Setup(a => a.TransformLogisticsData(It.IsAny<LogisticsInfo>())).Returns(dataSet);
            this.mockExcelService.Setup(a => a.ExportAndUploadLogisticsExcelAsync(It.IsAny<DataSet>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            await this.logistics.GenerateOfficialLogisticsAsync(new QueueMessage { TicketId = 1, SystemTypeId = 6 }, null, null, new ExecutionContext() { InvocationId = Guid.NewGuid() }).ConfigureAwait(false);
            dataSet.Dispose();
            this.mockOwnershipCalculationService.Verify(a => a.GetLogisticsDetailsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            this.mockDataGeneratorService.Verify(a => a.TransformLogisticsData(logisticsInfo), Times.Never);
            this.mockExcelService.Verify(a => a.ExportAndUploadLogisticsExcelAsync(It.IsAny<DataSet>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        /// <summary>
        /// Setups the mocks.
        /// </summary>
        /// <param name="isReady">if set to <c>true</c> [is ready].</param>
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

            this.mockServiceProvider.Setup(s => s.GetService(typeof(ITrueLogger<OwnershipGenerator>))).Returns(this.mockLogger.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConfigurationHandler))).Returns(this.mockConfigHandler.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IAzureClientFactory))).Returns(this.mockAzureClientFactory.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IDataGeneratorService))).Returns(this.mockDataGeneratorService.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IOwnershipCalculationService))).Returns(this.mockOwnershipCalculationService.Object);
            this.mockServiceProvider.Setup(r => r.GetService(typeof(ISqlTokenProvider))).Returns(this.mockSqlTokenProvider.Object);
            this.mockServiceProvider.Setup(r => r.GetService(typeof(IChaosManager))).Returns(this.mockChaosManager.Object);
        }
    }
}

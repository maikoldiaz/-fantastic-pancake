// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MasterProcessorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Tests.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Processors.Api.Admin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Role = Ecp.True.Entities.Core.Role;

    /// <summary>
    /// The Master Processor Tests.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Api.Tests.ProcessorTestBase" />
    [TestClass]
    public class MasterProcessorTests : ProcessorTestBase
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private MasterProcessor processor;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandler;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();

            this.processor = new MasterProcessor(this.mockFactory.Object, this.mockConfigurationHandler.Object);
        }

        /// <summary>
        /// Gets the logistic locations should get logistic center from repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetLogisticLocationsAsync_ShouldGetLogisticCenterFromRepository_WhenInvokedAsync()
        {
            var logisticCenters = new[] { new LogisticCenter() };
            var repoMock = new Mock<IRepository<LogisticCenter>>();
            repoMock.Setup(r => r.GetAllAsync(x => x.IsActive)).ReturnsAsync(logisticCenters);
            this.mockFactory.Setup(m => m.CreateRepository<LogisticCenter>()).Returns(repoMock.Object);

            var result = await this.processor.GetLogisticCentersAsync().ConfigureAwait(false);

            Assert.AreEqual(result, logisticCenters);
            repoMock.Verify(m => m.GetAllAsync(x => x.IsActive), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<LogisticCenter>(), Times.Once);
        }

        /// <summary>
        /// Gets all storage locations should get storage locations from repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetAllStorageLocationsAsync_ShouldGetStorageLocationsFromRepository_WhenInvokedAsync()
        {
            var storageLocations = new[] { new StorageLocation() };
            var repoMock = new Mock<IRepository<StorageLocation>>();
            repoMock.Setup(r => r.GetAllAsync(x => x.IsActive)).ReturnsAsync(storageLocations);
            this.mockFactory.Setup(m => m.CreateRepository<StorageLocation>()).Returns(repoMock.Object);

            var result = await this.processor.GetStorageLocationsAsync().ConfigureAwait(false);

            Assert.AreEqual(result, storageLocations);
            repoMock.Verify(m => m.GetAllAsync(x => x.IsActive), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<StorageLocation>(), Times.Once);
        }

        /// <summary>
        /// Gets all products asynchronous should get storage locations from repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetAllProductsAsync_ShouldGetProductsFromRepository_WhenInvokedAsync()
        {
            var products = new[] { new Product() };
            var repoMock = new Mock<IRepository<Product>>();
            repoMock.Setup(r => r.GetAllAsync(x => x.IsActive)).ReturnsAsync(products);
            this.mockFactory.Setup(m => m.CreateRepository<Product>()).Returns(repoMock.Object);

            var result = await this.processor.GetProductsAsync().ConfigureAwait(false);

            Assert.AreEqual(result, products);
            repoMock.Verify(m => m.GetAllAsync(x => x.IsActive), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<Product>(), Times.Once);
        }

        /// <summary>
        /// The get scenarios call should get the scenarios along with the features.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetScenariosAsync_ShouldGetScenariosFromRepository_WhenInvokedAsync()
        {
            var groupRoleConfig = new UserRoleSettings();
            groupRoleConfig.Mapping.Add(Role.Administrator, "a7479414-8c25-4afc-a7b4-b7652032649d");
            groupRoleConfig.Mapping.Add(Role.Approver, "0e8952c4-c7cc-44dd-81e5-1b08fbc8caf6");
            groupRoleConfig.Mapping.Add(Role.ProfessionalSegmentBalances, "1cade21c-daea-46e4-b634-e3aa652c8e1e");
            groupRoleConfig.Mapping.Add(Role.Programmer, "bc28029f-4156-44ef-ac68-2d83eb5102da");
            groupRoleConfig.Mapping.Add(Role.Query, "f27b4d64-a4ba-4322-8d01-1555847e29d8");

            var roles = new List<string>
            {
                "a7479414-8c25-4afc-a7b4-b7652032649d",
                "0e8952c4-c7cc-44dd-81e5-1b08fbc8caf6",
            };

            var featureRoles = new List<FeatureRole>
            {
                new FeatureRole
                {
                    FeatureId = 1,
                    RoleId = 1,
                    Feature = new Feature
                    {
                        FeatureId = 1,
                        Name = "some feature",
                        ScenarioId = 1,
                        Scenario = new Scenario
                        {
                            ScenarioId = 1,
                            Sequence = 1,
                            Name = "some name",
                        },
                    },
                },
            };

            var scenarios = new List<Scenario>
            {
                new Scenario
                {
                    ScenarioId = 1,
                    Sequence = 1,
                    Name = "some name",
                    Features = new List<Feature>
                    {
                        new Feature
                        {
                            FeatureId = 1,
                            Name = "some feature",
                        },
                    },
                },
            };

            var repoMock = new Mock<IRepository<FeatureRole>>();
            repoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<FeatureRole, bool>>>(), "Feature", "Role", "Feature.Scenario")).ReturnsAsync(featureRoles);
            this.mockFactory.Setup(m => m.CreateRepository<FeatureRole>()).Returns(repoMock.Object);
            this.mockConfigurationHandler.Setup(m => m.GetConfigurationAsync<UserRoleSettings>(ConfigurationConstants.UserRoleSettings)).ReturnsAsync(groupRoleConfig);

            var result = await this.processor.GetScenariosByRoleAsync(roles).ConfigureAwait(false);
            repoMock.Verify(m => m.GetAllAsync(It.IsAny<Expression<Func<FeatureRole, bool>>>(), "Feature", "Role", "Feature.Scenario"), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<FeatureRole>(), Times.Once);
            Assert.AreEqual(scenarios[0].ScenarioId, result.FirstOrDefault().ScenarioId);
            Assert.AreEqual(scenarios[0].Sequence, result.FirstOrDefault().Sequence);
            Assert.AreEqual(scenarios[0].Name, result.FirstOrDefault().Name);
            Assert.AreEqual(scenarios[0].Features.FirstOrDefault().Name, result.FirstOrDefault().Features.FirstOrDefault().Name);
        }

        /// <summary>
        /// Gets all products asynchronous should get storage locations from repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetSystemTypesAsync_ShouldGetSystemTypesFromRepository_WhenInvokedAsync()
        {
            var items = new[] { new SystemTypeEntity() };
            var repoMock = new Mock<IRepository<SystemTypeEntity>>();
            repoMock.Setup(r => r.GetAllAsync(null)).ReturnsAsync(items);
            this.mockFactory.Setup(m => m.CreateRepository<SystemTypeEntity>()).Returns(repoMock.Object);

            var result = await this.processor.GetSystemTypesAsync().ConfigureAwait(false);

            Assert.AreEqual(result, items);
            repoMock.Verify(m => m.GetAllAsync(null), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<SystemTypeEntity>(), Times.Once);
        }

        /// <summary>
        /// Gets all products asynchronous should get storage locations from repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetUsersAsync_ShouldGetUsersFromRepository_WhenInvokedAsync()
        {
            var items = new[] { new User() };
            var repoMock = new Mock<IRepository<User>>();
            repoMock.Setup(r => r.GetAllAsync(null)).ReturnsAsync(items);
            this.mockFactory.Setup(m => m.CreateRepository<User>()).Returns(repoMock.Object);

            var result = await this.processor.GetUsersAsync().ConfigureAwait(false);

            Assert.AreEqual(result, items);
            repoMock.Verify(m => m.GetAllAsync(null), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<User>(), Times.Once);
        }

        /// <summary>
        /// Gets the variable types asynchronous should get variable types from repository when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetVariableTypesAsync_ShouldGetVariableTypesFromRepository_WhenInvokedAsync()
        {
            var variableTypes = new[] { new VariableTypeEntity() };
            var repoMock = new Mock<IRepository<VariableTypeEntity>>();
            repoMock.Setup(r => r.GetAllAsync(null)).ReturnsAsync(variableTypes);
            this.mockFactory.Setup(m => m.CreateRepository<VariableTypeEntity>()).Returns(repoMock.Object);

            var result = await this.processor.GetVariableTypesAsync().ConfigureAwait(false);

            Assert.AreEqual(result, variableTypes);
            repoMock.Verify(m => m.GetAllAsync(null), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<VariableTypeEntity>(), Times.Once);
        }

        /// <summary>
        /// Gets the icons asynchronous should get icons from repository when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetIconsAsync_ShouldGetIconsFromRepository_WhenInvokedAsync()
        {
            var icons = new[] { new Icon() }.AsQueryable();
            var repoMock = new Mock<IRepository<Icon>>();
            repoMock.Setup(r => r.GetAllAsync(null)).ReturnsAsync(icons);
            this.mockFactory.Setup(m => m.CreateRepository<Icon>()).Returns(repoMock.Object);

            var result = await this.processor.GetIconsAsync().ConfigureAwait(false);

            Assert.AreEqual(result, icons);
            repoMock.Verify(m => m.GetAllAsync(null), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<Icon>(), Times.Once);
        }

        /// <summary>
        /// Gets the origin types asynchronous should get origin types from repository when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetOriginTypesAsync_ShouldGetOriginTypesFromRepository_WhenInvokedAsync()
        {
            var originTypes = new[] { new OriginType() }.AsQueryable();
            var repoMock = new Mock<IRepository<OriginType>>();
            repoMock.Setup(r => r.GetAllAsync(null)).ReturnsAsync(originTypes);
            this.mockFactory.Setup(m => m.CreateRepository<OriginType>()).Returns(repoMock.Object);

            var result = await this.processor.GetOriginTypesAsync().ConfigureAwait(false);

            Assert.AreEqual(result, originTypes);
            repoMock.Verify(m => m.GetAllAsync(null), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<OriginType>(), Times.Once);
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MasterControllerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests.Admin
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class MasterControllerTests
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private MasterController controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<IMasterProcessor> mockProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterControllerTests"/> class.
        /// </summary>
        public MasterControllerTests()
        {
            this.HttpContext = new DefaultHttpContext();
        }

        /// <summary>
        /// Gets the HTTP context.
        /// </summary>
        /// <value>
        /// The HTTP context.
        /// </value>
        protected HttpContext HttpContext { get; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockProcessor = new Mock<IMasterProcessor>();
            this.controller = new MasterController(this.mockProcessor.Object);

            var mockClaim = new Mock<IEnumerable<Claim>>();
            var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            mockClaimsPrincipal.Setup(x => x.Claims).Returns(mockClaim.Object);
            this.HttpContext.User = mockClaimsPrincipal.Object;

            var actionContext = new ActionContext();
            actionContext.HttpContext = this.HttpContext;
            actionContext.RouteData = new RouteData();
            actionContext.ActionDescriptor = new ControllerActionDescriptor();
            var context = new ControllerContext(actionContext);
            this.controller.ControllerContext = context;
        }

        /// <summary>
        /// Gets all logistic centers should invoke processor to return logistic centers.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetAllLogisticCenters_ShouldInvokeProcessor_ToReturnLogisticCentersAsync()
        {
            var logisticCenter = new[] { new LogisticCenter() };
            this.mockProcessor.Setup(m => m.GetLogisticCentersAsync()).ReturnsAsync(logisticCenter);

            var result = await this.controller.GetLogisticCentersAsync().ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GetLogisticCentersAsync(), Times.Once());
        }

        /// <summary>
        /// Gets all storage locations should invoke processor to return storage locations.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetAllStorageLocations_ShouldInvokeProcessor_ToReturnStorageLocationsAsync()
        {
            var storageLocation = new[] { new StorageLocation() };
            this.mockProcessor.Setup(m => m.GetStorageLocationsAsync()).ReturnsAsync(storageLocation);

            var result = await this.controller.QueryStorageLocationsAsync().ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EnumerableQuery));
            this.mockProcessor.Verify(c => c.GetStorageLocationsAsync(), Times.Once());
        }

        /// <summary>
        /// Gets all products should invoke processor to return products.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetAllProducts_ShouldInvokeProcessor_ToReturnProductsAsync()
        {
            var products = new[] { new Product() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryAllAsync<Product>(null)).ReturnsAsync(products);

            var result = await this.controller.QueryProductsAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, products);

            this.mockProcessor.Verify(c => c.QueryAllAsync<Product>(null), Times.Once());
        }

        /// <summary>
        /// Get all scenarios should invoke processor to return scenarios.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetAllScenarios_ShouldInvokeProcessor_ToReturnScenariosAsync()
        {
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

            this.mockProcessor.Setup(m => m.GetScenariosByRoleAsync(It.IsAny<IEnumerable<string>>())).ReturnsAsync(scenarios);

            var result = await this.controller.GetScenariosAsync().ConfigureAwait(false);
            var actionResult = result as EntityResult;
            var scenariosFromResult = actionResult.Value as List<Scenario>;
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GetScenariosByRoleAsync(It.IsAny<IEnumerable<string>>()), Times.Once());
            Assert.AreEqual(scenarios[0].ScenarioId, scenariosFromResult[0].ScenarioId);
            Assert.AreEqual(scenarios[0].Sequence, scenariosFromResult[0].Sequence);
            Assert.AreEqual(scenarios[0].Name, scenariosFromResult[0].Name);
            Assert.AreEqual(scenarios[0].Features, scenariosFromResult[0].Features);
        }

        /// <summary>
        /// Get all scenarios should invoke processor to return scenarios.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetAllScenarios_ShouldReturnNull_IfProcessorReturnsNullAsync()
        {
            List<Scenario> scenarios = null;
            this.mockProcessor.Setup(m => m.GetScenariosByRoleAsync(It.IsAny<IEnumerable<string>>())).ReturnsAsync(scenarios);

            var result = await this.controller.GetScenariosAsync().ConfigureAwait(false);
            var actionResult = result as EntityResult;
            var scenariosFromResult = actionResult.Value as List<Scenario>;
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GetScenariosByRoleAsync(It.IsAny<IEnumerable<string>>()), Times.Once());
            Assert.IsNull(scenariosFromResult);
        }

        /// <summary>
        /// Queries the algorithm list asynchronous should invoke processor to get all algorithms asynchronous.
        /// </summary>
        /// /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetAlgorithmListAsync_ShouldInvokeProcessor_ToGetAllAlgorithmsAsync()
        {
            var algorithms = new[] { new Algorithm() };
            this.mockProcessor.Setup(m => m.GetAlgorithmsAsync()).ReturnsAsync(algorithms);

            var result = await this.controller.GetAlgorithmsListAsync().ConfigureAwait(false);
            var actionResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            Assert.IsNotNull(actionResult, "Result should not be null");
            this.mockProcessor.Verify(m => m.GetAlgorithmsAsync(), Times.Once);
        }

        /// <summary>
        /// Queries the algorithm list asynchronous should invoke processor to get all algorithms asynchronous.
        /// </summary>
        /// /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetSystemTypesAsync_ShouldInvokeProcessor_ToGetAllSystemsAsync()
        {
            var systems = new[] { new SystemTypeEntity() };
            this.mockProcessor.Setup(m => m.GetSystemTypesAsync()).ReturnsAsync(systems);

            var result = await this.controller.QuerySystemTypesAsync().ConfigureAwait(false);
            var actionResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            Assert.IsNotNull(actionResult, "Result should not be null");
            this.mockProcessor.Verify(m => m.GetSystemTypesAsync(), Times.Once);
        }

        /// <summary>
        /// Queries the users list asynchronous should invoke processor to get all algorithms asynchronous.
        /// </summary>
        /// /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetUsersAsync_ShouldInvokeProcessor_ToGetAllUsersAsync()
        {
            var users = new[] { new User() };
            this.mockProcessor.Setup(m => m.GetUsersAsync()).ReturnsAsync(users);

            var result = await this.controller.GetUsersAsync().ConfigureAwait(false);
            var actionResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            Assert.IsNotNull(actionResult, "Result should not be null");
            this.mockProcessor.Verify(m => m.GetUsersAsync(), Times.Once);
        }

        /// <summary>
        /// Gets the variable types asynchronous should invoke processor to get all variable types asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetVariableTypesAsync_ShouldInvokeProcessor_ToGetAllVariableTypesAsync()
        {
            var variableTypes = new[] { new VariableTypeEntity() };
            this.mockProcessor.Setup(m => m.GetVariableTypesAsync()).ReturnsAsync(variableTypes);

            var result = await this.controller.GetVariableTypesAsync().ConfigureAwait(false);
            var actionResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            Assert.IsNotNull(actionResult, "Result should not be null");
            this.mockProcessor.Verify(m => m.GetVariableTypesAsync(), Times.Once);
        }

        /// <summary>
        /// Gets the icons asynchronous should return icons.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetIconsAsync_ShouldInvokeProcessor_ToReturnIconsAsync()
        {
            var icons = new[] { new Icon() }.AsQueryable();
            this.mockProcessor.Setup(m => m.GetIconsAsync()).ReturnsAsync(icons);

            var result = await this.controller.GetIconsAsync().ConfigureAwait(false);
            var actionResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            Assert.IsNotNull(actionResult, "Result should not be null");
            this.mockProcessor.Verify(c => c.GetIconsAsync(), Times.Once());
        }

        /// <summary>
        /// Gets the origin types asynchronous should return origin types.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetOriginTypesAsync_ShouldInvokeProcessor_ToReturnOriginTypesAsync()
        {
            var originTypes = new[] { new OriginType() }.AsQueryable();
            this.mockProcessor.Setup(m => m.GetOriginTypesAsync()).ReturnsAsync(originTypes);

            var result = await this.controller.GetOriginTypesAsync().ConfigureAwait(false);
            var actionResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            Assert.IsNotNull(actionResult, "Result should not be null");
            this.mockProcessor.Verify(c => c.GetOriginTypesAsync(), Times.Once());
        }
    }
}

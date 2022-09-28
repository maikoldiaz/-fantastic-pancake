// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateOfficialPointsAttributeTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Host.Sap.Api.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Host.Sap.Api.Filter;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Validate Official point Filter Attribute Tests.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Sap.Tests.ControllerTestBase" />
    [TestClass]
    public sealed class ValidateOfficialPointsAttributeTests : ControllerTestBase
    {
        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The resource provider mock.
        /// </summary>
        private Mock<IResourceProvider> resourceProviderMock;

        /// <summary>
        /// The resource provider mock.
        /// </summary>
        private Mock<IRepository<Movement>> movementRepoMock;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.SetupHttpContext();
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.resourceProviderMock = new Mock<IResourceProvider>();
            this.movementRepoMock = new Mock<IRepository<Movement>>();
            this.resourceProviderMock.Setup(r => r.GetResource(It.IsAny<string>())).Returns<string>(s => s);
        }

        /// <summary>
        /// Officials the movement validations null data exits when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_NullDataExits_WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var actionArguments = new Dictionary<string, object> { { "movements", null } };

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
            Assert.IsTrue(errorCodes.Any(x => x.Message == this.resourceProviderMock.Object.GetResource(Entities.Constants.NoRecordsFound)));
        }

        /// <summary>
        /// Officials the movement validations no data exits when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_NoDataExits_WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var actionArguments = new Dictionary<string, object> { { "movements", new List<SapMovement>() } };

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
            Assert.IsTrue(errorCodes.Any(x => x.Message == this.resourceProviderMock.Object.GetResource(Entities.Constants.NoRecordsFound)));
        }

        /// <summary>
        /// Officials the movement validations more than two records exits when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_MoreThanTwoRecordsExits_WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var actionArguments = new Dictionary<string, object> { { "movements", new List<SapMovement>() { new SapMovement(), new SapMovement(), new SapMovement() } } };

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
            Assert.IsTrue(errorCodes.Any(x => x.Message == this.resourceProviderMock.Object.GetResource(Entities.Constants.MoreThan2RecordsFound)));
        }

        /// <summary>
        /// Officials the movement validations official information not exits when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_OfficialInformationNotExits_WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var actionArguments = new Dictionary<string, object> { { "movements", new List<SapMovement>() { new SapMovement(), new SapMovement() } } };

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
            Assert.IsTrue(errorCodes.Any(x => x.Message == this.resourceProviderMock.Object.GetResource(Entities.Constants.OfficialInformationRequired)));
        }

        /// <summary>
        /// Officials the movement validations single movement is official when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_SingleMovementIsOfficial_WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var movements = new List<SapMovement>
            {
                new SapMovement
                {
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = false,
                        GlobalMovementId = "786",
                    },
                },
            };

            var actionArguments = new Dictionary<string, object> { { "movements", movements } };

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
            Assert.IsTrue(errorCodes.Any(x => x.Message == this.resourceProviderMock.Object.GetResource(Entities.Constants.SingleMovementOfficialPoint)));
        }

        /// <summary>
        /// Officials the movement validations single movement scenario single movement exist in database when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_SingleMovementScenario_SingleMovementExistInDb_WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var movements = new List<SapMovement>
            {
                new SapMovement
                {
                    MovementId = "123",
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = true,
                        GlobalMovementId = "786",
                    },
                },
            };

            var actionArguments = new Dictionary<string, object> { { "movements", movements } };
            this.movementRepoMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(() => null);
            this.mockFactory.Setup(x => x.CreateRepository<Movement>()).Returns(this.movementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
            Assert.IsTrue(errorCodes.Any(x => x.Message == this.resourceProviderMock.Object.GetResource(Entities.Constants.SingleMovementStored)));
        }

        /// <summary>
        /// Officials the movement validations single movement scenario no movement send to sap po when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_SingleMovementScenario_NoMovementSendToSapPO_WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var movements = new List<SapMovement>
            {
                new SapMovement
                {
                    MovementId = "123",
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = true,
                        GlobalMovementId = "786",
                    },
                    NetStandardVolume = 3,
                },
            };

            Movement movement = new Movement
            {
                MovementId = "123",
                MovementTransactionId = 123,
                NetStandardVolume = 2,
            };

            var actionArguments = new Dictionary<string, object> { { "movements", movements } };
            this.movementRepoMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement>() { movement });
            this.mockFactory.Setup(x => x.CreateRepository<Movement>()).Returns(this.movementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
            Assert.IsTrue(errorCodes.Any(x => x.Message == this.resourceProviderMock.Object.GetResource(Entities.Constants.SingleMovementNotSendToSap)));
        }

        /// <summary>
        /// Officials the movement validations single movement scenario single movement not match in database when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_SingleMovementScenario_SingleMovementNotMatchInDb_WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var movements = new List<SapMovement>
            {
                new SapMovement
                {
                    MovementId = "123",
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = true,
                        GlobalMovementId = "786",
                    },
                    NetStandardVolume = 3,
                },
            };

            Movement movement = new Movement
            {
                MovementId = "123",
                MovementTransactionId = 123,
                NetStandardVolume = 2,
            };

            SapTracking sapTracking = new SapTracking
            {
                MovementTransactionId = 123,
                SapTrackingId = (int)StatusType.PROCESSED,
                OperationalDate = DateTime.Now,
            };

            movement.SapTracking.Add(sapTracking);

            var actionArguments = new Dictionary<string, object> { { "movements", movements } };
            this.movementRepoMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement>() { movement });
            this.mockFactory.Setup(x => x.CreateRepository<Movement>()).Returns(this.movementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
            Assert.IsTrue(errorCodes.Any(x => x.Message == this.resourceProviderMock.Object.GetResource(Entities.Constants.SingleMovementDataNotMatchStored)));
        }

        /// <summary>
        /// Officials the movement validations multiple movement no official when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_MultipleMovementNoOfficial_WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var movements = new List<SapMovement>
            {
                new SapMovement
                {
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = false,
                    },
                },
                new SapMovement
                {
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = false,
                    },
                },
            };

            var actionArguments = new Dictionary<string, object> { { "movements", movements } };

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
            Assert.IsTrue(errorCodes.Any(x => x.Message == this.resourceProviderMock.Object.GetResource(Entities.Constants.OfficialPointError)));
        }

        /// <summary>
        /// Officials the movement validations multiple movement all official when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_MultipleMovementAllOfficial_WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var movements = new List<SapMovement>
            {
                new SapMovement
                {
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = true,
                    },
                },
                new SapMovement
                {
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = true,
                    },
                },
            };

            var actionArguments = new Dictionary<string, object> { { "movements", movements } };

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
            Assert.IsTrue(errorCodes.Any(x => x.Message == this.resourceProviderMock.Object.GetResource(Entities.Constants.OfficialPointError)));
        }

        /// <summary>
        /// Officials the movement validations multiple movement backup movement identifier is null for official when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_MultipleMovement_BackupMovementIdIsNullForOfficial_WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var movements = new List<SapMovement>
            {
                new SapMovement
                {
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = false,
                    },
                },
                new SapMovement
                {
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = true,
                    },
                },
            };

            var actionArguments = new Dictionary<string, object> { { "movements", movements } };

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
            Assert.IsTrue(errorCodes.Any(x => x.Message == this.resourceProviderMock.Object.GetResource(Entities.Constants.BackupMovementIdRequired)));
        }

        /// <summary>
        /// Officials the movement validations multiple movement different backup movement identifier for official when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_MultipleMovement_DifferentBackupMovementIdForOfficial_WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var movements = new List<SapMovement>
            {
                new SapMovement
                {
                    MovementId = "test1",
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = false,
                    },
                },
                new SapMovement
                {
                    MovementId = "test2",
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = true,
                        BackupMovementId = "456",
                    },
                },
            };

            var actionArguments = new Dictionary<string, object> { { "movements", movements } };

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
            Assert.IsTrue(errorCodes.Any(x => x.Message == this.resourceProviderMock.Object.GetResource(Entities.Constants.SameBackupMovementId)));
        }

        /// <summary>
        /// Officials the movement validations multiple movement different global movement identifier for official when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_MultipleMovement_DifferentGlobalMovementIdForOfficial_WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var movements = new List<SapMovement>
            {
                new SapMovement
                {
                    MovementId = "test1",
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = false,
                        GlobalMovementId = "567",
                    },
                },
                new SapMovement
                {
                    MovementId = "test2",
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = true,
                        BackupMovementId = "test1",
                        GlobalMovementId = "786",
                    },
                },
            };

            var actionArguments = new Dictionary<string, object> { { "movements", movements } };

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
            Assert.IsTrue(errorCodes.Any(x => x.Message == this.resourceProviderMock.Object.GetResource(Entities.Constants.SameGlobalMovementId)));
        }

        /// <summary>
        /// Officials the movement validations multiple movements scenario both movement not valid data when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_MultipleMovementsScenario_BothMovementNotValidData_WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var movements = new List<SapMovement>
            {
                new SapMovement
                {
                    MovementId = "test1",
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = false,
                        GlobalMovementId = "567",
                    },
                    MovementSource = new SapMovementSource
                    {
                        SourceNodeId = "1",
                        SourceProductId = "1",
                    },
                    MovementDestination = new SapMovementDestination
                    {
                        DestinationNodeId = "1",
                        DestinationProductId = "1",
                    },
                },
                new SapMovement
                {
                    MovementId = "test2",
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = true,
                        BackupMovementId = "test1",
                        GlobalMovementId = "567",
                    },
                    MovementSource = new SapMovementSource
                    {
                        SourceNodeId = "2",
                        SourceProductId = "1",
                    },
                    MovementDestination = new SapMovementDestination
                    {
                        DestinationNodeId = "2",
                        DestinationProductId = "1",
                    },
                },
            };

            var actionArguments = new Dictionary<string, object> { { "movements", movements } };
            this.movementRepoMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(() => null);
            this.mockFactory.Setup(x => x.CreateRepository<Movement>()).Returns(this.movementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
            Assert.IsTrue(errorCodes.Any(x => x.Message == this.resourceProviderMock.Object.GetResource(Entities.Constants.BothMovementDataNotvalid)));
        }

        /// <summary>
        /// Officials the movement validations multiple movements scenario single movement exist in database when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_MultipleMovementsScenario_SingleMovementExistInDb_WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var movements = new List<SapMovement>
            {
                new SapMovement
                {
                    MovementId = "test1",
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = false,
                        GlobalMovementId = "567",
                    },
                },
                new SapMovement
                {
                    MovementId = "test2",
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = true,
                        BackupMovementId = "test1",
                        GlobalMovementId = "567",
                    },
                },
            };

            var actionArguments = new Dictionary<string, object> { { "movements", movements } };
            this.movementRepoMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(() => null);
            this.mockFactory.Setup(x => x.CreateRepository<Movement>()).Returns(this.movementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
            Assert.IsTrue(errorCodes.Any(x => x.Message == this.resourceProviderMock.Object.GetResource(Entities.Constants.AtLeastOneMovementStored)));
        }

        /// <summary>
        /// Officials the movement validations multiple movements scenario no movement send to sap po when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_MultipleMovementsScenario_NoMovementSendToSapPO_WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var movements = new List<SapMovement>
            {
                new SapMovement
                {
                    MovementId = "test1",
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = false,
                        GlobalMovementId = "567",
                    },
                    MovementSource = new SapMovementSource
                    {
                        SourceNodeId = "1",
                        SourceProductId = "1",
                    },
                    MovementDestination = new SapMovementDestination
                    {
                        DestinationNodeId = "1",
                        DestinationProductId = "1",
                    },
                },
                new SapMovement
                {
                    MovementId = "test2",
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = true,
                        BackupMovementId = "test1",
                        GlobalMovementId = "567",
                    },
                    MovementSource = new SapMovementSource
                    {
                        SourceNodeId = "1",
                        SourceProductId = "1",
                    },
                    MovementDestination = new SapMovementDestination
                    {
                        DestinationNodeId = "1",
                        DestinationProductId = "1",
                    },
                },
            };

            Movement movement = new Movement
            {
                MovementId = "test2",
                MovementTransactionId = 123,
            };

            var actionArguments = new Dictionary<string, object> { { "movements", movements } };
            this.movementRepoMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement>() { movement });
            this.mockFactory.Setup(x => x.CreateRepository<Movement>()).Returns(this.movementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
            Assert.IsTrue(errorCodes.Any(x => x.Message == this.resourceProviderMock.Object.GetResource(Entities.Constants.MultipleMovementNotSendToSap)));
        }

        /// <summary>
        /// Officials the movement validations multiple movements scenario no movement valid data in true when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_MultipleMovementsScenario_NoMovementValidDataInTrue_WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var movements = new List<SapMovement>
            {
                new SapMovement
                {
                    MovementId = "test1",
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = false,
                        GlobalMovementId = "567",
                    },
                    MovementSource = new SapMovementSource
                    {
                        SourceNodeId = "1",
                        SourceProductId = "1",
                    },
                    MovementDestination = new SapMovementDestination
                    {
                        DestinationNodeId = "1",
                        DestinationProductId = "1",
                    },
                },
                new SapMovement
                {
                    MovementId = "test2",
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = true,
                        BackupMovementId = "test1",
                        GlobalMovementId = "567",
                    },
                    MovementSource = new SapMovementSource
                    {
                        SourceNodeId = "1",
                        SourceProductId = "1",
                    },
                    MovementDestination = new SapMovementDestination
                    {
                        DestinationNodeId = "1",
                        DestinationProductId = "1",
                    },
                },
            };

            Movement movement = new Movement
            {
                MovementId = "test2",
                MovementTransactionId = 123,
            };

            SapTracking sapTracking = new SapTracking
            {
                MovementTransactionId = 123,
                SapTrackingId = (int)StatusType.PROCESSED,
                OperationalDate = DateTime.Now,
            };

            movement.SapTracking.Add(sapTracking);

            var actionArguments = new Dictionary<string, object> { { "movements", movements } };
            this.movementRepoMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement>() { movement });
            this.mockFactory.Setup(x => x.CreateRepository<Movement>()).Returns(this.movementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
            Assert.IsTrue(errorCodes.Any(x => x.Message == this.resourceProviderMock.Object.GetResource(Entities.Constants.MultipleMovementDataNotMatchStored)));
        }

        /// <summary>
        /// Officials the movement validations invalid entity when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_InvalidEntityRaisedCatchException_WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var actionArguments = new Dictionary<string, object> { { "movements", new List<Movement>() } };

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
            Assert.IsTrue(errorCodes.Any(x => x.Message == this.resourceProviderMock.Object.GetResource(Entities.Constants.MovementInvalidDataType)));
        }

        /// <summary>
        /// Officials the movement validations single movement scenario sucess case when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_SingleMovementScenario_SucessCase_WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var movements = new List<SapMovement>
            {
                new SapMovement
                {
                    MovementId = "123",
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = true,
                        GlobalMovementId = "786",
                    },
                    NetStandardVolume = 2,
                },
            };

            Movement movement = new Movement
            {
                MovementId = "123",
                MovementTransactionId = 123,
                NetStandardVolume = 2,
            };

            SapTracking sapTracking = new SapTracking
            {
                MovementTransactionId = 123,
                SapTrackingId = (int)StatusType.PROCESSED,
                OperationalDate = DateTime.Now,
            };

            movement.SapTracking.Add(sapTracking);

            var actionArguments = new Dictionary<string, object> { { "movements", movements } };
            this.movementRepoMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement>() { movement });
            this.mockFactory.Setup(x => x.CreateRepository<Movement>()).Returns(this.movementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            var next = new ActionExecutionDelegate(() => Task.FromResult(CreateActionExecutedContext(actionExecutingContext)));
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            Assert.IsNull(result);
        }

        /// <summary>
        /// Officials the movement validations multiple movement scenario sucess case when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialMovementValidations_MultipleMovementScenario_SucessCase__WhenInvokedAsync()
        {
            var attribute = new ValidateOfficialPointsAttribute("movements");
            var movements = new List<SapMovement>
            {
                new SapMovement
                {
                    MovementId = "test1",
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = false,
                        GlobalMovementId = "567",
                    },
                    MovementDestination = new SapMovementDestination
                    {
                        DestinationNodeId = "1",
                        DestinationProductId = "1",
                    },
                },
                new SapMovement
                {
                    MovementId = "test2",
                    NetStandardVolume = 1,
                    BackupMovement = new BackupMovement
                    {
                        IsOfficial = true,
                        BackupMovementId = "test1",
                        GlobalMovementId = "567",
                    },
                    MovementDestination = new SapMovementDestination
                    {
                        DestinationNodeId = "1",
                        DestinationProductId = "1",
                    },
                },
            };

            Movement movement = new Movement
            {
                MovementId = "test2",
                MovementTransactionId = 123,
                NetStandardVolume = 1,
            };

            MovementDestination movementDestination = new MovementDestination
            {
                DestinationNodeId = 1,
                DestinationProductId = "1",
            };

            SapTracking sapTracking = new SapTracking
            {
                MovementTransactionId = 123,
                SapTrackingId = (int)StatusType.PROCESSED,
                OperationalDate = DateTime.Now,
            };

            movement.SapTracking.Add(sapTracking);
            movement.MovementDestination = movementDestination;

            var actionArguments = new Dictionary<string, object> { { "movements", movements } };
            this.movementRepoMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement>() { movement });
            this.mockFactory.Setup(x => x.CreateRepository<Movement>()).Returns(this.movementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            var next = new ActionExecutionDelegate(() => Task.FromResult(CreateActionExecutedContext(actionExecutingContext)));
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            Assert.IsNull(result);
        }

        private static ActionExecutedContext CreateActionExecutedContext(ActionExecutingContext context)
        {
            return new ActionExecutedContext(context, context.Filters, context.Controller)
            {
                Result = context.Result,
            };
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateSapRequestFilterAttributeTests.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core.Entities;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.Sap.Purchases;
    using Ecp.True.Host.Sap.Api.Filter;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Validate File Registration Transaction Filter Attribute Tests.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Sap.Api.Tests.ControllerTestBase" />
    [TestClass]
    public sealed class ValidateSapRequestFilterAttributeTests : ControllerTestBase
    {
        /// <summary>
        /// The service provider mock.
        /// </summary>
        private Mock<IServiceProvider> serviceProviderMock;

        /// <summary>
        /// The configuration handler mock.
        /// </summary>
        private Mock<IConfigurationHandler> configurationHandlerMockSapConfig;

        /// <summary>
        /// The sap config.
        /// </summary>
        private SapSettings systemConfig;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.SetupHttpContext();
            this.serviceProviderMock = new Mock<IServiceProvider>();
            this.systemConfig = new SapSettings { SapRecordsMaxLimit = 2000 };

            this.configurationHandlerMockSapConfig = new Mock<IConfigurationHandler>();
            this.configurationHandlerMockSapConfig.Setup(m => m.GetConfigurationAsync<SapSettings>(ConfigurationConstants.SapSettings)).ReturnsAsync(this.systemConfig);
            this.serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IConfigurationHandler)))).Returns(this.configurationHandlerMockSapConfig.Object);
        }

        /// <summary>
        /// Validates the sap request if movements count is zero when invoked asynchronous.
        /// </summary>
        /// <returns>Returns a task.</returns>
        [TestMethod]
        public async Task ValidateSapRequestFilterAttribute_IfMovementsCount_Is_Zero_WhenInvokedAsync()
        {
            var attribute = new ValidateSapRequestFilterAttribute("movements");
            var actionArguments = new Dictionary<string, object> { { "movements", new List<SapMovement>() } };

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = this.serviceProviderMock.Object;
            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(CreateActionExecutedContext(actionExecutingContext))).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequestObjectResult);
            Assert.IsNotNull(errorResponse);
            Assert.IsNotNull(errorCodes);
            Assert.AreEqual(1, errorCodes.Count());
            Assert.AreEqual("NoRecordsFound", errorCodes.ElementAt(0).Message);
        }

        /// <summary>
        /// Validates the sap request if inventories count is zero when invoked asynchronous.
        /// </summary>
        /// <returns>Returns a task.</returns>
        [TestMethod]
        public async Task ValidateSapRequestFilterAttribute_IfInventoriesCount_Is_Zero_WhenInvokedAsync()
        {
            var attribute = new ValidateSapRequestFilterAttribute("inventories");
            var actionArguments = new Dictionary<string, object> { { "inventories", new List<SapInventory>() } };

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = this.serviceProviderMock.Object;
            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(CreateActionExecutedContext(actionExecutingContext))).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequestObjectResult);
            Assert.IsNotNull(errorResponse);
            Assert.IsNotNull(errorCodes);
            Assert.AreEqual(1, errorCodes.Count());
            Assert.AreEqual("NoRecordsFound", errorCodes.ElementAt(0).Message);
        }

        /// <summary>
        /// Validates the sap request if movements count is more than max limit when invoked asynchronous.
        /// </summary>
        /// <returns>Returns a task.</returns>
        [TestMethod]
        public async Task ValidateSapRequestFilterAttribute_IfMovementsCount_Is_MoreThanMaxLimit_WhenInvokedAsync()
        {
            var attribute = new ValidateSapRequestFilterAttribute("movements");
            var actionArguments = new Dictionary<string, object> { { "movements", new List<SapMovement> { new SapMovement(), new SapMovement() } } };
            this.systemConfig.SapRecordsMaxLimit = 1;
            this.configurationHandlerMockSapConfig.Setup(m => m.GetConfigurationAsync<SapSettings>(ConfigurationConstants.SapSettings)).ReturnsAsync(this.systemConfig);
            this.serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IConfigurationHandler)))).Returns(this.configurationHandlerMockSapConfig.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = this.serviceProviderMock.Object;
            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(CreateActionExecutedContext(actionExecutingContext))).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequestObjectResult);
            Assert.IsNotNull(errorResponse);
            Assert.IsNotNull(errorCodes);
            Assert.AreEqual(1, errorCodes.Count());
            Assert.AreEqual("Solo se admiten hasta 1 registros por llamada", errorCodes.ElementAt(0).Message);
        }

        /// <summary>
        /// Validates the sap request if inventories count is more than max limit when invoked asynchronous.
        /// </summary>
        /// <returns>Returns a task.</returns>
        [TestMethod]
        public async Task ValidateSapRequestFilterAttribute_IfInventoriesCount_Is_MoreThanMaxLimit_WhenInvokedAsync()
        {
            var attribute = new ValidateSapRequestFilterAttribute("inventories");
            var actionArguments = new Dictionary<string, object> { { "inventories", new List<SapInventory> { new SapInventory(), new SapInventory() } } };
            this.systemConfig.SapRecordsMaxLimit = 1;
            this.configurationHandlerMockSapConfig.Setup(m => m.GetConfigurationAsync<SapSettings>(ConfigurationConstants.SapSettings)).ReturnsAsync(this.systemConfig);
            this.serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IConfigurationHandler)))).Returns(this.configurationHandlerMockSapConfig.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = this.serviceProviderMock.Object;
            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(CreateActionExecutedContext(actionExecutingContext))).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequestObjectResult);
            Assert.IsNotNull(errorResponse);
            Assert.IsNotNull(errorCodes);
            Assert.AreEqual(1, errorCodes.Count());
            Assert.AreEqual("Solo se admiten hasta 1 registros por llamada", errorCodes.ElementAt(0).Message);
        }

        /// <summary>
        /// Validates the sap request if inventories count is less than max limit when invoked asynchronous.
        /// </summary>
        /// <returns>Returns a task.</returns>
        [TestMethod]
        public async Task ValidateSapRequestFilterAttribute_IfInventoriesCount_Is_LessThanMaxLimit_WhenInvokedAsync()
        {
            var attribute = new ValidateSapRequestFilterAttribute("inventories");
            var actionArguments = new Dictionary<string, object> { { "inventories", new List<SapInventory> { new SapInventory(), new SapInventory() } } };
            this.systemConfig.SapRecordsMaxLimit = null;
            this.configurationHandlerMockSapConfig.Setup(m => m.GetConfigurationAsync<SapSettings>(ConfigurationConstants.SapSettings)).ReturnsAsync(this.systemConfig);
            this.serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IConfigurationHandler)))).Returns(this.configurationHandlerMockSapConfig.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = this.serviceProviderMock.Object;
            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(CreateActionExecutedContext(actionExecutingContext))).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            Assert.IsNull(result);
        }

        /// <summary>
        /// Validates the sap request if movements count is more than max limit when invoked asynchronous.
        /// </summary>
        /// <returns>Returns a task.</returns>
        [TestMethod]
        public async Task ValidateSapRequestFilterAttribute_IfSalesCount_Is_MoreThanMaxLimit_WhenInvokedAsync()
        {
            var attribute = new ValidateSapRequestFilterAttribute("sales");
            var actionArguments = new Dictionary<string, object> { { "sales", new Sale { OrderSale = new OrderSale { ControlData = new ControlData { EventSapPo = "CREAR" }, Header = new Header { DateOrder = DateTime.Now }, PositionObject = new PositionObject { Positions = new List<Position>() { new Position(), new Position(), new Position() } } } } } };
            this.systemConfig.SalesPositionsMaxLimit = 2;
            this.configurationHandlerMockSapConfig.Setup(m => m.GetConfigurationAsync<SapSettings>(ConfigurationConstants.SapSettings)).ReturnsAsync(this.systemConfig);
            this.serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IConfigurationHandler)))).Returns(this.configurationHandlerMockSapConfig.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = this.serviceProviderMock.Object;
            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(CreateActionExecutedContext(actionExecutingContext))).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var payloadToLargeObjectResult = result as ObjectResult;
            var payLoadRequestObject = payloadToLargeObjectResult.Value;
            var errorResponse = payLoadRequestObject as List<ErrorResponse>;
            var errorCodes = errorResponse.FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.IsNotNull(payloadToLargeObjectResult);
            Assert.IsNotNull(errorResponse);
            Assert.IsNotNull(errorCodes);
        }

        /// <summary>
        /// Validates the sap request if movements count is more than max limit when invoked asynchronous.
        /// </summary>
        /// <returns>Returns a task.</returns>
        [TestMethod]
        public async Task ValidateSapRequestFilterAttribute_SalesData_WhenInvokedAsync()
        {
            var attribute = new ValidateSapRequestFilterAttribute("sales");
            var sale = new Sale
            {
                OrderSale = new OrderSale
                {
                    ControlData = new ControlData
                    {
                        EventSapPo = "CREAR",
                        DateReceivedPo = DateTime.Now,
                        DestinationSystem = "TRUE_000000012121212212121",
                        MessageId = "1364531",
                        SourceSystem = "CMDCLNT1300345000000000001",
                    },
                    Header = new Header
                    {
                        DateOrder = DateTime.Now,
                        TypeOrder = "ZUT102",
                        OrganizationId = "124512364313143131321321145643531311351354",
                        DescriptionStatus = "1458232265695634694695695",
                    },
                    PositionObject = new PositionObject
                    {
                        Positions = new List<Position>()
                            {
                                new Position
                                {
                                    Batch = "12355131",
                                    DestinationLocationId = "lo64354dqeq43584354354e3q5w4e3q54we35q4w3e54c1",
                                    DestinationStorageLocationId = "loc52",
                                    EndTime = DateTime.Now,
                                    Frequency = "Mensual",
                                    Key = "464321321",
                                    Material = "00000046446545645454545656546454645444646544564625",
                                    PositionId = 1,
                                    PositionStatus = "X",
                                    Quantity = "126456465464646546546546513216840",
                                    QuantityUom = "Bbls",
                                    RejectionReason = "N/A",
                                    StartTime = DateTime.Now.AddDays(20),
                                },
                            },
                    },
                },
            };
            var actionArguments = new Dictionary<string, object> { { "sales", sale } };
            this.systemConfig.SalesPositionsMaxLimit = 5;
            this.configurationHandlerMockSapConfig.Setup(m => m.GetConfigurationAsync<SapSettings>(ConfigurationConstants.SapSettings)).ReturnsAsync(this.systemConfig);
            this.serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IConfigurationHandler)))).Returns(this.configurationHandlerMockSapConfig.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = this.serviceProviderMock.Object;
            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(CreateActionExecutedContext(actionExecutingContext))).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var payloadToLargeObjectResult = result as ObjectResult;
            var payLoadRequestObject = payloadToLargeObjectResult.Value;
            var errorResponse = payLoadRequestObject as List<ErrorResponse>;
            var errorCodes = errorResponse.FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.IsNotNull(payloadToLargeObjectResult);
            Assert.IsNotNull(errorResponse);
            Assert.IsNotNull(errorCodes);
        }

        /// <summary>
        /// Validates the sap request if movements count is more than max limit when invoked asynchronous.
        /// </summary>
        /// <returns>Returns a task.</returns>
        [TestMethod]
        public async Task ValidateSapRequestFilterAttribute_Sales_PositionEmpty_WhenInvokedAsync()
        {
            var attribute = new ValidateSapRequestFilterAttribute("sales");
            var sale = new Sale
            {
                OrderSale = new OrderSale
                {
                    ControlData = new ControlData
                    {
                        EventSapPo = "CREAR",
                        DateReceivedPo = DateTime.Now,
                        DestinationSystem = "TRUE_000000012121212212121",
                        MessageId = "1364531",
                        SourceSystem = "CMDCLNT1300345000000000001",
                    },
                    Header = new Header
                    {
                        DateOrder = DateTime.Now,
                        TypeOrder = "ZUT102",
                        OrganizationId = "124512364313143131321321145643531311351354",
                        DescriptionStatus = "1458232265695634694695695",
                    },
                    PositionObject = new PositionObject
                    {
                        Positions = new List<Position>(),
                    },
                },
            };
            var actionArguments = new Dictionary<string, object> { { "sales", sale } };
            this.systemConfig.SalesPositionsMaxLimit = 5;
            this.configurationHandlerMockSapConfig.Setup(m => m.GetConfigurationAsync<SapSettings>(ConfigurationConstants.SapSettings)).ReturnsAsync(this.systemConfig);
            this.serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IConfigurationHandler)))).Returns(this.configurationHandlerMockSapConfig.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = this.serviceProviderMock.Object;
            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(CreateActionExecutedContext(actionExecutingContext))).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var payloadToLargeObjectResult = result as ObjectResult;
            var payLoadRequestObject = payloadToLargeObjectResult.Value;
            var errorResponse = payLoadRequestObject as List<ErrorResponse>;
            var errorCodes = errorResponse.FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.IsNotNull(payloadToLargeObjectResult);
            Assert.IsNotNull(errorResponse);
            Assert.IsNotNull(errorCodes);
        }

        /// <summary>
        /// Validates the sap request if movements count is more than max limit when invoked asynchronous.
        /// </summary>
        /// <returns>Returns a task.</returns>
        [TestMethod]
        public async Task ValidateSapRequestFilterAttribute_SalesDeleteData_WhenInvokedAsync()
        {
            var attribute = new ValidateSapRequestFilterAttribute("sales");
            var sale = new Sale
            {
                OrderSale = new OrderSale
                {
                    ControlData = new ControlData
                    {
                        EventSapPo = "MODIFICAR",
                        DateReceivedPo = DateTime.Now,
                        DestinationSystem = "TRUE",
                        MessageId = "1364531",
                        SourceSystem = "CMDCLNT130",
                    },
                    Header = new Header
                    {
                        DateOrder = DateTime.Now,
                    },
                    PositionObject = new PositionObject
                    {
                        Positions = new List<Position>()
                            {
                                new Position
                                {
                                    Batch = "12355131",
                                    DestinationLocationId = "loc1",
                                    DestinationStorageLocationId = "loc2",
                                    EndTime = DateTime.Now,
                                    Frequency = "Mensual",
                                    Key = "464321321",
                                    Material = "0000004625",
                                    PositionId = 1,
                                    PositionStatus = "X",
                                    Quantity = "120",
                                    QuantityUom = "Bbl",
                                    RejectionReason = "N/A",
                                    StartTime = DateTime.Now.AddDays(20),
                                },
                            },
                    },
                },
            };
            var actionArguments = new Dictionary<string, object> { { "sales", sale } };
            this.systemConfig.SalesPositionsMaxLimit = 5;
            this.configurationHandlerMockSapConfig.Setup(m => m.GetConfigurationAsync<SapSettings>(ConfigurationConstants.SapSettings)).ReturnsAsync(this.systemConfig);
            this.serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IConfigurationHandler)))).Returns(this.configurationHandlerMockSapConfig.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = this.serviceProviderMock.Object;
            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(CreateActionExecutedContext(actionExecutingContext))).ConfigureAwait(false);

            var result = actionExecutingContext.Result;

            Assert.IsNull(result);
        }

        /// <summary>
        /// Validates the sap request if purchases items count is more than max limit when invoked asynchronous.
        /// </summary>
        /// <returns>Returns a task.</returns>
        [TestMethod]
        public async Task ValidateSapRequestFilterAttribute_IfPurchasesCount_Is_MoreThanMaxLimit_WhenInvokedAsync()
        {
            var attribute = new ValidateSapRequestFilterAttribute("purchase");
            var actionArguments = new Dictionary<string, object> { { "purchase", new SapPurchase { PurchaseOrder = new SapPurchaseOrder() { PurchaseOrder = new SapOrder() { PurchaseItem = new SapPurchaseItem() { PurchaseItem = new List<SapItem>() { new SapItem(), new SapItem(), new SapItem() } } } } } } };
            this.systemConfig.PurchasesPositionsMaxLimit = 2;
            this.configurationHandlerMockSapConfig.Setup(m => m.GetConfigurationAsync<SapSettings>(ConfigurationConstants.SapSettings)).ReturnsAsync(this.systemConfig);
            this.serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IConfigurationHandler)))).Returns(this.configurationHandlerMockSapConfig.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = this.serviceProviderMock.Object;
            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(CreateActionExecutedContext(actionExecutingContext))).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var payloadToLargeObjectResult = result as ObjectResult;
            var payLoadRequestObject = payloadToLargeObjectResult.Value;
            var errorResponse = payLoadRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result);
            Assert.IsNotNull(payloadToLargeObjectResult);
            Assert.IsNotNull(errorResponse);
            Assert.IsNotNull(errorCodes);
            Assert.AreEqual(1, errorCodes.Count());
            Assert.AreEqual("Solo se admiten hasta 2 registros por llamada", errorCodes.ElementAt(0).Message);
        }

        /// <summary>
        /// Validates the sap request if purchases items count is more than max limit when invoked asynchronous.
        /// </summary>
        /// <returns>Returns a task.</returns>
        [TestMethod]
        public async Task ValidateSapRequestFilterAttribute_PurchasesItemsIsEmpty_WhenInvokedAsync()
        {
            var attribute = new ValidateSapRequestFilterAttribute("purchase");
            var purchaseData = new SapPurchase
            {
                PurchaseOrder = new SapPurchaseOrder
                {
                    PurchaseOrder = new SapOrder
                    {
                        PurchaseItem = new SapPurchaseItem
                        {
                            PurchaseItem = new List<SapItem>(),
                        },
                    },
                },
            };
            var actionArguments = new Dictionary<string, object> { { "purchase", purchaseData } };
            this.systemConfig.PurchasesPositionsMaxLimit = 2;
            this.configurationHandlerMockSapConfig.Setup(m => m.GetConfigurationAsync<SapSettings>(ConfigurationConstants.SapSettings)).ReturnsAsync(this.systemConfig);
            this.serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IConfigurationHandler)))).Returns(this.configurationHandlerMockSapConfig.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = this.serviceProviderMock.Object;
            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(CreateActionExecutedContext(actionExecutingContext))).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var payloadToLargeObjectResult = result as ObjectResult;
            var payLoadRequestObject = payloadToLargeObjectResult.Value;
            var errorResponse = payLoadRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result);
            Assert.IsNotNull(payloadToLargeObjectResult);
            Assert.IsNotNull(errorResponse);
            Assert.IsNotNull(errorCodes);
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

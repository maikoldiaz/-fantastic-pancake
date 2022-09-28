// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractControllerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Sap.Api.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.Sap.Purchases;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Host.Sap.Api.Controllers;
    using Ecp.True.Processors.Transform.Input.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The sales controller tests.
    /// </summary>
    [TestClass]
    public class ContractControllerTests
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private ContractController controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<IInputFactory> mockProcessor;

        /// <summary>
        /// The mock business context.
        /// </summary>
        private Mock<IBusinessContext> mockBusinessContext;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockProcessor = new Mock<IInputFactory>();
            this.mockBusinessContext = new Mock<IBusinessContext>();

            this.mockBusinessContext.Setup(m => m.ActivityId).Returns(Guid.NewGuid());
            this.controller = new ContractController(this.mockBusinessContext.Object, this.mockProcessor.Object);
        }

        /// <summary>
        /// Create Sale asynchronous should invoke processor.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task CreateSaleAsync_ShouldInvokeProcessor_ToCreateSaleAsync()
        {
            var sale = new Sale
            {
                OrderSale = new OrderSale
                {
                    Header = new Header(),
                    ControlData = new ControlData(),
                    PositionObject = new PositionObject
                    {
                        Positions = new List<Position>(),
                    },
                },
            };

            this.mockProcessor.Setup(p => p.SaveSapJsonAsync(It.IsAny<object>(), It.IsAny<TrueMessage>()));

            var result = await this.controller.CreateSaleAsync(sale).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.SaveSapJsonAsync(It.IsAny<object>(), It.Is<TrueMessage>(p => p.Message == Entities.Core.MessageType.Sale && !p.ShouldHomologate)), Times.Once());
        }

        /// <summary>
        /// Updates the purchases asynchronous should invoke processor.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task CreatePurchasesAsync_ShouldInvokeProcessor_ToCreatePurchasesAsync()
        {
            var purchase = new SapPurchase();

            this.mockProcessor.Setup(p => p.SaveSapJsonAsync(It.IsAny<object>(), It.IsAny<TrueMessage>()));

            var result = await this.controller.CreatePurchasesAsync(purchase).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.SaveSapJsonAsync(It.IsAny<object>(), It.Is<TrueMessage>(p => p.Message == Entities.Core.MessageType.Purchase && !p.ShouldHomologate)), Times.Once());
        }

        /// <summary>
        /// Testh method for property invalid.
        /// </summary>
        [TestMethod]
        public void ValidateCriterion_FieldProperty()
        {
            var criterion = new SapCriterion()
            {
                Value = 101,
                Property = "Compra-Otro",
                Uom = "$",
            };

            var validationContext = new ValidationContext(criterion);

            var results = criterion.Validate(validationContext);

            Assert.IsInstanceOfType(results, typeof(IEnumerable<ValidationResult>));
        }

        /// <summary>
        /// Testh method for value and uom invalid.
        /// </summary>
        [TestMethod]
        public void ValidateCriterion_FieldsValueAndUom()
        {
            var criterion = new SapCriterion()
            {
                Value = 101,
                Property = SapConstants.PropertyPurchasePercentage,
                Uom = "$",
            };

            var validationContext = new ValidationContext(criterion);

            var results = criterion.Validate(validationContext);

            Assert.IsInstanceOfType(results, typeof(IEnumerable<ValidationResult>));
        }

        /// <summary>
        /// Testh method for value and uom when value is valid.
        /// </summary>
        [TestMethod]
        public void ValidateCriterion_FieldsValueAndUom_ValueValid()
        {
            var criterion = new SapCriterion()
            {
                Value = -1,
                Property = SapConstants.PropertyPurchasePercentage,
                Uom = "$",
            };

            var validationContext = new ValidationContext(criterion);

            var results = criterion.Validate(validationContext);

            Assert.IsInstanceOfType(results, typeof(IEnumerable<ValidationResult>));
        }

        /// <summary>
        /// Testh method for status inactive.
        /// </summary>
        [TestMethod]
        public void ValidateOrder_FieldStatus_Invalid()
        {
            var order = new SapOrder()
            {
                Status = "Inactiva",
            };

            var validationContext = new ValidationContext(order);

            var results = order.Validate(validationContext);

            Assert.IsInstanceOfType(results, typeof(IEnumerable<ValidationResult>));
        }

        /// <summary>
        /// Testh method for status active.
        /// </summary>
        [TestMethod]
        public void ValidateOrder_FieldStatus_Active()
        {
            var order = new SapOrder()
            {
                Status = SapConstants.StatusActive,
                Category = null,
                DateOrder = null,
                Other = null,
                Provider = null,
                PurchaseItem = null,
                PurchaseOrderId = null,
                PurchaseOrderType = null,
                SupplyCenter = null,
                Society = null,
                SourceLocation = null,
            };

            var validationContext = new ValidationContext(order);

            var results = order.Validate(validationContext);

            Assert.IsInstanceOfType(results, typeof(IEnumerable<ValidationResult>));
        }

        /// <summary>
        /// Testh method for end period greater than start period.
        /// </summary>
        [TestMethod]
        public void ValidatePurchasesPeriod_EndPeriodGreaterThanStartPeriod()
        {
            var period = new Entities.Sap.Purchases.SapPeriod()
            {
                StartPeriod = DateTime.Now,
                EndPeriod = DateTime.Now.AddDays(-1),
            };

            var validationContext = new ValidationContext(period);

            var results = period.Validate(validationContext);

            Assert.IsInstanceOfType(results, typeof(IEnumerable<ValidationResult>));
        }

        /// <summary>
        /// Test method for position status in sales when the value is null.
        /// </summary>
        [TestMethod]
        public void ValidateSalesPositionStatus_WhenIsNull()
        {
            var position = new Position()
            {
                PositionStatus = null,
                Frequency = string.Empty,
            };

            var validationContext = new ValidationContext(position);

            var result = position.Validate(validationContext);

            Assert.AreEqual(0, result.Count());
        }

        /// <summary>
        /// Test method for position status in sales when the value is valid.
        /// </summary>
        [TestMethod]
        public void ValidateSalesPositionStatus_WhenIs_Valid()
        {
            var position = new Position()
            {
                PositionStatus = "X",
                Frequency = string.Empty,
            };

            var validationContext = new ValidationContext(position);

            var result = position.Validate(validationContext);

            Assert.AreEqual(0, result.Count());
        }

        /// <summary>
        /// Test method for position status in sales when the value is empty.
        /// </summary>
        [TestMethod]
        public void ValidateSalesPositionStatus_WhenIs_Empty()
        {
            var position = new Position()
            {
                PositionStatus = string.Empty,
                Frequency = string.Empty,
            };

            var validationContext = new ValidationContext(position);

            var result = position.Validate(validationContext);

            Assert.AreEqual(0, result.Count());
        }

        /// <summary>
        /// Test method for position status in sales when the value is invalid.
        /// </summary>
        [TestMethod]
        public void ValidateSalesPositionStatus_WhenIs_Invalid()
        {
            var position = new Position()
            {
                PositionStatus = "T",
                Frequency = string.Empty,
            };

            var validationContext = new ValidationContext(position);

            var result = position.Validate(validationContext);

            Assert.AreEqual(1, result.Count());
        }

        /// <summary>
        /// Test method for position status in sales when the value is null.
        /// </summary>
        [TestMethod]
        public void ValidatePurchasePositionStatus_WhenIsNull()
        {
            var position = new SapItem()
            {
                PositionStatus = null,
            };

            var validationContext = new ValidationContext(position);

            var result = position.Validate(validationContext);

            Assert.AreEqual(0, result.Count());
        }

        /// <summary>
        /// Test method for position status in sales when the value is valid.
        /// </summary>
        [TestMethod]
        public void ValidatePurchasePositionStatus_WhenIs_Valid()
        {
            var position = new SapItem()
            {
                PositionStatus = "L",
            };

            var validationContext = new ValidationContext(position);

            var result = position.Validate(validationContext);

            Assert.AreEqual(0, result.Count());
        }

        /// <summary>
        /// Test method for position status in sales when the value is empty.
        /// </summary>
        [TestMethod]
        public void ValidatePurchasePositionStatus_WhenIs_Empty()
        {
            var position = new SapItem()
            {
                PositionStatus = string.Empty,
            };

            var validationContext = new ValidationContext(position);

            var result = position.Validate(validationContext);

            Assert.AreEqual(0, result.Count());
        }

        /// <summary>
        /// Test method for position status in sales when the value is invalid.
        /// </summary>
        [TestMethod]
        public void ValidatePurchasePositionStatus_WhenIs_Invalid()
        {
            var position = new SapItem()
            {
                PositionStatus = "T",
            };

            var validationContext = new ValidationContext(position);

            var result = position.Validate(validationContext);

            Assert.AreEqual(1, result.Count());
        }

        /// <summary>
        /// Testh method for end period greater than start period.
        /// </summary>
        [TestMethod]
        public void ValidateSalesPeriod_EndPeriodGreaterThanStartPeriod()
        {
            var position = new Position()
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(-1),
            };

            var validationContext = new ValidationContext(position);

            var results = position.Validate(validationContext);

            Assert.IsInstanceOfType(results, typeof(IEnumerable<ValidationResult>));
        }

        /// <summary>
        /// Testh method for status inactive.
        /// </summary>
        [TestMethod]
        public void ValidatePurchaseOrder_FieldEvent()
        {
            var purchaseOrder = new SapPurchaseOrder()
            {
                Event = "Event",
                Date = null,
                MessageId = null,
                PurchaseOrder = null,
                SourceSystem = null,
            };

            var validationContext = new ValidationContext(purchaseOrder);

            var results = purchaseOrder.Validate(validationContext);

            Assert.IsInstanceOfType(results, typeof(IEnumerable<ValidationResult>));
        }

        /// <summary>
        /// Testh method for frequency invalid.
        /// </summary>
        [TestMethod]
        public void ValidatePurchaseItem_FieldFrequency_WhenIsInvalid()
        {
            var item = new SapItem()
            {
                Id = 1,
                Commodity = null,
                Criterion = null,
                EstimatedVolume = null,
                ExpeditionClass = null,
                Facilities = null,
                Location = null,
                Period = null,
                PositionStatus = null,
                Tolerance = 1,
                Frequency = "Bimestral",
            };

            var validationContext = new ValidationContext(item);

            var results = item.Validate(validationContext);

            Assert.IsInstanceOfType(results, typeof(IEnumerable<ValidationResult>));
        }
    }
}

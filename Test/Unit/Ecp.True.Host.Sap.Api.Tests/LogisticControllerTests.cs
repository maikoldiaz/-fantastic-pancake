// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticControllerTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Host.Sap.Api.Controllers;
    using Ecp.True.Processors.Transform.Input.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The sales controller tests.
    /// </summary>
    [TestClass]
    public class LogisticControllerTests
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private LogisticController controller;

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
            this.controller = new LogisticController(this.mockBusinessContext.Object, this.mockProcessor.Object);
        }

        /// <summary>
        /// Process a logistic movement asynchronous should invoke processor.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProcessLogisticMovementResponseAsync_ShouldInvokeProcessor_ToProcessMovementAsync()
        {
            var logisticMovement = new LogisticMovementResponse { MovementId = "123", };

            this.mockProcessor.Setup(p => p.SaveSapLogisticJsonAsync(It.IsAny<object>(), It.IsAny<TrueMessage>()));

            var result = await this.controller.ProcessLogisticMovementResponseAsync(logisticMovement).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.SaveSapLogisticJsonAsync(It.IsAny<object>(), It.Is<TrueMessage>(p => p.Message == Entities.Core.MessageType.Logistic && !p.ShouldHomologate)), Times.Once());
        }

        /// <summary>
        /// Test method for property invalid.
        /// </summary>
        [TestMethod]
        public void ValidateStatus_WhenErrorAndInformation_IsNull()
        {
            var logisticMovement = new LogisticMovementResponse()
            {
                StatusMessage = "E",
                Information = null,
            };

            var validationContext = new ValidationContext(logisticMovement);

            var results = logisticMovement.Validate(validationContext);

            Assert.IsInstanceOfType(results, typeof(IEnumerable<ValidationResult>));
        }

        /// <summary>
        /// Test method for property invalid.
        /// </summary>
        [TestMethod]
        public void ValidateStatus_WhenErrorAndInformation_HasInformation()
        {
            var logisticMovement = new LogisticMovementResponse()
            {
                StatusMessage = "E",
                Information = "Error",
            };

            var validationContext = new ValidationContext(logisticMovement);

            var results = logisticMovement.Validate(validationContext);

            Assert.AreEqual(0, results.Count());
        }

        /// <summary>
        /// Test method for property invalid.
        /// </summary>
        [TestMethod]
        public void ValidateStatus_WhenSuccessAndTransactionId_IsNull()
        {
            var logisticMovement = new LogisticMovementResponse()
            {
                StatusMessage = "S",
                TransactionId = null,
            };

            var validationContext = new ValidationContext(logisticMovement);

            var results = logisticMovement.Validate(validationContext);

            Assert.IsInstanceOfType(results, typeof(IEnumerable<ValidationResult>));
        }

        /// <summary>
        /// Test method for property invalid.
        /// </summary>
        [TestMethod]
        public void ValidateStatus_WhenSuccessAndTransactionId_HasInformation()
        {
            var logisticMovement = new LogisticMovementResponse()
            {
                StatusMessage = "S",
                TransactionId = "321645",
            };

            var validationContext = new ValidationContext(logisticMovement);

            var results = logisticMovement.Validate(validationContext);

            Assert.AreEqual(0, results.Count());
        }

        /// <summary>
        /// Test method for property invalid.
        /// </summary>
        [TestMethod]
        public void ValidateStatus_WhenSuccessAndTransactionId_hasMaxLength()
        {
            var logisticMovement = new LogisticMovementResponse()
            {
                StatusMessage = "S",
                TransactionId = "321645898988",
            };

            var validationContext = new ValidationContext(logisticMovement);

            var results = logisticMovement.Validate(validationContext);

            Assert.AreEqual(1, results.Count());
        }

        /// <summary>
        /// Create Sale asynchronous should invoke processor.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task CreateLogisticResponseAsync_ShouldInvokeProcessorAsync()
        {
            var logisticMovement = new LogisticMovementResponse
            {
                DateReceivedSystem = DateTime.Now,
                DestinationSystem = "TRUE",
                IdMessage = "123545",
                Information = "asd",
                MovementId = "MOV_001",
                SourceSystem = "CMDCLNT130",
                StatusMessage = "S",
                TransactionId = "125",
            };

            this.mockProcessor.Setup(p => p.SaveSapLogisticJsonAsync(It.IsAny<object>(), It.IsAny<TrueMessage>()));

            var result = await this.controller.ProcessLogisticMovementResponseAsync(logisticMovement).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.SaveSapLogisticJsonAsync(It.IsAny<object>(), It.Is<TrueMessage>(p => p.Message == Entities.Core.MessageType.Logistic)), Times.Once());
        }
    }
}

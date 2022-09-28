// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InterfaceMovementGeneratorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Balance.Tests.Operation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance.Calculation.Output;
    using Ecp.True.Processors.Balance.Operation;
    using Ecp.True.Processors.Balance.Operation.Input;
    using Ecp.True.Processors.Balance.Operation.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;

    /// <summary>
    /// The interface movement generator tests.
    /// </summary>
    [TestClass]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Test Class")]
    public class InterfaceMovementGeneratorTests
    {
        /// <summary>
        /// The interface movement generator factory.
        /// </summary>
        private Mock<IMovementGenerator> movementGenerator;

        /// <summary>
        /// The balance tolerance movement generator.
        /// </summary>
        private InterfaceMovementGenerator interfaceMovementGenerator;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<InterfaceMovementGenerator>> mockLogger;

        /// <summary>
        /// The Movement Input.
        /// </summary>
        private MovementInput movementInputObject;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<InterfaceMovementGenerator>>();
            this.movementGenerator = new Mock<IMovementGenerator>();
            this.interfaceMovementGenerator = new InterfaceMovementGenerator(this.mockLogger.Object);
        }

        /// <summary>
        /// Unidentified Loss MovementGenerator.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task BalanceToleranceMovementGenerator_ShouldGenerateMovement_Scenario4_WithSuccessAsync()
        {
            var ticket = new Ticket { TicketId = 23678, CategoryElementId = 2, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 16), Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };
            var interfaces = JsonConvert.DeserializeObject<IEnumerable<InterfaceInfo>>(File.ReadAllText("CalculatorJson/Interface_Scenario4.json"));
            this.movementInputObject = new MovementInput(interfaces, ticket, new DateTime(2019, 02, 12));

            // Act
            var result = await this.interfaceMovementGenerator.GenerateAsync(this.movementInputObject).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unidentified Loss MovementGenerator.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task BalanceToleranceMovementGenerator_ShouldGenerateMovement_Scenario5_WithSuccessAsync()
        {
            var ticket = new Ticket { TicketId = 23678, CategoryElementId = 2, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 16), Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };
            var interfaces = JsonConvert.DeserializeObject<IEnumerable<InterfaceInfo>>(File.ReadAllText("CalculatorJson/Interface_Scenario5.json"));
            this.movementInputObject = new MovementInput(interfaces, ticket, new DateTime(2019, 02, 12));

            // Act
            var result = await this.interfaceMovementGenerator.GenerateAsync(this.movementInputObject).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Unidentified Loss MovementGenerator.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task BalanceToleranceMovementGenerator_ShouldGenerateMovement_WithErrorAsync()
        {
            var ticket = new Ticket { TicketId = 23678, CategoryElementId = 2, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 16), Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };
            var interfaces = JsonConvert.DeserializeObject<IEnumerable<InterfaceInfo>>(File.ReadAllText("CalculatorJson/Interface_SumNotZero.json"));
            this.movementInputObject = new MovementInput(interfaces, ticket, new DateTime(2019, 02, 12));

            // Act
            var result = await this.interfaceMovementGenerator.GenerateAsync(this.movementInputObject).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsNull(result);
        }
    }
}

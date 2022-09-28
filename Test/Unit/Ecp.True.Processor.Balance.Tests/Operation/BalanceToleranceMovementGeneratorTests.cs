// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalanceToleranceMovementGeneratorTests.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
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
    /// The balance tolerance movement generator tests.
    /// </summary>
    [TestClass]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Test Class")]
    public class BalanceToleranceMovementGeneratorTests
    {
        /// <summary>
        /// The interface movement generator factory.
        /// </summary>
        private Mock<IMovementGenerator> movementGenerator;

        /// <summary>
        /// The balance tolerance movement generator.
        /// </summary>
        private BalanceToleranceMovementGenerator balanceToleranceMovementGenerator;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<BalanceToleranceMovementGenerator>> mockLogger;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private Mock<IUnitOfWork> unitOfWorkMock;

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
            this.mockLogger = new Mock<ITrueLogger<BalanceToleranceMovementGenerator>>();
            this.movementGenerator = new Mock<IMovementGenerator>();
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            var movementRepository = new Mock<IMovementRepository>();
            this.unitOfWorkMock.Setup(a => a.MovementRepository).Returns(movementRepository.Object);
            this.balanceToleranceMovementGenerator = new BalanceToleranceMovementGenerator(this.mockLogger.Object);
            this.unitOfWorkMock.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            var ticket = new Ticket { TicketId = 23678, CategoryElementId = 2, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 16), Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };
            var balanceTolerances = JsonConvert.DeserializeObject<IEnumerable<BalanceTolerance>>(File.ReadAllText("CalculatorJson/BalanceTolerance.json"));
            this.movementInputObject = new MovementInput(balanceTolerances, ticket, new DateTime(2019, 02, 12));
        }

        /// <summary>
        /// Unidentified Loss MovementGenerator.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task BalanceToleranceMovementGenerator_ShouldGenerateMovement_WithSuccessAsync()
        {
            // Act
            var result = await this.balanceToleranceMovementGenerator.GenerateAsync(this.movementInputObject).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ToList().Count);
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalanceToleranceCalculatorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Balance.Tests.Calculation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance.Calculation;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Ecp.True.Processors.Balance.Calculation.Interfaces;
    using Ecp.True.Processors.Balance.Calculation.Output;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;

    /// <summary>
    /// The balance intolerance calculator tests.
    /// </summary>
    [TestClass]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Test Class")]
    public class BalanceToleranceCalculatorTests
    {
        /// <summary>
        /// The logger mock.
        /// </summary>
        private readonly Mock<ITrueLogger<BalanceToleranceCalculator>> loggerMock = new Mock<ITrueLogger<BalanceToleranceCalculator>>();

        /// <summary>
        /// The unbalances.
        /// </summary>
        private IEnumerable<UnbalanceComment> unbalances;

        /// <summary>
        /// The repository factory.
        /// </summary>
        private Mock<IUnbalanceCalculator> unbalanceCalculator;

        /// <summary>
        /// The balance tolerance calculator.
        /// </summary>
        private BalanceToleranceCalculator balanceToleranceCalculator;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.unbalances = JsonConvert.DeserializeObject<IEnumerable<UnbalanceComment>>(File.ReadAllText("UnbalanceJson/UnbalancesExcelValues.json"));

            this.unbalanceCalculator = new Mock<IUnbalanceCalculator>();
            this.unbalanceCalculator.Setup(x => x.CalculateAsync(It.IsAny<CalculationInput>())).Returns(Task.FromResult(this.unbalances));

            this.balanceToleranceCalculator = new BalanceToleranceCalculator(this.unbalanceCalculator.Object, this.loggerMock.Object);
        }

        /// <summary>
        /// ticket processor should calculate asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task BalanceIntoleranceCalculator_ShouldCalculate_WithSuccessAsync()
        {
            var calculationDate = DateTime.UtcNow.Date;
            var calculationInput = new CalculationInput
            {
                Node = new NodeInput
                {
                    NodeId = 100,
                    Name = "AYACUCHO CRD-GALAN 14",
                    ControlLimit = Convert.ToDecimal(1.96),
                    CalculationDate = calculationDate,
                },
                TicketId = 1,
                Movements = new List<MovementCalculationInput>
                {
                    new MovementCalculationInput
                    {
                    MovementId = "100",
                    SourceNodeId = 300,
                    SourceProductId = "1",
                    DestinationNodeId = 100,
                    DestinationProductId = "1",
                    NetStandardVolume = Convert.ToDecimal(442558.55),
                    OperationalDate = calculationDate,
                    UncertaintyPercentage = 1,
                    },
                    new MovementCalculationInput
                    {
                    MovementId = "105",
                    SourceNodeId = 300,
                    SourceProductId = "1",
                    DestinationNodeId = 100,
                    DestinationProductId = "1",
                    NetStandardVolume = Convert.ToDecimal(432000),
                    OperationalDate = calculationDate,
                    UncertaintyPercentage = 2,
                    },
                },
                InitialInventories = new List<InventoryInput>
                {
                    new InventoryInput
                    {
                        InventoryDate = calculationDate,
                        NodeId = 100,
                        UncertaintyPercentage = 1,
                        ProductVolume = 10,
                    },
                    new InventoryInput
                    {
                        InventoryDate = calculationDate,
                        NodeId = 2,
                        UncertaintyPercentage = 2,
                        ProductVolume = 20,
                    },
                },
                FinalInventories = new List<InventoryInput>
                {
                    new InventoryInput
                    {
                        InventoryDate = calculationDate.AddDays(1),
                        NodeId = 100,
                        UncertaintyPercentage = 1,
                        ProductVolume = 10,
                    },
                    new InventoryInput
                    {
                        InventoryDate = calculationDate.AddDays(1),
                        NodeId = 2,
                        UncertaintyPercentage = 2,
                        ProductVolume = 20,
                    },
                },
                CalculationDate = calculationDate,
            };

            var result = await this.balanceToleranceCalculator.CalculateAsync(calculationInput).ConfigureAwait(false);
            var balanceTolerance = result as IEnumerable<BalanceTolerance>;

            // Assert or Verify
            Assert.IsNotNull(balanceTolerance);
            this.unbalanceCalculator.Verify(x => x.CalculateAsync(It.IsAny<CalculationInput>()), Times.Once);
            Assert.IsNotNull(balanceTolerance.ElementAtOrDefault(0).NodeId);
            Assert.IsNotNull(balanceTolerance.ElementAtOrDefault(1).NodeId);
            Assert.IsNotNull(balanceTolerance.ElementAtOrDefault(0).ProductId);
            Assert.IsNotNull(balanceTolerance.ElementAtOrDefault(1).ProductId);
            Assert.IsNotNull(balanceTolerance.ElementAtOrDefault(0).IdentifiedLosses);
            Assert.IsNotNull(balanceTolerance.ElementAtOrDefault(1).IdentifiedLosses);
            Assert.AreEqual(new decimal(874558.55), balanceTolerance.ElementAtOrDefault(0).Inputs);
            Assert.AreEqual(new decimal(667475.99), balanceTolerance.ElementAtOrDefault(1).Inputs);
            Assert.AreEqual(new decimal(9.89), balanceTolerance.ElementAtOrDefault(0).IdentifiedLosses);
            Assert.AreEqual(new decimal(5620.15), balanceTolerance.ElementAtOrDefault(1).IdentifiedLosses);
            Assert.AreEqual(Math.Round(new decimal(1418.15), 2, MidpointRounding.AwayFromZero), Math.Round(balanceTolerance.ElementAtOrDefault(0).Tolerance, 2, MidpointRounding.AwayFromZero));
            Assert.AreEqual(Math.Round(new decimal(1312.17), 2, MidpointRounding.AwayFromZero), Math.Round(balanceTolerance.ElementAtOrDefault(1).Tolerance, 2, MidpointRounding.AwayFromZero));
        }
    }
}

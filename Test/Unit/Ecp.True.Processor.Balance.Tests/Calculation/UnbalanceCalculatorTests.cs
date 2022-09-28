// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnbalanceCalculatorTests.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance.Calculation;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The unbalance calculator tests.
    /// </summary>
    [TestClass]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Test Class")]
    public class UnbalanceCalculatorTests
    {
        /// <summary>
        /// The input.
        /// </summary>
        private CalculationInput input;

        /// <summary>
        /// The repository factory.
        /// </summary>
        private Mock<IConfigurationHandler> configurationHandlerMock;

        /// <summary>
        /// The logger mock.
        /// </summary>
        private Mock<ITrueLogger<UnbalanceCalculator>> loggerMock;

        /// <summary>
        /// The interface calculator.
        /// </summary>
        private UnbalanceCalculator unbalanceCalculator;

        /// <summary>
        /// unbalance calculator should calculate unbalance asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task UnbalanceCalculator_ShouldCalculateAsync_WithSuccessAsync()
        {
            this.input = new CalculationInput
            {
                Node = new NodeInput
                {
                    NodeId = 100,
                    Name = "AYACUCHO CRD-GALAN 14",
                    ControlLimit = Convert.ToDecimal(1.96),
                },
                TicketId = 1,
                CalculationDate = DateTime.Now,
                InitialInventories = new List<InventoryInput>
                {
                    new InventoryInput
                    {
                       NodeId = 1,
                       ProductId = "1",
                       MeasurementUnit = "Bbl",
                       ProductName = "Test 4",
                       ProductVolume = 8,
                       UncertaintyPercentage = 9,
                       InventoryId = "1",
                       InventoryDate = DateTime.Now.AddDays(-1).Date,
                    },
                },
                FinalInventories = new List<InventoryInput>
                {
                    new InventoryInput
                    {
                       NodeId = 1,
                       ProductId = "1",
                       MeasurementUnit = "Bbl",
                       ProductName = "Test 4",
                       ProductVolume = 8,
                       UncertaintyPercentage = 9,
                       InventoryId = "1",
                       InventoryDate = DateTime.Now.Date,
                    },
                },
                Movements = new List<MovementCalculationInput>
                {
                    new MovementCalculationInput
                    {
                       MovementId = "1",
                       MeasurementUnit = "9",
                       DestinationNodeId = 1,
                       DestinationProductId = "2",
                       DestinationProductName = "Test 2",
                       MessageTypeId = 1,
                       NetStandardVolume = 10,
                       SourceNodeId = 2,
                       SourceProductId = "3",
                       SourceProductName = "Test 3",
                       OperationalDate = DateTime.Now,
                       UncertaintyPercentage = 9,
                    },
                },
            };
            this.input.ProductsInput.Add("1", "Test 4");
            this.input.ProductsInput.Add("2", "Test 2");
            this.input.ProductsInput.Add("3", "Test 3");

            this.configurationHandlerMock = new Mock<IConfigurationHandler>();
            this.loggerMock = new Mock<ITrueLogger<UnbalanceCalculator>>();
            var systemConfig = new SystemSettings
            {
                StandardUncertaintyPercentage = 0.2M,
                ControlLimit = 1.98M,
            };

            this.configurationHandlerMock.Setup(x => x.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);
            this.unbalanceCalculator = new UnbalanceCalculator(this.configurationHandlerMock.Object, this.loggerMock.Object);

            // Act
            var unbalances = await this.unbalanceCalculator.CalculateAsync(this.input).ConfigureAwait(false);

            // Assert or Verify
            Assert.AreEqual(100, unbalances.ElementAtOrDefault(0).NodeId);
            Assert.AreEqual(100, unbalances.ElementAtOrDefault(1).NodeId);
            Assert.AreEqual(100, unbalances.ElementAtOrDefault(2).NodeId);
            Assert.AreEqual("1", unbalances.ElementAtOrDefault(0).ProductId);
            Assert.AreEqual("2", unbalances.ElementAtOrDefault(1).ProductId);
            Assert.AreEqual("3", unbalances.ElementAtOrDefault(2).ProductId);
            Assert.AreEqual(new decimal(0), unbalances.ElementAtOrDefault(0).Unbalance);
            Assert.AreEqual(new decimal(0), unbalances.ElementAtOrDefault(1).Unbalance);
            Assert.AreEqual("31", unbalances.ElementAtOrDefault(0).Units);
            Assert.AreEqual("31", unbalances.ElementAtOrDefault(1).Units);
            Assert.AreEqual(new decimal(1.96), unbalances.ElementAtOrDefault(0).ControlLimit);
            Assert.AreEqual(new decimal(1.96), unbalances.ElementAtOrDefault(1).ControlLimit);
            Assert.AreEqual(new decimal(0), unbalances.ElementAtOrDefault(0).Inputs);
            Assert.AreEqual(new decimal(0), unbalances.ElementAtOrDefault(1).Inputs);
        }
    }
}

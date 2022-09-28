// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InterfaceCalculatorTests.cs" company="Microsoft">
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
    /// The interface calculator tests.
    /// </summary>
    [TestClass]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Test Class")]
    public class InterfaceCalculatorTests
    {
        /// <summary>
        /// The input.
        /// </summary>
        private CalculationInput input;

        /// <summary>
        /// The repository factory.
        /// </summary>
        private Mock<IUnbalanceCalculator> unbalanceCalculator;

        /// <summary>
        /// The logger mock.
        /// </summary>
        private Mock<ITrueLogger<InterfaceCalculator>> loggerMock;

        /// <summary>
        /// The interface calculator.
        /// </summary>
        private InterfaceCalculator interfaceCalculator;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
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
            };

            var unbalances = JsonConvert.DeserializeObject<IEnumerable<UnbalanceComment>>(File.ReadAllText("UnbalanceJson/Unbalances.json"));

            this.unbalanceCalculator = new Mock<IUnbalanceCalculator>();
            this.loggerMock = new Mock<ITrueLogger<InterfaceCalculator>>();
            this.unbalanceCalculator.Setup(x => x.CalculateAsync(It.IsAny<CalculationInput>())).Returns(Task.FromResult(unbalances));

            this.interfaceCalculator = new InterfaceCalculator(this.unbalanceCalculator.Object, this.loggerMock.Object);
        }

        /// <summary>
        /// ticket processor should calculate asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task InterfaceCalculator_ShouldCalculate_WithSuccessAsync()
        {
            // Act
            var interfaceInfos = (IEnumerable<InterfaceInfo>)await this.interfaceCalculator.CalculateAsync(this.input).ConfigureAwait(false);

            // Assert or Verify
            this.unbalanceCalculator.Verify(x => x.CalculateAsync(this.input), Times.Once);
            Assert.IsNotNull(interfaceInfos.ElementAtOrDefault(0).NodeId);
            Assert.IsNotNull(interfaceInfos.ElementAtOrDefault(1).NodeId);
            Assert.IsNotNull(interfaceInfos.ElementAtOrDefault(0).ProductId);
            Assert.IsNotNull(interfaceInfos.ElementAtOrDefault(1).ProductId);
            Assert.AreEqual(new decimal(874558.55), interfaceInfos.ElementAtOrDefault(0).Inputs);
            Assert.AreEqual(new decimal(667475.99), interfaceInfos.ElementAtOrDefault(1).Inputs);
            Assert.AreEqual(new decimal(1418.15), interfaceInfos.ElementAtOrDefault(0).Unbalance);
            Assert.AreEqual(new decimal(1312.17), interfaceInfos.ElementAtOrDefault(1).Unbalance);
            Assert.AreEqual(0, interfaceInfos.Sum(x => x.Interface));
            Assert.AreEqual(Math.Round(new decimal(-130.3397635535453051525032635), 2, MidpointRounding.AwayFromZero), Math.Round(interfaceInfos.ElementAtOrDefault(0).Interface, 2, MidpointRounding.AwayFromZero));
            Assert.AreEqual(Math.Round(new decimal(130.3397635535453051525032635), 2, MidpointRounding.AwayFromZero), Math.Round(interfaceInfos.ElementAtOrDefault(1).Interface, 2, MidpointRounding.AwayFromZero));
        }
    }
}

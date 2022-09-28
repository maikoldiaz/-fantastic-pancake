// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnIdentifiedLossCalculatorTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance.Calculation;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Ecp.True.Processors.Balance.Calculation.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;

    /// <summary>
    /// The unidentified loss calculator tests.
    /// </summary>
    [TestClass]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Test Class")]
    public class UnIdentifiedLossCalculatorTests
    {
        /// <summary>
        /// The logger mock.
        /// </summary>
        private readonly Mock<ITrueLogger<UnIdentifiedLossCalculator>> loggerMock = new Mock<ITrueLogger<UnIdentifiedLossCalculator>>();

        /// <summary>
        /// The unbalances.
        /// </summary>
        private IEnumerable<UnbalanceComment> unbalances;

        /// <summary>
        /// The repository factory.
        /// </summary>
        private Mock<IUnbalanceCalculator> unbalanceCalculator;

        /// <summary>
        /// The unidentified loss calculator.
        /// </summary>
        private UnIdentifiedLossCalculator unidentifiedLossCalculator;

        /// <summary>
        /// ticket processor should calculate asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task UnIdentifiedLossCalculator_ShouldCalculate_WithSuccessAsync()
        {
            this.unbalances = JsonConvert.DeserializeObject<IEnumerable<UnbalanceComment>>(File.ReadAllText("UnbalanceJson/UnBalalanceInput.json"));

            this.unbalanceCalculator = new Mock<IUnbalanceCalculator>();
            this.unbalanceCalculator.Setup(x => x.CalculateAsync(It.IsAny<CalculationInput>())).ReturnsAsync(this.unbalances);

            this.unidentifiedLossCalculator = new UnIdentifiedLossCalculator(this.unbalanceCalculator.Object, this.loggerMock.Object);

            // Act
            await this.unidentifiedLossCalculator.CalculateAsync(new CalculationInput { TicketId = 1 }).ConfigureAwait(false);

            // Assert or Verify
            this.unbalanceCalculator.Verify(x => x.CalculateAsync(It.IsAny<CalculationInput>()), Times.Once);
        }

        /// <summary>
        /// Uns the identified loss calculator should calculate if inputs aand initial inventory are with success asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task UnIdentifiedLossCalculator_ShouldCalculateIfInputsAandInitialInventoryAre_WithSuccessAsync()
        {
            this.unbalances = JsonConvert.DeserializeObject<IEnumerable<UnbalanceComment>>(File.ReadAllText("UnbalanceJson/UnBalalanceInputIdendifiedLoss.json"));

            this.unbalanceCalculator = new Mock<IUnbalanceCalculator>();
            this.unbalanceCalculator.Setup(x => x.CalculateAsync(It.IsAny<CalculationInput>())).ReturnsAsync(this.unbalances);

            this.unidentifiedLossCalculator = new UnIdentifiedLossCalculator(this.unbalanceCalculator.Object, this.loggerMock.Object);

            // Act
            await this.unidentifiedLossCalculator.CalculateAsync(new CalculationInput { TicketId = 1 }).ConfigureAwait(false);

            // Assert or Verify
            this.unbalanceCalculator.Verify(x => x.CalculateAsync(It.IsAny<CalculationInput>()), Times.Once);
        }
    }
}

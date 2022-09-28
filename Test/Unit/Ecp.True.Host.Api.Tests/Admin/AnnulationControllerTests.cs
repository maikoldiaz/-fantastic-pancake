// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnnulationControllerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests.Admin
{
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The reversal controller tests.
    /// </summary>
    [TestClass]
    public class AnnulationControllerTests
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private AnnulationController controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<IAnnulationProcessor> mockProcessor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockProcessor = new Mock<IAnnulationProcessor>();
            this.controller = new AnnulationController(this.mockProcessor.Object);
        }

        /// <summary>
        /// Gets the annulation asynchronous should return reversal.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task QueryAnnulationsAsync_ShouldInvokeProcessor_ToReturnReversalAsync()
        {
            var reversals = new[] { new Annulation() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryAllAsync<Annulation>(null)).ReturnsAsync(reversals);

            var result = await this.controller.QueryAnnulationsAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, reversals);

            this.mockProcessor.Verify(c => c.QueryAllAsync<Annulation>(null), Times.Once());
        }

        /// <summary>
        /// Creates the annulation asynchronous should invoke processor to create reversal.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task CreateAnnulationRelationshipAsync_ShouldInvokeProcessor_ToCreateReversalAsync()
        {
            var reversal = new Annulation();
            this.mockProcessor.Setup(m => m.CreateAnnulationRelationshipAsync(It.IsAny<Annulation>()));

            var result = await this.controller.CreateAnnulationRelationshipAsync(reversal).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.CreateAnnulationRelationshipAsync(reversal), Times.Once());
        }

        /// <summary>
        /// Updates the annulation relationship asynchronous should invoke processor to create reversal asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateAnnulationRelationshipAsync_ShouldInvokeProcessor_ToCreateReversalAsync()
        {
            var annulation = new Annulation();
            this.mockProcessor.Setup(m => m.UpdateAnnulationRelationshipAsync(It.IsAny<Annulation>()));

            var result = await this.controller.UpdateAnnulationRelationshipAsync(annulation).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.UpdateAnnulationRelationshipAsync(annulation), Times.Once());
        }

        /// <summary>
        /// Checks if annulation exists asynchronous should invoke processor to create reversal.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task ExistsAnnulationsRelationshipAsync_ShouldInvokeProcessor_ToReversalRelationshipExistsAsync()
        {
            var reversal = new Annulation();
            this.mockProcessor.Setup(m => m.ExistsAnnulationRelationshipAsync(It.IsAny<Annulation>()));

            var result = await this.controller.ExistsAnnulationsRelationshipAsync(reversal).ConfigureAwait(false);

            Assert.IsNotNull(result, "Result should not be null");
            this.mockProcessor.Verify(c => c.ExistsAnnulationRelationshipAsync(reversal), Times.Once());
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationStrategyFactoryTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Core.Tests
{
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Core.Registration;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The EventRegistrationStrategyTests.
    /// </summary>
    [TestClass]
    public class RegistrationStrategyFactoryTests
    {
        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<RegistrationStrategyFactory>> mockLogger;

        /// <summary>
        /// The mock movement registration service.
        /// </summary>
        private Mock<IMovementRegistrationService> mockMovementRegistrationService;

        /// <summary>
        /// The registration strategy factory.
        /// </summary>
        private RegistrationStrategyFactory registrationStrategyFactory;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<RegistrationStrategyFactory>>();
            this.mockMovementRegistrationService = new Mock<IMovementRegistrationService>();

            this.registrationStrategyFactory =
                new RegistrationStrategyFactory(
                    this.mockLogger.Object,
                    this.mockAzureClientFactory.Object,
                    this.mockMovementRegistrationService.Object);
        }

        /// <summary>
        /// Registrations the strategy factory should return ownership registration strategy.
        /// </summary>
        [TestMethod]
        public void RegistrationStrategyFactory_ShouldReturn_OwnershipRegistrationStrategy()
        {
            Assert.IsNotNull(this.registrationStrategyFactory.OwnershipRegistrationStrategy);
        }

        /// <summary>
        /// Registrations the strategy factory should return movement registration strategy.
        /// </summary>
        [TestMethod]
        public void RegistrationStrategyFactory_ShouldReturn_MovementRegistrationStrategy()
        {
            Assert.IsNotNull(this.registrationStrategyFactory.MovementRegistrationStrategy);
        }

        /// <summary>
        /// Registrations the strategy factory should return inventory product registration strategy.
        /// </summary>
        [TestMethod]
        public void RegistrationStrategyFactory_ShouldReturn_InventoryProductRegistrationStrategy()
        {
            Assert.IsNotNull(this.registrationStrategyFactory.InventoryProductRegistrationStrategy);
        }

        /// <summary>
        /// Registrations the strategy factory should return event registration strategy.
        /// </summary>
        [TestMethod]
        public void RegistrationStrategyFactory_ShouldReturn_EventRegistrationStrategy()
        {
            Assert.IsNotNull(this.registrationStrategyFactory.EventRegistrationStrategy);
        }

        /// <summary>
        /// Registrations the strategy factory should return contract registration strategy.
        /// </summary>
        [TestMethod]
        public void RegistrationStrategyFactory_ShouldReturn_ContractRegistrationStrategy()
        {
            Assert.IsNotNull(this.registrationStrategyFactory.ContractRegistrationStrategy);
        }
    }
}

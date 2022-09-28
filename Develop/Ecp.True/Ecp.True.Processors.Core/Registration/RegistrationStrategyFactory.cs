// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationStrategyFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Registration
{
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// The registration strategy factory.
    /// </summary>
    public class RegistrationStrategyFactory : IRegistrationStrategyFactory
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<RegistrationStrategyFactory> logger;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The movement registration service.
        /// </summary>
        private readonly IMovementRegistrationService movementRegistrationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationStrategyFactory" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="movementRegistrationService">The movement registration service.</param>
        public RegistrationStrategyFactory(
            ITrueLogger<RegistrationStrategyFactory> logger,
            IAzureClientFactory azureClientFactory,
            IMovementRegistrationService movementRegistrationService)
        {
            this.logger = logger;
            this.azureClientFactory = azureClientFactory;
            this.movementRegistrationService = movementRegistrationService;
        }

        /// <inheritdoc/>
        public IRegistrationStrategy OwnershipRegistrationStrategy => new OwnershipRegistrationStrategy(
            this.logger,
            this.azureClientFactory);

        /// <summary>
        /// Gets the movement registration strategy.
        /// </summary>
        /// <value>
        /// The movement registration strategy.
        /// </value>
        public IRegistrationStrategy MovementRegistrationStrategy =>
            new MovementRegistrationStrategy(
                this.logger,
                this.azureClientFactory,
                this.movementRegistrationService);

        /// <summary>
        /// Gets the inventory product registration strategy.
        /// </summary>
        /// <value>
        /// The inventory product registration strategy.
        /// </value>
        public IRegistrationStrategy InventoryProductRegistrationStrategy =>
            new InventoryProductRegistrationStrategy(
                this.logger,
                this.azureClientFactory);

        /// <summary>
        /// Gets the event registration strategy.
        /// </summary>
        /// <value>
        /// The event registration strategy.
        /// </value>
        public IRegistrationStrategy EventRegistrationStrategy =>
            new EventRegistrationStrategy(
                this.logger,
                this.azureClientFactory);

        /// <summary>
        /// Gets the contract registration strategy.
        /// </summary>
        /// <value>
        /// The contract registration strategy.
        /// </value>
        public IRegistrationStrategy ContractRegistrationStrategy =>
            new ContractRegistrationStrategy(
                this.logger,
                this.azureClientFactory);
    }
}

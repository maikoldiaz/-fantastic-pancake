// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositeValidatorFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Registration.Validation
{
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Registration.Interfaces;

    /// <summary>
    /// The composite validator factory.
    /// </summary>
    public class CompositeValidatorFactory : ICompositeValidatorFactory
    {
        /// <summary>
        /// The inventory validator.
        /// </summary>
        private readonly ICompositeValidator<InventoryProduct> inventoryCompositeValidator;

        /// <summary>
        /// The movement validator.
        /// </summary>
        private readonly ICompositeValidator<Movement> movementCompositeValidator;

        /// <summary>
        /// The event validator.
        /// </summary>
        private readonly ICompositeValidator<Event> eventCompositeValidator;

        /// <summary>
        /// The contract validator.
        /// </summary>
        private readonly ICompositeValidator<Contract> contractCompositeValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidatorFactory" /> class.
        /// </summary>
        /// <param name="inventoryCompositeValidator">The inventory validator.</param>
        /// <param name="movementCompositeValidator">The movement validator.</param>
        /// <param name="eventCompositeValidator">The event validator.</param>
        /// <param name="contractCompositeValidator">The contract validator.</param>
        public CompositeValidatorFactory(
            ICompositeValidator<InventoryProduct> inventoryCompositeValidator,
            ICompositeValidator<Movement> movementCompositeValidator,
            ICompositeValidator<Event> eventCompositeValidator,
            ICompositeValidator<Contract> contractCompositeValidator)
        {
            this.inventoryCompositeValidator = inventoryCompositeValidator;
            this.movementCompositeValidator = movementCompositeValidator;
            this.eventCompositeValidator = eventCompositeValidator;
            this.contractCompositeValidator = contractCompositeValidator;
        }

        /// <inheritdoc/>
        public ICompositeValidator<InventoryProduct> InventoryCompositeValidator => this.inventoryCompositeValidator;

        /// <inheritdoc/>
        public ICompositeValidator<Movement> MovementCompositeValidator => this.movementCompositeValidator;

        /// <inheritdoc/>
        public ICompositeValidator<Event> EventCompositeValidator => this.eventCompositeValidator;

        /// <inheritdoc/>
        public ICompositeValidator<Contract> ContractCompositeValidator => this.contractCompositeValidator;
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICompositeValidatorFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Registration.Interfaces
{
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The composite validator factory.
    /// </summary>
    public interface ICompositeValidatorFactory
    {
        /// <summary>
        /// Gets the inventory composite validator.
        /// </summary>
        /// <value>
        /// The inventory composite validator.
        /// </value>
        ICompositeValidator<InventoryProduct> InventoryCompositeValidator { get; }

        /// <summary>
        /// Gets the movement composite validator.
        /// </summary>
        /// <value>
        /// The movement composite validator.
        /// </value>
        ICompositeValidator<Movement> MovementCompositeValidator { get; }

        /// <summary>
        /// Gets the event composite validator.
        /// </summary>
        /// <value>
        /// The event composite validator.
        /// </value>
        ICompositeValidator<Event> EventCompositeValidator { get; }

        /// <summary>
        /// Gets the contract composite validator.
        /// </summary>
        /// <value>
        /// The contract composite validator.
        /// </value>
        ICompositeValidator<Contract> ContractCompositeValidator { get; }
    }
}

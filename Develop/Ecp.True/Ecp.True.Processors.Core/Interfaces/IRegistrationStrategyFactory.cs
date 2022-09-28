// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRegistrationStrategyFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Interfaces
{
    /// <summary>
    /// The registration strategy factory.
    /// </summary>
    public interface IRegistrationStrategyFactory
    {
        /// <summary>
        /// Gets the movement registration strategy.
        /// </summary>
        /// <value>
        /// The movement registration strategy.
        /// </value>
        IRegistrationStrategy MovementRegistrationStrategy { get; }

        /// <summary>
        /// Gets the ownership registration strategy.
        /// </summary>
        /// <value>
        /// The ownership registration strategy.
        /// </value>
        IRegistrationStrategy OwnershipRegistrationStrategy { get; }

        /// <summary>
        /// Gets the inventory product registration strategy.
        /// </summary>
        /// <value>
        /// The inventory product registration strategy.
        /// </value>
        IRegistrationStrategy InventoryProductRegistrationStrategy { get; }

        /// <summary>
        /// Gets the event registration strategy.
        /// </summary>
        /// <value>
        /// The event registration strategy.
        /// </value>
        IRegistrationStrategy EventRegistrationStrategy { get; }

        /// <summary>
        /// Gets the contract registration strategy.
        /// </summary>
        /// <value>
        /// The contract registration strategy.
        /// </value>
        IRegistrationStrategy ContractRegistrationStrategy { get; }
    }
}

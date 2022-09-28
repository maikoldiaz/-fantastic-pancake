// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMovementRegistrationService.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The IMovementRegistrationService Service class.
    /// </summary>
    public interface IMovementRegistrationService
    {
        /// <summary>
        /// Registers the movement asynchronous.
        /// </summary>
        /// <param name="movementToRegister">The movement to register.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="validateTransferPoint">if set to <c>true</c> [validate transfer point].</param>
        /// <returns>The movementTransactionId.</returns>
        Task<int> RegisterMovementAsync(Movement movementToRegister, IUnitOfWork unitOfWork, bool validateTransferPoint);

        /// <summary>
        /// Update the executor asynchronous.
        /// </summary>
        /// <param name="movementToUpdate">The movementToUpdate.</param>
        /// <param name="movementReceived">The movementReceived.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>The Task.</returns>
        Task UpdateMovementOffChainDbAsync(Movement movementToUpdate, Movement movementReceived, IUnitOfWork unitOfWork);

        /// <summary>
        /// Gets the movements with sap tracking asynchronous.
        /// </summary>
        /// <param name="movementId">The movement identifier.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>The movement collection.</returns>
        Task<IEnumerable<Movement>> GetMovementsWithSapTrackingAsync(string movementId, IUnitOfWork unitOfWork);
    }
}

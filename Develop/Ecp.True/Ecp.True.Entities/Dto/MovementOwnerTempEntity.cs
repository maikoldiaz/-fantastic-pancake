// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementOwnerTempEntity.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Interfaces;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The MovementOwnerTempEntity.
    /// </summary>
    public class MovementOwnerTempEntity : ITempEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementOwnerTempEntity"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="tempId">The temporary identifier.</param>
        /// <param name="createdBy">The created by.</param>
        public MovementOwnerTempEntity(Owner owner, int tempId, string createdBy)
        {
            ArgumentValidators.ThrowIfNull(owner, nameof(owner));

            this.TempId = tempId;
            this.OwnerId = owner.OwnerId;
            this.OwnershipValue = owner.OwnershipValue;
            this.OwnershipValueUnit = owner.OwnershipValueUnit;
            this.BlockchainStatus = (int)StatusType.PROCESSING;
            this.CreatedBy = createdBy;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int? Id { get; }

        /// <summary>
        /// Gets or sets the temporary identifier.
        /// </summary>
        /// <value>
        /// The temporary identifier.
        /// </value>
        public int TempId { get; set; }

        /// <summary>
        /// Gets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int OwnerId { get; }

        /// <summary>
        /// Gets the ownership value.
        /// </summary>
        /// <value>
        /// The ownership value.
        /// </value>
        public decimal? OwnershipValue { get; }

        /// <summary>
        /// Gets the ownership value unit.
        /// </summary>
        /// <value>
        /// The ownership value unit.
        /// </value>
        public string OwnershipValueUnit { get; }

        /// <summary>
        /// Gets or sets the blockchain status.
        /// </summary>
        /// <value>
        /// The blockchain status.
        /// </value>
        public int BlockchainStatus { get; set; }

        /// <summary>
        /// Gets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public string CreatedBy { get; }
    }
}

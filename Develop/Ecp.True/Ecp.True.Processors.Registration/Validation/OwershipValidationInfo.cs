// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwershipValidationInfo.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace Ecp.True.Processors.Registration.Validation
{
    using System.Collections.Generic;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The ownership validation info DTO.
    /// </summary>
    public class OwershipValidationInfo
    {
        private OwershipValidationInfo(ICollection<Owner> owners, decimal? totalVolume, CategoryElement segment, ScenarioType scenarioType)
        {
            this.Owners = owners;
            this.TotalVolume = totalVolume.GetValueOrDefault();
            this.DeviationPercentage = segment?.DeviationPercentage ?? 0;
            this.ScenarioType = scenarioType;
            this.Segment = segment;
        }

        /// <summary>
        /// Gets the owners collection.
        /// </summary>
        public ICollection<Owner> Owners { get; }

        /// <summary>
        /// Gets the total volume.
        /// </summary>
        public decimal TotalVolume { get; }

        /// <summary>
        /// Gets the deviation percentage.
        /// </summary>
        public decimal DeviationPercentage { get; }

        /// <summary>
        /// Gets he scenario type.
        /// </summary>
        public ScenarioType ScenarioType { get; }

        /// <summary>
        /// Gets the segment.
        /// </summary>
        public CategoryElement Segment { get; }

        /// <summary>
        /// Creates a new validation info DTO from an inventory.
        /// </summary>
        /// <param name="inventory">The inventory.</param>
        /// <param name="segment">The segment.</param>
        /// <returns>The Ownership validation info.</returns>
        public static OwershipValidationInfo CreateFromInventory(InventoryProduct inventory, CategoryElement segment)
        {
            ArgumentValidators.ThrowIfNull(inventory, nameof(inventory));

            return new OwershipValidationInfo(
                inventory.Owners,
                inventory.ProductVolume,
                segment,
                inventory.ScenarioId);
        }

        /// <summary>
        /// Creates a new validation info DTO from an inventory.
        /// </summary>
        /// <param name="movement">The inventory.</param>
        /// <param name="segment">The segment.</param>
        /// <returns>The Ownership validation info.</returns>
        public static OwershipValidationInfo CreateFromMovement(Movement movement, CategoryElement segment)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));

            return new OwershipValidationInfo(
                movement.Owners,
                movement.NetStandardVolume,
                segment,
                movement.ScenarioId);
        }
    }
}
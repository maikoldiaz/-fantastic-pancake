// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipExtensions.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.OwnershipRules
{
    using Ecp.True.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Proxies.OwnershipRules.OfficialDeltaDataRequest;

    /// <summary>
    /// Extensions for the ownership rules proxy.
    /// </summary>
    public static class OwnershipExtensions
    {
        /// <summary>
        /// Gets whether a movement is of the given type.
        /// </summary>
        /// <param name="movement">The movement to check.</param>
        /// <param name="movementType">The movement type.</param>
        /// <returns>Returns whether a movement is of a given type.</returns>
        public static bool IsOfType(this OfficialDeltaConsolidatedMovement movement, MovementType movementType)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));

            bool valid = int.TryParse(movement.MovementTypeId, out int typeId);
            return valid && typeId == (int)movementType;
        }

        /// <summary>
        /// Gets whether a movement is of the given type.
        /// </summary>
        /// <param name="movement">The movement to check.</param>
        /// <param name="movementType">The movement type.</param>
        /// <returns>Returns whether a movement is of a given type.</returns>
        public static bool IsOfType(this PendingOfficialMovement movement, MovementType movementType)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));

            return movement.MovementTypeID == (int)movementType;
        }
    }
}
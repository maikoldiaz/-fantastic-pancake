// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageType.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Core
{
    /// <summary>
    /// The system type enumeration.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// The movement message.
        /// </summary>
        Movement = 1,

        /// <summary>
        /// The loss message.
        /// </summary>
        Loss = 2,

        /// <summary>
        /// The special movement.
        /// </summary>
        SpecialMovement = 3,

        /// <summary>
        /// The inventory message.
        /// </summary>
        Inventory = 4,

        /// <summary>
        /// The movement and inventory.
        /// </summary>
        MovementAndInventory = 5,

        /// <summary>
        /// The movement ownership
        /// </summary>
        MovementOwnership = 6,

        /// <summary>
        /// The inventory ownership
        /// </summary>
        InventoryOwnership = 7,

        /// <summary>
        /// The contract
        /// </summary>
        Contract = 8,

        /// <summary>
        /// The events
        /// </summary>
        Events = 9,

        /// <summary>
        /// The sap
        /// </summary>
        Sap = 10,

        /// <summary>
        /// The purchase
        /// </summary>
        Purchase = 11,

        /// <summary>
        /// The sale
        /// </summary>
        Sale = 12,

        /// <summary>
        /// The logistic
        /// </summary>
        Logistic = 13,
    }
}
